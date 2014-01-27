using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using LitJson;

//@TODO This Class is cancer. need refactoring!!!
public class HandMotion : MonoBehaviour {
	
	
	public float dist = 0.02f;
	public int _port = 11000;
	
	//	
	//	private Vector3 moveDirection = Vector3.zero;
	//	private Vector3 _moveDirection = Vector3.zero;
	private float rotateDirection = 0.0f;
	private Vector3 rHandV = Vector3.zero;
	private Vector3 rotateRHand = Vector3.zero;
	private Vector3 lHandV = Vector3.zero;
	private Vector3 rotateLHand = Vector3.zero;
	
	public GameObject rootObject  = null;
	private CharacterController controller;
	public GameObject rHand = null;
	private Vector3 rHandR = Vector3.zero;
	public GameObject lHand = null;
	private Vector3 lHandR = Vector3.zero;
	private Vector3 rootR = Vector3.zero;
	public string[] vecAxis = new string[3]{"z","y","x"};
	public Vector3 wpRotate = new Vector3(1f,-1f,-1f);
	public float rotateRiv = 1.0f;
	public float rotateLimit = 40.0f;
	
	private Thread trd;
	private UdpClient listener;
	
	
	private Queue<string> rHandAction = new Queue<string>();
	private Queue<string> lHandAction = new Queue<string>();

	private HandController rHandController;
	private HandController lHandController;
	
	
	//@TODO delete MoveStatus validate ,  use MainCharactorController Method
//	private MoveStatus mainController.MoveStatus;

	private MainCharacterController mainController;
	
	void Start () {
		GlobalController gc = GlobalController.getInstance();
		rootObject = gc.MainCharacter.Data.RootObject;
		if (rootObject==null) {
			controller = (CharacterController)GetComponent("CharacterController");
			rootObject = this.gameObject;
		} else {
			controller = (CharacterController)rootObject.GetComponent("CharacterController");
		}
		
//		mainController.MoveStatus = new Normal(rootObject,dist);

		mainController = GlobalController.getInstance().MainCharacter;

		trd = new Thread(new ThreadStart(this.ThreadTask));
		trd.IsBackground = true;
		trd.Start();
		
		if (rHand!=null) {
			rHandR = rHand.transform.eulerAngles;
		}
		if (lHand!=null) {
			lHandR = lHand.transform.eulerAngles;
		}
		rootR = transform.eulerAngles;

		rHandController = GlobalController.getInstance().MainCharacter.Data.RightHandController;
		lHandController = GlobalController.getInstance().MainCharacter.Data.LeftHandController;
	}
	
	void OnApplicationQuit () {
		listener.Close();
		trd.Abort();
	}
	
	// Use this for initialization
	void ThreadTask () {
		bool done = false;
		
		listener = new UdpClient(_port);
		IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, _port);
		Debug.Log (listener);
		try {
			while (!done) {
				//Console.WriteLine("Waiting for broadcast");
				byte[] bytes = listener.Receive(ref groupEP);
				string res = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
				//                    Debug.Log("Received broadcast from"+groupEP.ToString()+" ## "+res);
				remoteAction(res);
			}/**/
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		} finally {
			listener.Close();
		}
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//		Debug.Log(moveDirection);
		
		// move
		//		_moveDirection.x = moveDirection.x*dist*Time.deltaTime;
		//		_moveDirection.y = 0;
		//		_moveDirection.z = moveDirection.z*dist*Time.deltaTime;
		//		_moveDirection = Quaternion.Euler(0,transform.eulerAngles.y,0) * _moveDirection;
		//		controller.Move(_moveDirection);


//		controller.Move(mainController.MoveStatus.getMove());
		
		if (mainController.MoveStatus is Fall && isGrounded()) {
			mainController.MoveStatus = new Normal(rootObject,dist);
		}
		
		// rotate
		if (-1*rotateLimit<rotateDirection&&rotateDirection<rotateLimit) {
			rootObject.transform.Rotate(0, rotateDirection , 0);
			rotateDirection=0;
		}
		
		// hand rotate
		if (rHand!=null) {
			rotateRHand = (rHandV) + transform.eulerAngles;
			rHand.transform.eulerAngles = rotateRHand;//-rootR;
		}
		if (lHand!=null) {
			rotateLHand = (lHandV) + transform.eulerAngles;
			lHand.transform.eulerAngles = rotateLHand-rootR;
		}
		
		// action
		if (0<rHandAction.Count) action(rHand,objR,rHandAction.Dequeue());
		if (0<lHandAction.Count) action(lHand,objL,lHandAction.Dequeue());
		
		
		// GUI Out
		setGUI ();
	}
	
	void action(GameObject hand, GameObject obj,  string action) {
		switch (action) {
		case "ancer"	: this.ancer(hand,obj);break;
		default			: break;
		}
	}
	
	bool isGrounded() {
		// 鉛直下向きにGroundオブジェクトが存在するかどうかを判定
		return Physics.Raycast(rootObject.transform.position, new Vector3(0, -1, 0), 2.0f);
	}
	
	void setGUI () {
		MotionGUI.setRightAnchor((mainController.MoveStatus is AnchorLR)||(mainController.MoveStatus is AnchorR));
		MotionGUI.setLeftAnchor((mainController.MoveStatus is AnchorLR)||(mainController.MoveStatus is AnchorL));
		MotionGUI.setMessage("mode : "+(mainController.MoveStatus.GetType()));
	}
	
	
	
	private string moveDevice = "";
	private IDictionary<string,Vector3> startPosition = new Dictionary<string,Vector3>();
	private IDictionary<string,string> actionDic = new Dictionary<string,string>();
	
	// first controller
	private void remoteAction(string action) {
		if (!action.StartsWith("{")||!action.EndsWith("}")) {
			return;
		}
		JsonData d = LitJson.JsonMapper.ToObject(action);
		//Debug.Log("dataCount : "+jsonData.Count);
		string type = (string)d["type"];
		//		Debug.Log("iphone send : "+type);
		switch (type) {
		case "Dstart"	: this.setPosition((string)d["name"],int.Parse((string)d["x"]),int.Parse((string)d["y"]));break;
		case "Dend"		: this.setPosition((string)d["name"],int.Parse((string)d["x"]),int.Parse((string)d["y"]));break;
		case "move"		: this.move(this.getPosition((string)d["name"]),int.Parse((string)d["x"]),int.Parse((string)d["y"]));
			actionDic[(string)d["name"]]="move";break;
		case "turn"		: this.turn(this.getPosition((string)d["name"]),int.Parse((string)d["x"]),int.Parse((string)d["y"]));
			actionDic[(string)d["name"]]="turn";break;
		case "R" 		:
		case "right" 	: this.handRotation(rHandController,float.Parse((string)d[vecAxis[0]]),float.Parse((string)d[vecAxis[1]]),float.Parse((string)d[vecAxis[2]]));break;
		case "L" 		:
		case "left" 	: this.handRotation(lHandController,float.Parse((string)d[vecAxis[0]]),float.Parse((string)d[vecAxis[1]]),float.Parse((string)d[vecAxis[2]]));break;
		case "anchorR"	: this.setAction(true,"ancer");break;
		case "anchorL"	: this.setAction(false,"ancer");break;
		case "fly"		: this.setFly();break;
		case "holdR" 	: this.hold(rHandController);break;
		case "holdL"	: this.hold(lHandController);break;
		case "jump"		: this.jump();break;
		default			: break;//_reset();
		}
	}
	
	/**
	 * hand 1:right, 0:left
	 **/
	void setAction(bool hand, string action) {
		if (hand) {
			if (mainController.MoveStatus is AnchorR) {
				mainController.MoveStatus = new Fall(rootObject,dist,mainController.MoveStatus);
			} else if (mainController.MoveStatus is AnchorLR) {
				mainController.MoveStatus = ((AnchorLR)mainController.MoveStatus).getLeft();
			} else {
				rHandAction.Enqueue(action);
			}
		} else {
			if (mainController.MoveStatus is AnchorL) {
				mainController.MoveStatus = new Fall(rootObject,dist,mainController.MoveStatus);
			} else if (mainController.MoveStatus is AnchorLR) {
				mainController.MoveStatus = ((AnchorLR)mainController.MoveStatus).getRight();
			} else {
				lHandAction.Enqueue(action);
			}
		}
	}
	
	// second Controller (start tap motion)
	private void setPosition(string name, int x, int z) {
		startPosition[name] = new Vector3((float)x,0,(float)z);
		if (actionDic.ContainsKey(name)) {
			if (actionDic[name] == "move") {
				Vector3 vec = mainController.MoveStatus.getMoveDirection();
				mainController.MoveStatus.setMoveDirection(new Vector3(0.0f,vec.y,0.0f));
			} else if (mainController.MoveStatus is Fly && actionDic[name] == "turn") {
				Vector3 vec = mainController.MoveStatus.getMoveDirection();
				mainController.MoveStatus.setMoveDirection(new Vector3(vec.x,0.0f,vec.z));
			}
		}
		
	}
	private Vector3 getPosition (string name) {
		return startPosition[name];
	}
	
	// move
	void move(Vector3 pos, int x, int z) {
		//		moveDirection = new Vector3((float)x-pos.x,0,pos.z-(float)z);
		mainController.MoveStatus.setMove(pos,new Vector3((float)x,0.0f,(float)z));
	}
	
	// turn
	void turn(Vector3 pos, int x, int z) {
		rotateDirection = ((float)x-pos.x)*rotateRiv*0.1f;
		if (mainController.MoveStatus is Fly && mainController.MoveStatus.getMoveDirection().y == 0.0f) {
			mainController.MoveStatus.setMoveDirection(mainController.MoveStatus.getMoveDirection()+new Vector3(0.0f,(pos.z-z)*2.0f,0.0f));
		}
	}

	private void handRotation(HandController hc, float x, float y, float z) {
		hc.HoldState.setEuler(new Vector3(x*wpRotate.x,y*wpRotate.y,z*wpRotate.z));
	}

	private void hold(HandController hc) {
		if (hc.FingerState.getEuler()==Vector3.zero) {
			hc.FingerState.setEuler(new Vector3(0f,0f,60f));
		} else {
			hc.FingerState.setEuler(Vector3.zero);
		}
	}

	// right hand motion
	void rHandMotion(int x, int y, int z) {
		rHandV = new Vector3(x*wpRotate.x,y*wpRotate.y,z*wpRotate.z);
	}
	
	// left hand motion
	void lHandMotion(int x, int y, int z) {
		lHandV = new Vector3(x*wpRotate.x,y*wpRotate.y,z*wpRotate.z);
	}
	
	
	void ancerMove (Vector3 pos, int x, int z) {
		Vector3 front;
	}
	
	void setFly () {
		if (mainController.MoveStatus is Fly) {
			mainController.MoveStatus = new Fall(rootObject,dist,mainController.MoveStatus);
		}else {
			mainController.MoveStatus = new Fly(rootObject,dist);
		}
	}

	private void jump() {
		Debug.Log("JUMP");
		mainController.MoveStatus.addMoveDirection(new Vector3(0f,20f,0f));
	}
	
	public GameObject objR = null;
	public GameObject objL = null;
	
	//	private Vector3 rAncer = Vector3.zero;
	//	private Vector3 lAncer = Vector3.zero;
	
	void ancer(GameObject target, GameObject obj) {
		Ray ray = new Ray(target.transform.position,obj.transform.position-target.transform.position);
		RaycastHit hit = new RaycastHit();
		
		Debug.Log("Ray : "+ray+"   angle : "+target.transform.eulerAngles);
		// 何かにぶつかった
		if (!Physics.Raycast(ray, out hit, 1000f)) {
			return;
		}
		if (target==rHand) {
			if (mainController.MoveStatus is AnchorL) {
				SingleAnchor anc = new AnchorR(rootObject,dist,hit.point);
				mainController.MoveStatus = new AnchorLR(anc, (SingleAnchor)mainController.MoveStatus);
			} else {
				mainController.MoveStatus = new AnchorR(rootObject,dist,hit.point);
			}
		} else {
			if (mainController.MoveStatus is AnchorR) {
				SingleAnchor anc = new AnchorL(rootObject,dist,hit.point);
				mainController.MoveStatus = new AnchorLR((SingleAnchor)mainController.MoveStatus, anc);
			} else {
				mainController.MoveStatus = new AnchorL(rootObject,dist,hit.point);
			}
		}		
	}
	
}
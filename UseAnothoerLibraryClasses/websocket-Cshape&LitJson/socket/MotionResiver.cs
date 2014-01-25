using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using LitJson;


public class MotionResiver : MonoBehaviour {
	
	
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
	public Vector3 wpRotate = new Vector3(0.0f,-1f,1f);
	public float rotateRiv = 1.0f;
	public float rotateLimit = 40.0f;
	
	private Thread trd;
	private UdpClient listener;
	
	
	private Queue<string> rHandAction = new Queue<string>();
	private Queue<string> lHandAction = new Queue<string>();
	
	
	private MoveStatus moveMode;
	
	void Start () {
		if (rootObject==null) {
			controller = (CharacterController)GetComponent("CharacterController");
			rootObject = this.gameObject;
		} else {
			controller = (CharacterController)rootObject.GetComponent("CharacterController");
		}
		
		moveMode = new Normal(rootObject,dist);
		
		
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
	}
	
	void OnApplicationQuit () {
		if (listener!=null) {
			listener.Close();
		}
		if (trd!=null) {
			trd.Abort();
		}
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
		controller.Move(moveMode.getMove());
		
		if (moveMode is Fall && isGrounded()) {
			moveMode = new Normal(rootObject,dist);
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
		MotionGUI.setRightAnchor((moveMode is AnchorLR)||(moveMode is AnchorR));
		MotionGUI.setLeftAnchor((moveMode is AnchorLR)||(moveMode is AnchorL));
		MotionGUI.setMessage("mode : "+(moveMode.GetType()));
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
		case "right" 	: this.rHandMotion(int.Parse((string)d["x"]),int.Parse((string)d["y"]),int.Parse((string)d["z"]));break;
		case "left" 	: this.lHandMotion(int.Parse((string)d["x"]),int.Parse((string)d["y"]),int.Parse((string)d["z"]));break;
		case "anchorR"	: this.setAction(true,"ancer");break;
		case "anchorL"	: this.setAction(false,"ancer");break;
		case "fly"		: this.setFly();break;
		default			: break;//_reset();
		}
	}
	
	/**
	 * hand 1:right, 0:left
	 **/
	void setAction(bool hand, string action) {
		if (hand) {
			if (moveMode is AnchorR) {
				moveMode = new Fall(rootObject,dist,moveMode);
			} else if (moveMode is AnchorLR) {
				moveMode = ((AnchorLR)moveMode).getLeft();
			} else {
				rHandAction.Enqueue(action);
			}
		} else {
			if (moveMode is AnchorL) {
				moveMode = new Fall(rootObject,dist,moveMode);
			} else if (moveMode is AnchorLR) {
				moveMode = ((AnchorLR)moveMode).getRight();
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
			Vector3 vec = moveMode.getMoveDirection();
			moveMode.setMoveDirection(new Vector3(0.0f,vec.y,0.0f));
		} else if (moveMode is Fly && actionDic[name] == "turn") {
			Vector3 vec = moveMode.getMoveDirection();
			moveMode.setMoveDirection(new Vector3(vec.x,0.0f,vec.z));
		}
		}
		
	}
	private Vector3 getPosition (string name) {
		return startPosition[name];
	}
	
	// move
	void move(Vector3 pos, int x, int z) {
//		moveDirection = new Vector3((float)x-pos.x,0,pos.z-(float)z);
		moveMode.setMove(pos,new Vector3((float)x,0.0f,(float)z));
	}
	
	// turn
	void turn(Vector3 pos, int x, int z) {
		rotateDirection = ((float)x-pos.x)*rotateRiv*0.1f;
		if (moveMode is Fly && moveMode.getMoveDirection().y == 0.0f) {
			moveMode.setMoveDirection(moveMode.getMoveDirection()+new Vector3(0.0f,(pos.z-z)*2.0f,0.0f));
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
		if (moveMode is Fly) {
			moveMode = new Fall(rootObject,dist,moveMode);
		}else {
			moveMode = new Fly(rootObject,dist);
		}
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
			if (moveMode is AnchorL) {
				SingleAnchor anc = new AnchorR(rootObject,dist,hit.point);
				moveMode = new AnchorLR(anc, (SingleAnchor)moveMode);
			} else {
		   		moveMode = new AnchorR(rootObject,dist,hit.point);
			}
		} else {
			if (moveMode is AnchorR) {
				SingleAnchor anc = new AnchorL(rootObject,dist,hit.point);
				moveMode = new AnchorLR((SingleAnchor)moveMode, anc);
			} else {
				moveMode = new AnchorL(rootObject,dist,hit.point);
			}
		}		
	}

}

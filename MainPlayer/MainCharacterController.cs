using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class MainCharacterController : MonoBehaviour {

	public GameObject mainCamera;

	public bool mainPlayer = false;
	public float dist = 0.02f;
	
	public MainCharacterData data = new MainCharacterData();
	public MainCharacterData Data {
		get{return this.data;}
	}

	private MainMotionController motionController;
	public MainMotionController MotionController {
		get{return this.motionController;}
	}
	
	private CharacterController controller;
	public CharacterController Controller {
		get{return controller;}
	}

	private MoveStatus moveStatus = null;
	public MoveStatus MoveStatus {
		set{this.moveStatus = value;}
		get{return this.moveStatus;}
	}

	private RotateStatus rotateStatus = new RotateStatus();

	private RotationState[] jointRotations;

//	private MoveAnimatorController moveAnimator;
	
	public void Awake() {
		if (!data.MainPlayer) {
			data.MainPlayer = this.gameObject;
		}
		if (!data.RootObject) {
			data.RootObject = this.gameObject;
		}

		controller = (CharacterController)data.RootObject.GetComponent("CharacterController");

		moveStatus = new Normal(data.RootObject,dist);

		int playerID = -1;
		if (mainPlayer) {
			GlobalController.getInstance().MainCharacter = this;
			playerID = 0;
			motionController = MainMotionController.AddComponent(data.MainPlayer,this);
		} else {
			playerID = GlobalController.getInstance().addPlayer(this);
		}
		data.init(playerID);

		jointRotations = new RotationState[(int)(Enum.GetNames(typeof(PlayerJoint)).Length)];
		for (int i=0; i<jointRotations.Length; i++) {
			jointRotations[i] = new RotationState();
		}
	}
	
	void Start () {
		if (mainCamera) {
			CameraPosition.AddComponent(mainCamera,data.RootObject,data.getJoint((int)PlayerJoint.Head),new Vector3(0f,0.11f,-0.037f));
		}
//		Animator ani = GetComponent<Animator>();
//		moveAnimator = new MoveAnimatorController(ani);

	}
	
	void LateUpdate () {
		Vector3 moveDirection = moveStatus.getMove();
		controller.Move(moveDirection);
//		moveAnimator.animation(moveDirection);
		
		if (moveStatus is Fall && controller.isGrounded) {
			moveStatus = new Normal(data.rootObject,dist);
		}

		data.RightHandController.rotateJoints();
		data.LeftHandController.rotateJoints();

		for (int i=0; i<jointRotations.Length; i++) {
			if (jointRotations[i].isChange()) {
//				Debug.Log(i+"   "+jointRotations[i].getRotation());
				data.setRotation(i, jointRotations[i].getRotation());
			}
		}
	}

	public void move(Vector3 vec) {
		controller.Move(vec);
	}
	
	public void rotate(Vector3 eulerAngle) {
		data.RootObject.transform.Rotate(eulerAngle);
	}


	public RotationState[] getJointRotationManager() {
		return jointRotations;
	}
//	public GameObject getJoint(int i) {
//		return data.getJoint(i);
//	}
//	public void setPosition(int i, Vector3 position) {
//		data.setPosition(i,position);
//	}
//	
//	public void setAngle(int i, Vector3 eulerAngle) {
//		data.setAngle(i,eulerAngle);
//	}
//	
//	public void setRotation(int i, Quaternion rotation) {
//		data.setRotation(i,rotation);
//	}
	
	
}

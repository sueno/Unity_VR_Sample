using UnityEngine;
using System;
using System.Collections;

enum PlayerJoint {
	Head,
    Neck,
    Torso,
    Waist,
    LeftCollar,
    LeftShoulder,
    LeftElbow,
    LeftWrist,
    LeftHand,
    LeftFingertip,
    RightCollar,
    RightShoulder,
    RightElbow,
   	RightWrist,
    RightHand,
    RightFingertip,
	LeftHip,
    LeftKnee,
    LeftAnkle,
    LeftFoot,
    RightHip,
    RightKnee,
    RightAnkle,
    RightFoot,
	RightHandHold,
	LeftHandHold,
	Eyes,
	RightEye,
	LeftEye,
	Expression
}


[System.Serializable]
public class MainCharacterData {
	
	public PlayerMaterial playerStatus = new PlayerMaterial();
	
	public GameObject rootObject = null;
	public GameObject mainPlayer = null;

	private int playerID = -1;
	
    private GameObject[] gameObjects;
    private Quaternion[] initialRotations;

	private bool[] moveFilter;
	
    public GameObject Head = null;
    public GameObject Neck = null;
    public GameObject Torso = null;
    public GameObject Waist = null;

    public GameObject LeftCollar = null;
    public GameObject LeftShoulder = null;
    public GameObject LeftElbow = null;
    public GameObject LeftWrist = null;
    public GameObject LeftHand = null;
    public GameObject LeftFingertip = null;

    public GameObject RightCollar = null;
    public GameObject RightShoulder = null;
    public GameObject RightElbow = null;
    public GameObject RightWrist = null;
    public GameObject RightHand = null;
    public GameObject RightFingertip = null;

    public GameObject LeftHip = null;
    public GameObject LeftKnee = null;
    public GameObject LeftAnkle = null;
    public GameObject LeftFoot = null;

    public GameObject RightHip = null;
    public GameObject RightKnee = null;
    public GameObject RightAnkle = null;
    public GameObject RightFoot = null;
	
	public GameObject RightHandHold = null;
	public GameObject LeftHandHold = null;
	public GameObject Eyes = null;
	public GameObject RightEye = null;
	public GameObject LeftEye = null;
	public GameObject Expression = null;

	public HandController RightHandController;
	public HandController LeftHandController;
	
    public MainCharacterData() {
		
	}
	
	public void init (int id) {
		playerID = id;

		playerStatus.init(mainPlayer);
        int jointCount = Enum.GetNames(typeof(PlayerJoint)).Length;
		
	    gameObjects = new GameObject[jointCount];
        initialRotations = new Quaternion[jointCount];

		moveFilter = new bool[jointCount];

        gameObjects[(int)PlayerJoint.Head] = Head;
        gameObjects[(int)PlayerJoint.Neck] = Neck;
        gameObjects[(int)PlayerJoint.Torso] = Torso;
        gameObjects[(int)PlayerJoint.Waist] = Waist;
        gameObjects[(int)PlayerJoint.LeftCollar] = LeftCollar;
        gameObjects[(int)PlayerJoint.LeftShoulder] = LeftShoulder;
        gameObjects[(int)PlayerJoint.LeftElbow] = LeftElbow;
        gameObjects[(int)PlayerJoint.LeftWrist] = LeftWrist;
        gameObjects[(int)PlayerJoint.LeftHand] = LeftHand;
        gameObjects[(int)PlayerJoint.LeftFingertip] = LeftFingertip;
        gameObjects[(int)PlayerJoint.RightCollar] = RightCollar;
        gameObjects[(int)PlayerJoint.RightShoulder] = RightShoulder;
        gameObjects[(int)PlayerJoint.RightElbow] = RightElbow;
        gameObjects[(int)PlayerJoint.RightWrist] = RightWrist;
        gameObjects[(int)PlayerJoint.RightHand] = RightHand;
        gameObjects[(int)PlayerJoint.RightFingertip] = RightFingertip;
        gameObjects[(int)PlayerJoint.LeftHip] = LeftHip;
        gameObjects[(int)PlayerJoint.LeftKnee] = LeftKnee;
        gameObjects[(int)PlayerJoint.LeftAnkle] = LeftAnkle;
        gameObjects[(int)PlayerJoint.LeftFoot] = LeftFoot;
        gameObjects[(int)PlayerJoint.RightHip] = RightHip;
        gameObjects[(int)PlayerJoint.RightKnee] = RightKnee;
        gameObjects[(int)PlayerJoint.RightAnkle] = RightAnkle;
		gameObjects[(int)PlayerJoint.RightFoot] = RightFoot;
		gameObjects[(int)PlayerJoint.RightHandHold] = RightHandHold;
		gameObjects[(int)PlayerJoint.LeftHandHold] = LeftHandHold;
		gameObjects[(int)PlayerJoint.Eyes] = Eyes;
		gameObjects[(int)PlayerJoint.RightEye] = RightEye;
		gameObjects[(int)PlayerJoint.LeftEye] = LeftEye;
		gameObjects[(int)PlayerJoint.Expression] = Expression;

		RightHandController = new HandController(rootObject,1);
		LeftHandController = new HandController(rootObject,-1);
		setMMDJoint();

		foreach (GameObject obj in gameObjects) {
//			Debug.Log(obj);
			if (obj!=null) {
//				obj.AddComponent<Rigidbody>();
//				obj.rigidbody.useGravity = false;
//				MeshCollider mesh = obj.AddComponent<MeshCollider>();
//				obj.collider.isTrigger = true;
//				mesh.convex = true;
//				obj.AddComponent<MobParts>();
//				obj.tag = "joint";
//				MobParts part = obj.GetComponent<MobParts>();
//				if (part!=null) {
//					playerStatus.DataSource = part;
////					Debug.Log (playerStatus+"  "+obj.name);
//				}
//				if(obj.transform.collider&&rootObject.transform.collider) {
//					Physics.IgnoreCollision(rootObject.transform.collider,obj.transform.collider);
//								}

////				Collider[] collider1 = rootObject.GetComponents<Collider>();
//				Collider c1 = rootObject.GetComponent<CharacterController>();
//				Collider[] collider2 = obj.GetComponents<Collider>();
//				Debug.Log("IGNORE  : "+c1+"  ::  "+obj.rigidbody.collider);
////				foreach (Collider c1 in collider1) {
//					foreach (Collider c2 in collider2) {
//					Physics.IgnoreCollision(c1,c2);
//					Physics.IgnoreCollision(c2,c1);
//					}
////				}
			}
		}
	}
	// for MMD
	private void setMMDJoint() {
		MMDJoint jo = new MMDJoint(rootObject);
		GameObject[] joints = jo.registGameObj(mainPlayer);
		for (int i=0;i<gameObjects.Length;i++) {
			if (gameObjects[i]) {
				return;
			}
		}
		gameObjects = joints;
		Debug.Log("MMDModel : "+rootObject.name+"  , playerID : "+playerID);
		jo.registListener(playerID,rootObject,playerStatus);

		RightHandController.registGameObj(rootObject);
		LeftHandController.registGameObj(rootObject);
	}
	
	public GameObject RootObject {
		set{this.rootObject = value;}
    	get{return this.rootObject;}
  	}
	public GameObject MainPlayer {
		set{this.mainPlayer = value;}
    	get{return this.mainPlayer;}
  	}
	
	public GameObject getJoint(int i) {
//		Debug.Log("getJoint (i : "+i+"  ::::  "+gameObjects[i]);
		return gameObjects[i];
	}

	// for MMD
	public MorphManager getMorphManager() {
		if (!gameObjects[(int)PlayerJoint.Expression]) {
			return null;
		}
		return (MorphManager)gameObjects[(int)PlayerJoint.Expression].GetComponent<MorphManager>();
	}
	
	public void setPosition(int i, Vector3 position) {
		gameObjects[i].transform.position = position;
	}
	
	public void setAngle(int i, Vector3 eulerAngle) {
		gameObjects[i].transform.Rotate(eulerAngle);
	}
	
	public void setRotation(int i, Quaternion rotation) {
		if (gameObjects[i])
		gameObjects[i].transform.rotation = rotation;
	}

	public void moveTrue(params int[] nums) {
		foreach (int n in nums) {
			moveFilter[n] = true;
		}
	}
	
	public void moveFalse(params int[] nums) {
		foreach (int n in nums) {
			moveFilter[n] = false;
		}
	}

	public void setMoveFilter(bool[] filter) {
		for (int i=0; i<filter.Length; i++) {
			moveFilter[i] = filter[i];
		}
	}
}

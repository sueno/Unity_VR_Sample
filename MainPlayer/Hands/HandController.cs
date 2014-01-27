using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HandController{

	private int handIndex;
	private string handName;
	private string handRootName;
	private string handCollisionName;
	private List<string> jointNameList = 
	new List<string>(){"親指","親指２","人指１","人指２","人指３","中指１","中指２","中指３",
						"薬指１","薬指２","薬指３","小指１","小指２","小指３"};

	public string rightHandName = "右手首";
	public string leftHandName = "左手首";
	public string rightHandCollisionName = "r右手";
	public string leftHandCollisionName = "r左手";

	private RotationState fingerState;// = new RotationState();
	public RotationState FingerState {
		get{return fingerState;}
	}
	
	public RotationState holdState = new RotationState();
	public RotationState HoldState {
		get{return holdState;}
	}

	private Quaternion handRotationRivision;// = new Vector3(180f,30f,-30f);

	//@TODO change private
	private GameObject rootObj;
	public GameObject handObj = null;
	public GameObject[] joints;
	public GameObject holdObj;
	public HandCollision objCol;
	
	public HandController(int handIndex) {
//		this.rootObj = GlobalController.getInstance().MainCharacter.Data.RootObject;
		this.handIndex = 0<handIndex ? 1:-1;
		this.handName = 0<handIndex ? "右":"左";
		handRotationRivision = Quaternion.Euler(new Vector3(180f,30f*handIndex,-30f*handIndex));
		fingerState = new RotationState(new Vector3(1,1,handIndex));
//		holdState = new RotationState(new Vector3(1,1,1));
//		joints = new GameObject[jointNameList.Count];
		for (int i=0;i<jointNameList.Count;i++) {
			jointNameList[i] = handName+jointNameList[i];
		}
//		if (handObj) objCol = handObj.AddComponent<HandCollision>();
	}


	
	public GameObject[] registGameObj(GameObject rootObj) {
		this.rootObj = rootObj;
		this.handRootName = 0<handIndex ? rightHandName:leftHandName;
		this.handCollisionName = 0<handIndex ? rightHandCollisionName:leftHandCollisionName;
		joints = new GameObject[jointNameList.Count];
		registGameObj_recursion(rootObj);
		return joints;
	}
	public void registGameObj_recursion(GameObject obj) {
		foreach (Transform child in obj.transform) {
			//			Debug.Log("child name : "+child.name+"   "+jointNameList.Contains(child.name)+"   ("+jointNameList.IndexOf(child.name));
			if (jointNameList.Contains(child.name)) {
				joints[jointNameList.IndexOf(child.name)] = child.gameObject;
			} else if (child.name==handRootName) {
				child.gameObject.transform.rotation = Quaternion.identity;
				handObj = child.gameObject;
			} else if (child.name==handCollisionName) {
				child.gameObject.AddComponent<Rigidbody>();
				child.rigidbody.useGravity = false;
				child.rigidbody.isKinematic = true;
				objCol = child.gameObject.AddComponent<HandCollision>();
			}
			registGameObj_recursion(child.gameObject);
		}
	}

	public void rotateJoints() {
		if (fingerState.isChange()) {
			if (!objCol) {
			} else if (fingerState.getEuler()!=Vector3.zero) {
				holdObj = objCol.getCollisionObj();
				if (holdObj) {
					holdObj.transform.rotation = rootObj.transform.rotation;
					holdObj.transform.parent = joints[5].transform;
					holdObj.transform.localPosition = new Vector3(0.01f*handIndex,-0.03f,0f);
					if (holdObj.rigidbody) {
						holdObj.rigidbody.useGravity = false;
						holdObj.rigidbody.isKinematic = true;
					}
					Debug.Log("Hold : "+holdObj);

				}
			} else if (holdObj){
				holdObj.transform.parent = null;
				if (holdObj.rigidbody) {
					holdObj.rigidbody.useGravity = true;
					holdObj.rigidbody.isKinematic = false;
				}
				holdObj = null;
			}
			foreach (GameObject joint in joints) {
				if (joint)
					joint.transform.localRotation = Quaternion.Euler(fingerState.getEuler());
			}
		}
		Vector3 rotateDirection = new Vector3(0,rootObj.transform.eulerAngles.y,0);
		if (holdObj) {
			holdObj.transform.rotation = Quaternion.Slerp(holdObj.transform.rotation, Quaternion.Euler(holdState.getEuler()+rotateDirection), 0.2f);
		}
		handObj.transform.rotation = Quaternion.Slerp(handObj.transform.rotation, Quaternion.Euler(holdState.getEuler()+rotateDirection)*handRotationRivision, 0.2f);//+rotateDirection);
	}

}

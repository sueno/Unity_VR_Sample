using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HandController{

	private int handIndex;
	private string handName;
	private List<string> jointNameList = 
	new List<string>(){"親指","親指２","人指１","人指２","人指３","中指１","中指２","中指３",
						"薬指１","薬指２","薬指３","小指１","小指２","小指３"};

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
	
	public HandController(GameObject rootObj, int handIndex) {
		this.rootObj = rootObj;
		this.handIndex = 0<handIndex ? 1:-1;
		this.handName = 0<handIndex ? "右":"左";
		handRotationRivision = Quaternion.Euler(new Vector3(180f,30f*handIndex,-30f*handIndex));
		fingerState = new RotationState(new Vector3(1,1,handIndex));
//		holdState = new RotationState(new Vector3(1,1,1));
		joints = new GameObject[jointNameList.Count];
		for (int i=0;i<jointNameList.Count;i++) {
			jointNameList[i] = handName+jointNameList[i];
		}
//		if (handObj) objCol = handObj.AddComponent<HandCollision>();
	}
	
	public GameObject[] registGameObj(GameObject obj) {
		foreach (Transform child in obj.transform) {
			//			Debug.Log("child name : "+child.name+"   "+jointNameList.Contains(child.name)+"   ("+jointNameList.IndexOf(child.name));
			if (jointNameList.Contains(child.name)) {
				joints[jointNameList.IndexOf(child.name)] = child.gameObject;
			} else if (child.name==handName+"手首") {
				child.gameObject.transform.rotation = Quaternion.identity;
				handObj = child.gameObject;
			} else if (child.name=="r"+handName+"手首") {
				child.gameObject.AddComponent<Rigidbody>();
				child.rigidbody.useGravity = false;
				child.rigidbody.isKinematic = true;
				objCol = child.gameObject.AddComponent<HandCollision>();
			}
			registGameObj(child.gameObject);
		}
		return joints;
	}

	public void rotateJoints() {
		if (fingerState.isChange()) {
			if (fingerState.getEuler()!=Vector3.zero) {
				holdObj = objCol.getCollisionObj();
				if (holdObj) {
					holdObj.transform.rotation = rootObj.transform.rotation;
					holdObj.transform.parent = joints[5].transform;
					holdObj.transform.localPosition = new Vector3(0.01f*handIndex,-0.03f,0f);
					Debug.Log("Hold : "+holdObj);

				}
			} else if (holdObj){
				holdObj.transform.parent = null;
				holdObj = null;
			}
			foreach (GameObject joint in joints) {
				if (joint)
					joint.transform.localRotation = Quaternion.Euler(fingerState.getEuler());
			}
		}
		Vector3 rotateDirection = new Vector3(0,rootObj.transform.eulerAngles.y,0);
		if (holdObj) {
//			holdObj.transform.rotation = holdState.getRotation();
//			holdObj.transform.rotation = Quaternion.Slerp(holdObj.transform.rotation, holdState.getRotation(), 0.2f);
//			holdObj.transform.eulerAngles += rotateDirection;
			
			holdObj.transform.rotation = Quaternion.Slerp(holdObj.transform.rotation, Quaternion.Euler(holdState.getEuler()+rotateDirection), 0.2f);
		}
//		handObj.transform.rotation = holdState.getQuatenion()*handRotationRivision;
//		handObj.transform.rotation = Quaternion.Slerp(handObj.transform.rotation, (holdState.getRotation()*handRotationRivision), 0.2f);
//		handObj.transform.eulerAngles += rotateDirection;
		//		handObj.transform.Rotate(0, rotateDirection , 0);
		handObj.transform.rotation = Quaternion.Slerp(handObj.transform.rotation, Quaternion.Euler(holdState.getEuler()+rotateDirection)*handRotationRivision, 0.2f);//+rotateDirection);
	}

}

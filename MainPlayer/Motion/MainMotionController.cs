using UnityEngine;
using System;
using System.Collections;

public class MainMotionController : MonoBehaviour {
	
	private MainCharacterController mainCharacter;
	GameObject rootObj;
	// Use this for initialization
	void Start () {
		rootObj = mainCharacter.Data.RootObject;
		
//		// create Observer
//		GameObject rHandObj = mainCharacter.Data.getJoint((int)PlayerJoint.RightHand);
//		Observer motionController = new MotionHandlerView(new Hand(rHandObj));
//		Observable rightHand = (Observable)MotionHandler.AddComponent(rootObj,rHandObj);
//		// add observer
//		motionController.DataSource = rightHand;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static MainMotionController AddComponent (GameObject target, MainCharacterController mainCharactor) {
		MainMotionController gameObject = target.AddComponent<MainMotionController>();
		gameObject.mainCharacter = mainCharactor;
		return gameObject;
	}
	
	public bool setMotionHandler(JointInterface target, int joint) {
		GameObject jointObj = mainCharacter.Data.getJoint(joint);
		return setMotionHandler(target,jointObj);
	}
	public bool setMotionHandler(JointInterface target, GameObject jointObj) {
		try {
			rootObj = mainCharacter.Data.RootObject;
			Observer motionController = new MotionHandlerView(target);
			Observable observe = (Observable)MotionHandler.AddComponent(rootObj,jointObj);
			motionController.DataSource = observe;
			return true;
		} catch (Exception ex) {
			Debug.Log(ex);
		}
		return false;
	}
}

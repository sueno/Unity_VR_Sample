using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CustomZigSkeleton : MonoBehaviour
{
	public Transform Head;
	public Transform Neck;
	public Transform Torso;
	public Transform Waist;
	
	public Transform LeftCollar;
	public Transform LeftShoulder;
	public Transform LeftElbow;
	public Transform LeftWrist;
	public Transform LeftHand;
	public Transform LeftFingertip;
	
	public Transform RightCollar;
	public Transform RightShoulder;
	public Transform RightElbow;
	public Transform RightWrist;
	public Transform RightHand;
	public Transform RightFingertip;
	
	public Transform LeftHip;
	public Transform LeftKnee;
	public Transform LeftAnkle;
	public Transform LeftFoot;
	
	public Transform RightHip;
	public Transform RightKnee;
	public Transform RightAnkle;
	public Transform RightFoot;
	//edit start 9/21 {
	//public bool mirror = false;
	public bool mirror = false;
	//} 9/21
	public bool UpdateJointPositions = false;
	public bool UpdateRootPosition = true;
	public bool UpdateOrientation = true;
	public bool RotateToPsiPose = false;
	public float RotationDamping = 10.0f;
	public float Damping = 3.0f;
	public Vector3 Scale = new Vector3(0.001f, 0.001f, 0.001f);
	
	public Vector3 PositionBias = Vector3.zero;
	
	public Transform[] transforms;
	private Quaternion[] initialRotations;
	private Vector3 rootPosition;
	
	
	//TODO add
	private Boolean motionDump = false;
	private float dumpTime = 0.0f;
	private float chara_height;
	private bool down = false;
	private bool jump = false;
	private float down_y = 0.0f;
	private float div_y = 0.0f;
	private float ave_y = 0.0f;
	private int count_y = 0;
	
	public Vector3 revisionPosition = new Vector3(4,4,4);
	public float heightRoot = 0.25f;
	public float heightRivision = 0.5f;
	
	private Vector3[] rivisionRotations;
	
	public bool allmirror = false;
	
	private MainCharacterController mainCharactorController;

	private RotationState[] jointRotations;
	
	
	//TODO add positionlog
	//private Vector3 revisionPosition = new Vector3(5,2,5);
	private Vector3 moveDirection = Vector3.zero;
	
//	public GameObject rootObject  = null;
//	private CharacterController controller;
	
	ZigJointId mirrorJoint(ZigJointId joint)
	{
		switch (joint)
		{
		case ZigJointId.LeftCollar:
			return ZigJointId.RightCollar;
		case ZigJointId.LeftShoulder:
			return ZigJointId.RightShoulder;
		case ZigJointId.LeftElbow:
			return ZigJointId.RightElbow;
		case ZigJointId.LeftWrist:
			return ZigJointId.RightWrist;
		case ZigJointId.LeftHand:
			return ZigJointId.RightHand;
		case ZigJointId.LeftFingertip:
			return ZigJointId.RightFingertip;
		case ZigJointId.LeftHip:
			return ZigJointId.RightHip;
		case ZigJointId.LeftKnee:
			return ZigJointId.RightKnee;
		case ZigJointId.LeftAnkle:
			return ZigJointId.RightAnkle;
		case ZigJointId.LeftFoot:
			return ZigJointId.RightFoot;
			
		case ZigJointId.RightCollar:
			return ZigJointId.LeftCollar;
		case ZigJointId.RightShoulder:
			return ZigJointId.LeftShoulder;
		case ZigJointId.RightElbow:
			return ZigJointId.LeftElbow;
		case ZigJointId.RightWrist:
			return ZigJointId.LeftWrist;
		case ZigJointId.RightHand:
			return ZigJointId.LeftHand;
		case ZigJointId.RightFingertip:
			return ZigJointId.LeftFingertip;
		case ZigJointId.RightHip:
			return ZigJointId.LeftHip;
		case ZigJointId.RightKnee:
			return ZigJointId.LeftKnee;
		case ZigJointId.RightAnkle:
			return ZigJointId.LeftAnkle;
		case ZigJointId.RightFoot:
			return ZigJointId.LeftFoot;
			
			
		default:
			return joint;
		}
	}
	
	
	public void Start()
	{
		Application.targetFrameRate = 20;
		
		int jointCount = Enum.GetNames(typeof(ZigJointId)).Length;
		
		transforms = new Transform[jointCount];
		initialRotations = new Quaternion[jointCount];
		
		//TODO add 13'11/07
		rivisionRotations = new Vector3[jointCount];
		
		
		transforms[(int)ZigJointId.Head] = Head;
		transforms[(int)ZigJointId.Neck] = Neck;
		transforms[(int)ZigJointId.Torso] = Torso;
		transforms[(int)ZigJointId.Waist] = Waist;
		transforms[(int)ZigJointId.LeftCollar] = LeftCollar;
		transforms[(int)ZigJointId.LeftShoulder] = LeftShoulder;
		transforms[(int)ZigJointId.LeftElbow] = LeftElbow;
		transforms[(int)ZigJointId.LeftWrist] = LeftWrist;
		transforms[(int)ZigJointId.LeftHand] = LeftHand;
		transforms[(int)ZigJointId.LeftFingertip] = LeftFingertip;
		transforms[(int)ZigJointId.RightCollar] = RightCollar;
		transforms[(int)ZigJointId.RightShoulder] = RightShoulder;
		transforms[(int)ZigJointId.RightElbow] = RightElbow;
		transforms[(int)ZigJointId.RightWrist] = RightWrist;
		transforms[(int)ZigJointId.RightHand] = RightHand;
		transforms[(int)ZigJointId.RightFingertip] = RightFingertip;
		transforms[(int)ZigJointId.LeftHip] = LeftHip;
		transforms[(int)ZigJointId.LeftKnee] = LeftKnee;
		transforms[(int)ZigJointId.LeftAnkle] = LeftAnkle;
		transforms[(int)ZigJointId.LeftFoot] = LeftFoot;
		transforms[(int)ZigJointId.RightHip] = RightHip;
		transforms[(int)ZigJointId.RightKnee] = RightKnee;
		transforms[(int)ZigJointId.RightAnkle] = RightAnkle;
		transforms[(int)ZigJointId.RightFoot] = RightFoot;
		
		
		
		// save all initial rotations
		// NOTE: Assumes skeleton model is in "T" pose since all rotations are relative to that pose
		foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
		{
			//add 13/12/24
			MainCharacterData data = GlobalController.getInstance().MainCharacter.Data;
			if (!transforms[(int)j]) {
				GameObject obj = (int)j>0 ? data.getJoint((int)j-1) : null;
				//				Debug.Log(obj);
				if (obj!=null) {
					transforms[(int)j] = obj.transform;
				}
			}
			
			if (transforms[(int)j])
			{
				// we will store the relative rotation of each joint from the gameobject rotation
				// we need this since we will be setting the joint's rotation (not localRotation) but we 
				// still want the rotations to be relative to our game object
				initialRotations[(int)j] = Quaternion.Inverse(transform.rotation) * transforms[(int)j].rotation;
			}
			
			//add 13/11/07
			rivisionRotations[(int)j] = transform.rotation.eulerAngles;

		}
		
		mainCharactorController = GlobalController.getInstance().MainCharacter;
		//    }
		//    void Start()
		//    {
		//        int jointCount = Enum.GetNames(typeof(ZigJointId)).Length;
		//		MainCharacterController mController = (MainCharacterController)GetComponent("MainCharacterController");
		//		
		//		int _i;
		//		for (_i=0;_i<jointCount;_i++) {
		//			if (mController!=null) {
		//				transforms[_i] = mController.getJoint(_i).transform;
		//			}
		//		}
		
		// start out in calibration pose
		if (RotateToPsiPose)
		{
			RotateToCalibrationPose();
		}
		
		//TODO add
//		if (rootObject==null) {
//			controller = (CharacterController)GetComponent("CharacterController");
//		} else {
//			controller = (CharacterController)rootObject.GetComponent("CharacterController");
//		}
		CharacterController controller = mainCharactorController.Data.RootObject.GetComponent<CharacterController>();
		chara_height = controller.height;

		
		//add 14/1/22
		jointRotations = mainCharactorController.getJointRotationManager();
	}
	
	void UpdateRoot(Vector3 skelRoot)
	{	
		// +Z is backwards in OpenNI coordinates, so reverse it
		rootPosition = Vector3.Scale(new Vector3(skelRoot.x, skelRoot.y, skelRoot.z), doMirror(Scale)) + PositionBias;
		if (UpdateRootPosition)
		{
			//TODO commentout
			if (rootPosition.z<(moveDirection.z)-1 || (moveDirection.z)+1<rootPosition.z ||
			    rootPosition.x<(moveDirection.x)-1 || (moveDirection.x)+1<rootPosition.x) {
				moveDirection = new Vector3(0,0,0);
			} else {
				moveDirection = rootPosition-moveDirection;
			}
			moveDirection = Quaternion.Euler(0,transform.eulerAngles.y,0) * moveDirection;
			
			
			//			Debug.Log(rootPosition);
			float height = (rootPosition.y+heightRoot);
			if (height<-0.3f) {
				transform.localPosition = new Vector3(0,(rootPosition.y*heightRivision)+heightRoot,0);
			}else if (0.4f<height) {
				Debug.Log("zig jump");
				Debug.Log(height);
				mainCharactorController.MoveStatus.setHeight(height*40f*heightRivision);
			} else {
				transform.localPosition = Vector3.zero;
			}
			mainCharactorController.move(new Vector3(moveDirection.x,-9.8f,moveDirection.z));
			moveDirection = rootPosition;
			
		}
	}
	
	void UpdateRotation(ZigJointId joint, Quaternion orientation)
	{
		joint = mirror ? mirrorJoint(joint) : joint;
		// make sure something is hooked up to this joint
		if (!transforms[(int)joint])
		{
			return;
		}
		
		if (UpdateOrientation)
		{
			Quaternion newRotation = transform.rotation * orientation * initialRotations[(int)joint];
			
			if (mirror)
			{
				newRotation.y = -newRotation.y;
				newRotation.z = -newRotation.z;
			}
			
			if (allmirror)
			{
				newRotation.x = -newRotation.x;
				//				newRotation.y = -newRotation.y;
				newRotation.z = -newRotation.z;
			}
			// change
			//			float dump = 1.0f;
			//			Vector3 diff = new Vector3(newRotation.x- transforms[(int)joint].rotation.x,newRotation.y- transforms[(int)joint].rotation.y,newRotation.z- transforms[(int)joint].rotation.z);
			//			if (motionDump==true&&(diff.x<-dump||dump<diff.x||diff.y<-dump||dump<diff.y||diff.y<-dump||dump<diff.y)){
			//				Debug.Log ("Dump Motion : "+diff);
			//			} else {
			//				motionDump = true;
//			transforms[(int)joint].rotation = Quaternion.Slerp(transforms[(int)joint].rotation, newRotation, Time.deltaTime * RotationDamping);
			//			}
			//TODO add 13/11/07
			//	transforms[(int)joint].Rotate(rivisionRotations[(int)joint]);

			Quaternion rot = Quaternion.Slerp(jointRotations[(int)joint-1].getRotation(), newRotation, Time.deltaTime * RotationDamping);
			jointRotations[(int)joint-1].setRotation(rot);
		}
	}
	Vector3 doMirror(Vector3 vec)
	{
		//TODO change parameter 9/21
		//			Debug.Log ("vsc : "+vec);
		return new Vector3(mirror ? -vec.x*revisionPosition.x : vec.x*revisionPosition.x, vec.y*revisionPosition.y, vec.z*revisionPosition.z);
	}
	void UpdatePosition(ZigJointId joint, Vector3 position)
	{
		joint = mirror ? mirrorJoint(joint) : joint;
		// make sure something is hooked up to this joint
		if (!transforms[(int)joint])
		{
			return;
		}
		
		if (UpdateJointPositions)
		{
			Vector3 dest = Vector3.Scale(position, doMirror(Scale)) - rootPosition;
			transforms[(int)joint].localPosition = Vector3.Lerp(transforms[(int)joint].localPosition, dest, Time.deltaTime * Damping);
		}
	}
	
	public void RotateToCalibrationPose()
	{
		foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
		{
			if (null != transforms[(int)j])
			{
				transforms[(int)j].rotation = transform.rotation * initialRotations[(int)j];
			}
		}
		
		// calibration pose is skeleton base pose ("T") with both elbows bent in 90 degrees
		if (null != RightElbow)
		{
			RightElbow.rotation = transform.rotation * Quaternion.Euler(0, -90, 90) * initialRotations[(int)ZigJointId.RightElbow];
		}
		if (null != LeftElbow)
		{
			LeftElbow.rotation = transform.rotation * Quaternion.Euler(0, 90, -90) * initialRotations[(int)ZigJointId.LeftElbow];
		}
	}
	
	public void SetRootPositionBias()
	{
		this.PositionBias = -rootPosition;
	}
	
	public void SetRootPositionBias(Vector3 bias)
	{
		this.PositionBias = bias;
	}
	
	void Zig_UpdateUser(ZigTrackedUser user)
	{
		//		Debug.Log("FPS : "+(1/Time.deltaTime));
		//		dumpTime += Time.deltaTime;
		//		if (1<dumpTime) {
		//			motionDump = false;
		//		}
		UpdateRoot(user.Position);
		if (user.SkeletonTracked)
		{
			foreach (ZigInputJoint joint in user.Skeleton)
			{
				if (joint.GoodPosition) UpdatePosition(joint.Id, joint.Position);
				if (joint.GoodRotation) UpdateRotation(joint.Id, joint.Rotation);
			}
		}
	}
	
}

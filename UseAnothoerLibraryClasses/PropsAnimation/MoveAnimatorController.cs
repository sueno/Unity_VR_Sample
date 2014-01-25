using UnityEngine;
using System.Collections;

[System.Serializable]
public class MoveAnimatorController{

	Vector3 moveDirection = Vector3.zero;
	
	Animator animator;

	private static MotionFilter motionFilter = new MotionFilter(
		new bool[] {
		true,	//		Head,
		true,	//		Neck,
		true,	//		Torso,
		false,	//		Waist,
		true,	//		LeftCollar,
		true,	//		LeftShoulder,
		true,	//		LeftElbow,
		true,	//		LeftWrist,
		true,	//		LeftHand,
		true,	//		LeftFingertip,
		true,	//		RightCollar,
		true,	//		RightShoulder,
		true,	//		RightElbow,
		true,	//		RightWrist,
		true,	//		RightHand,
		true,	//		RightFingertip,
		false,	//		LeftHip,
		false,	//		LeftKnee,
		false,	//		LeftAnkle,
		false,	//		LeftFoot,
		false,	//		RightHip,
		false,	//		RightKnee,
		false,	//		RightAnkle,
		false,	//		RightFoot,
		true,	//		RightHandHold,
		true,	//		LeftHandHold,
		true,	//		Eyes,
		true,	//		RightEye,
		true,	//		LeftEye,
		true,	//		Expression
	});

	public MoveAnimatorController (Animator animator) {
		this.animator = animator;
	}


	public bool animation (Vector3 moveDirection) {
		if (moveDirection.x!=0 || moveDirection.z!=0) {
			moveDirection *= 100f;
			animator.SetFloat("MovementX", moveDirection.x);
			animator.SetFloat("MovementZ", moveDirection.z);
			return true;
		} else {
			return false;
		}
	}


	public long filter() {
		return motionFilter.Filter;
	}
}

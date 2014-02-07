using UnityEngine;
using System.Collections;
using Unity_VR.Global;

public class Enemy_1 : MonoBehaviour {
	
	private CharacterController controller;
	
	public GameObject self = null;
	public GameObject target = null;
	
	public float moveDistance = 10.0f;
	public float targetVelocity = 0.0002f;
	
	public float attackInterval = 0.001f;
	
	void Start () {
		controller = GetComponent<CharacterController>();
		
		if (self == null) {
			self = this.gameObject;
		}
		if (target == null) {
//			target = GameObject.FindWithTag("Player");
			target = GlobalController.getInstance().MainCharacter.gameObject;
		}
		
	}
	
	void Update () {
		float dis = Vector3.Distance(target.transform.position,self.transform.position);
		Vector3 _direction = (target.transform.position - this.transform.position);
		if (dis<1.8f) {
			attackInterval+=Time.deltaTime;
			if (Random.Range(0.0f,500.0f/attackInterval)<1f) {
				this.transform.LookAt(new Vector3(target.transform.position.x,this.transform.position.y,target.transform.position.z));
				self.animation.Play("attack");
				attackInterval = 0.001f;
			}
			controller.Move(new Vector3(Random.Range(0,1),0,Random.Range(0,1))*targetVelocity);
		} else if (dis<moveDistance*0.5f) {
			attackInterval+=Time.deltaTime;
			this.transform.LookAt(new Vector3(target.transform.position.x,this.transform.position.y,target.transform.position.z));
			self.animation.Play("waitingforbattle");
			controller.Move(_direction*targetVelocity*0.2f);
		} else if (dis<moveDistance) {
			this.transform.LookAt(target.transform);
			self.animation.Play("run");
			controller.Move(_direction*targetVelocity);
		} else {
			self.animation.Play("idle");
		}
	}
}

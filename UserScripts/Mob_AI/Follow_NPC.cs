using UnityEngine;
using System.Collections;
using Unity_VR.Global;

public class Follow_NPC : MonoBehaviour {
	
	private CharacterController controller;
	
	public GameObject self = null;
	public GameObject target = null;
	
	public float moveDistance = 100.0f;
	public float targetVelocity = 0.0002f;

	
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
		this.transform.LookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
		controller.Move(_direction.normalized*targetVelocity*Time.deltaTime);
	}
}

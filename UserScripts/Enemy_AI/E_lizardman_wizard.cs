using UnityEngine;
using System.Collections;
using Unity_VR.Global;

public class E_lizardman_wizard : MonoBehaviour {
	
	
	private CharacterController controller;
	
	public GameObject self = null;
	public GameObject target = null;
	
	public float moveDistance = 10.0f;
	public float targetVelocity = 2000000f;
	
	public float attackInterval = 0.001f;
	public float attackFrequency = 500f;
	
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
		if (dis<10f) {
			attackInterval+=Time.deltaTime;
			if (self.animation.IsPlaying("_stay")) {
				float random = Random.Range(0.0f,attackFrequency/attackInterval);
				if (random<1f) {
//					self.animation.Play("_attack01");
//				} else if (random<1.5f) {
//					self.animation.Play("_attack02");
//				} else if (random<5.7f) {
					self.animation.Play("_attack03");
					Invoke("attackImpact",1.7f);
				} else {
				}
				if (random<1.6f) {
					this.transform.LookAt(new Vector3(target.transform.position.x,this.transform.position.y,target.transform.position.z));
				}
			} else if (!self.animation.isPlaying) {
				attackInterval = 0.001f;
				self.animation.Play("_stay");
			}
			
//			controller.Move(new Vector3(Random.Range(0,1),0,Random.Range(0,1))*targetVelocity);
		} else if (self.animation.isPlaying && !self.animation.IsPlaying("_stay") && !self.animation.IsPlaying("_move")) {
			
		} else if (dis<moveDistance*0.3f) {
			attackInterval+=Time.deltaTime;
			this.transform.LookAt(new Vector3(target.transform.position.x,this.transform.position.y,target.transform.position.z));
			self.animation.Play("_stay");
			//			controller.Move(_direction.normalized*targetVelocity);
		} else if (dis<moveDistance) {
			this.transform.LookAt(target.transform);
			self.animation.Play("_stay");//move
			//			controller.Move(_direction.normalized*targetVelocity);
		} else {
			self.animation.Play("_stay");
		}
	}
	
	public void attackImpact() {
		float dis = Vector3.Distance(target.transform.position,self.transform.position);
		Vector3[] objPos = new Vector3[3];
		Vector3 distanceVec = new Vector3(0f,0f,Random.Range(dis-3f,dis+3f));
		objPos[0] = transform.position + (Quaternion.Euler(0f,transform.eulerAngles.y,0f) * distanceVec);
		objPos[1] = transform.position + (Quaternion.Euler(0f,transform.eulerAngles.y+40f,0f) * distanceVec);
		objPos[2] = transform.position + (Quaternion.Euler(0f,transform.eulerAngles.y-40f,0f) * distanceVec);

		foreach(Vector3 pos in objPos) {
			GameObject effect = (GameObject)Resources.Load("iceBlockBase");
			GameObject eff = (GameObject)GameObject.Instantiate(effect,pos, transform.rotation);
			GameObject effect2 = (GameObject)Resources.Load("iceBlockBace_Col");
			GameObject eff2 = (GameObject)GameObject.Instantiate(effect2,pos, transform.rotation);
			Destroy(eff2,1f);
		}
	}
}

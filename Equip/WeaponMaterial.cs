using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WeaponMaterial : DefaultItem, JointInterface, ICollisionMaterial{

	//@TODO change private
	public int playerID = -1;
	public int PlayerID {
		set {playerID = value;}
		get {return playerID;}
	}

	private ICollisionMaterial colObj = null;
	public ICollisionMaterial ColObj {
		get {return colObj;}
	}

	public string w_name = "Weapon";
	public string tagName = "Weapon";
	public Weapon weapon = new Weapon ();
	public Mob owner = new Mob ();
	public Mob Owner {
		set{owner = value;}
		get{weaponState.init(weapon,owner);return owner;}
	}

	private bool isActive = false;
	public GameObject body = null;
	public GameObject parentObj = null;
	public SimpleMeshRenderer trail = null;
	private bool trail_visible = false;
	public SimpleMeshRenderer skilltrail = null;
	private bool skilltrail_visible = false;
	
	private SkillInterface activeSkill = null;
	public GameObject keepEffect;
	public GameObject collisionEffect;
	public GameObject hitEffect;

	public IWeaponStatus weaponState;

	public GameObject ParentObj {
		get{return parentObj;}
	}

	void Awake() {
		base.Awake();
	}

	void Start () {
		base.Start();

		this.name = this.w_name;
//		this.tag = this.tagName;
	
		if (!body) {
			body = this.gameObject;
		}
		if (!parentObj) {
			parentObj = this.gameObject;
		}
		
		trail_ctrl (trail_visible);
		
		bool f = GlobalController.getInstance().MainCharacter.MotionController.setMotionHandler((JointInterface)this,this.gameObject);
		Debug.Log("regist weaponMaterial : "+f);
		
		
		keepEffect = (GameObject)Resources.Load("Charge");
		collisionEffect = (GameObject)Resources.Load("CollisionEffect");
		hitEffect = (GameObject)Resources.Load("HitEffect");

		if (weaponState == null) {
			weaponState = WeaponStateFactory.newInstance(playerID);
		}
		weaponState.init(weapon,owner);
		Debug.Log(weaponState);

		if (trail&&keepEffect&&body) {
			isActive = true;
		}
	}

	void Update () {
		base.Update();
//		bool flag = addPower (body.transform.position);
		bool flag = 0<weaponState.calc(Time.deltaTime,body.transform.position);
		if (flag != trail_visible) {
			trail_visible = flag;
			trail_ctrl (trail_visible&&!skilltrail_visible);
		}

		if (isActive&&activeSkill!=null&&weaponState.isActive()&&!activeSkill.action()) {
			skilltrail_visible = false;
			resetActiveSkill();
		}
	}

	public float getDamage() {
		float damage = weaponState.getDamage();
		if (0<damage) {
			hitParticle();
		}
		return damage;
	}

	public IWeaponStatus getWeaponStatus() {
		return weaponState;
	}

	void trail_ctrl (bool flag) {
		if (trail != null) {
			trail.rendererActive(flag);
		}
	}
	
	void skilltrail_ctrl (bool flag) {
		if (skilltrail != null) {
			skilltrail.rendererActive (flag);
		}
	}
	
	public void action(int i) {
		if (skilltrail!=null) {
			return;
		}
		SkillInterface newskill = owner.skillActivate(i);
		if (newskill!=null) {
			GameObject eff = (GameObject)GameObject.Instantiate(keepEffect,parentObj.transform.position,parentObj.transform.rotation);
			Destroy(eff, 0.9f);
			skilltrail_visible = true;
			skilltrail_ctrl(skilltrail_visible);
		}
	}
	
	public void resetAction() {
//		skilltrail_visible = false;
//		skilltrail_ctrl(skilltrail_visible);	
	}
	
	private void resetActiveSkill() {
//			Debug.Log("ggg"+activeSkill);
		skilltrail_visible = false;
		skilltrail.renderInvisible();
		activeSkill = null;
	}
	
	
	private void hitParticle () {
		if (activeSkill!=null) {
			GameObject[] obj = activeSkill.hitEffect();
			foreach (GameObject o in obj) {
//				Debug.Log("obj :::: "+o);
				GameObject eff = (GameObject)GameObject.Instantiate(o,transform.position, transform.rotation);
				Destroy(eff, 0.9f);
			}
		} else {
			GameObject eff = (GameObject)GameObject.Instantiate(hitEffect,transform.position, transform.rotation*Quaternion.Euler(-90f,0f,0f));
			Destroy(eff,0.8f);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag.StartsWith("Weapon")) {
			GameObject eff = (GameObject)GameObject.Instantiate(collisionEffect,transform.position,transform.rotation);
			Destroy(eff, 0.6f);
			WeaponMaterial wm = col.GetComponent<WeaponMaterial>();
			IWeaponStatus ws = wm.getWeaponStatus();
			ws.revise(0.2f);
		}
	}

	public virtual GameObject getGameObject() {
		return parentObj;
	}

	public override void release() {
		base.release();
		collider.isTrigger = true;
		Debug.Log("Trigger : "+collider.isTrigger);
	}
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MobParts : Observable, ICollisionMaterial
{
	
	public string ignoreObjTag = "joint";
	
	public GameObject effObj = null;
	private IDictionary<string,Vector3> damagePositions = new Dictionary<string,Vector3> ();
	
	private ICollisionMaterial colObj = null;
	public ICollisionMaterial ColObj {
		get {return colObj;}
	}

	private int playerID = -1;
	public int PlayerID {
		set {playerID = value;owner=GlobalController.getInstance().getPlayer(playerID).Data.playerStatus.mob;}
		get {return playerID;}
	}

	public GameObject ParentObj {
		get{return this.gameObject;}
	}
	
	public Mob owner = null;
	public Mob Owner {
		set{}
		get{return owner;}
	}

	void Start ()
	{
		if (effObj == null) {
			effObj = GameObject.Find ("DamagePoint");
		}
		colObj = null;
	}
	
	void OnCollisionEnter (Collision col)
	{
		damagePositions [col.gameObject.name] = col.contacts[0].point;
		updateColObj(col.gameObject);
	}
	void OnCollisionExit (Collision col)
	{
//		damagePositions [col.gameObject.name] = col.contacts[0].point;
		updateColObj(col.gameObject);
	}

	void OnTriggerEnter (Collider col)
	{
		damagePositions [col.gameObject.name] = (col.transform.position+this.transform.position)/2.0f;
		updateColObj(col.gameObject);
	}

	void OnTriggerStay (Collider col)
	{
		damageEffect (col.gameObject, (col.transform.position+this.transform.position)/2.0f);
		updateColObj(col.gameObject);
	}

	void damageEffect (GameObject obj, Vector3 pos)
	{
//		Debug.Log ("hoge-----"+obj.name+"  Tag: " +obj.tag + "  igno: "+ignoreObjTag +" "+(obj.tag==ignoreObjTag)+"  eff: "+(effObj==null?"null":effObj.name));
		if (!(obj.tag == ignoreObjTag) && effObj != null) {
			Vector3 damagePosition = (Vector3)damagePositions [obj.name];
			Vector3 vec = (pos - damagePosition);
			Vector3 nvec = vec.normalized / 10.0f;
			int count = (int)(vec.magnitude * 10.0f);
		Debug.Log("mag : ");
			for (; 0<count; count--) {
				damagePosition += nvec;
				GameObject eff = (GameObject)Instantiate (effObj, damagePosition, Quaternion.identity);
				eff.transform.parent = transform.parent; 
				Destroy (eff, 0.5f);
			}
			damagePositions [obj.name] = damagePosition;
		}
	}
	
	void updateColObj(GameObject obj) {
//		if (!obj.tag.StartsWith("Weapon")) {
//			return;
//		}
//		WeaponMaterial w = obj.GetComponent<WeaponMaterial>();
//		if (w != null) {
//			colObj = w;
//			RaiseUpdate(this);
//		}

		colObj = getCollisionMaterial(obj);
		if (obj) {
			RaiseUpdate(this);
		}
	}

	private ICollisionMaterial getCollisionMaterial(GameObject obj) {
		MobParts partC = obj.GetComponent<MobParts>();
		//		Debug.Log("THIS :"+this.gameObject+"("+this.playerID+")  COL :"+obj+"("+(partC?partC.playerID+"":"-2")+")");
		if (partC) {
			if (partC.PlayerID==this.playerID) {
				return null;
			}
//			Debug.Log(partC.gameObject.name);
			if (0>partC.gameObject.name.IndexOf("手")&&0>partC.gameObject.name.IndexOf("腕")) {
				return null;
			}

			return partC;
		}

		WeaponMaterial wm = obj.GetComponent<WeaponMaterial>();

		if (wm) {
			if (wm.PlayerID == this.playerID) {
				return null;
			}

			return wm;
		}

		return null;
	}
}

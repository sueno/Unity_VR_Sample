using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Mob{
	
	
	public int maxLife = 100;
	public int life = 100;
	public int str = 20;
	public int def = 0;
	public int spd = 60;
	
	public float height  = 1.7f;
	public float weight  = 60f;
	
	public float nockback = 1.0f;
	
	public SkillList skillList = new SkillList();
	
	//public Equip equip;
	
	//public Mob (int life , int str , int def , int spd , float height , float weight) {
	//	this.maxLife = life;
	//	this.life = life;
	//	this.str = str;
	//	this.def = def;
	//	this.spd = spd;
	//	
	//	this.height = height>0 ? height:1;
	//	this.weight = weight>0 ? weight:1;
	//	
	//	this.nockback = 1.0f;
	//	
	////	this.equip = new Equip();
	//}
	
	public Mob () {
	}
	
	public bool damage (int dmg) {
		int d = dmg - this.def;
		return this.damageD(0<d?d:0);
	}
	
	public bool damageD (int dmg) {
		//		Debug.Log("Damage "+dmg);
		this.life -= dmg;
		return (0<life);
	}
	
	public float move_speed () {
		return 3 + spd*100;
	}

	//@TODO return Array or list
	public SkillInterface skillActivate (int i) {
		return skillList.skillActivate(i);
	}
}

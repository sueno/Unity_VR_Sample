using UnityEngine;
using System.Collections;

public interface IWeaponStatus {
	
	void init(Weapon weapon, Mob mob);
	float calc(float time, Vector3 point);
	float getDamage();
	void revise(float rev);
	bool isActive();
	void reset();
}

using UnityEngine;
using System;
using System.Collections;

public class DamageEvent : CollisionEventInterface {

	private Mob mob;

	public DamageEvent (Mob mob) {
		this.mob = mob;
	}

	public Type getActionType() {
		return typeof(WeaponMaterial);
	}
	
	public void action(ICollisionMaterial player, ICollisionMaterial colObj) {
		if (!(colObj is WeaponMaterial)) {
			return;
		}
		WeaponMaterial mat = colObj as WeaponMaterial;

		int damage = (int)mat.getDamage();
		mob.damage(damage);
	}
}

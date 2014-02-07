using UnityEngine;
using System.Collections;

public class WeaponStateFactory {

	public static IWeaponStatus newInstance(int player) {
		IWeaponStatus state = null;
		if (0==player) {
			state = new PlayerWeapon();
		} else {
			state = new DefaultWeaponStatus();
		}
		return state;
	}
}

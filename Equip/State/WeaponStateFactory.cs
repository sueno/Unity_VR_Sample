﻿using UnityEngine;
using System.Collections;

namespace Unity_VR.Equip.State
{

    public class WeaponStateFactory
    {

        public static IWeaponStatus newInstance(int player)
        {
            IWeaponStatus state = null;
            if (0 == player)
            {
                state = new PlayerWeapon();
            }
            else if (player <= -10000)
            {
                state = new DefaultWeaponStatus();
            }
			else
			{
				state = new DistanceWeaponStatus();
			}
            return state;
        }
    }
}
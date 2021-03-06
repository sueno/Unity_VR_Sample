using UnityEngine;
using System.Collections;

namespace Unity_VR.Equip.State
{

    [System.Serializable]
    public class DefaultWeaponStatus : WeaponStatus
    {

        public override System.Single calc(System.Single time, Vector3 point)
        {
            return base.calc(time, point);
        }

        public override float getDamage()
        {
            return base.damage;
        }
    }
}
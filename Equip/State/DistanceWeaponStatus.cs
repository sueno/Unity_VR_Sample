using UnityEngine;
using System.Collections;

namespace Unity_VR.Equip.State
{

    public class DistanceWeaponStatus : WeaponStatus
    {

        protected Vector3 point = Vector3.zero;
        protected Vector3 pointroot = Vector3.zero;

        public override System.Single calc(System.Single time, Vector3 point)
        {
            if (this.point == point)
            {
                pointroot = point;
            }
            this.point = point;
            return base.calc(time, point);
        }

        public override float getDamage()
        {
            float dis = Vector3.Distance(pointroot, point);
            pointroot = point;
            return 0.3f < dis ? base.damage : 0f;
        }
    }
}
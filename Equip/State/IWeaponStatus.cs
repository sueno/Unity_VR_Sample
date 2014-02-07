using UnityEngine;
using System.Collections;
using Unity_VR.Mobs;

namespace Unity_VR.Equip.State
{

    public interface IWeaponStatus
    {

        void init(Weapon weapon, Mob mob);
        float calc(float time, Vector3 point);
        float getDamage();
        void revise(float rev);
        bool isActive();
        void reset();
    }
}
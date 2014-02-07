using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity_VR.Mobs;

namespace Unity_VR.Equip.State
{

    [System.Serializable]
    public abstract class WeaponStatus : IWeaponStatus
    {

        protected Weapon weapon;
        protected Mob mob;
        protected float damage;
        protected float revision = 1f;

        public virtual void init(Weapon weapon, Mob mob)
        {
            this.weapon = weapon;
            this.mob = mob;
        }

        public virtual float calc(float time, Vector3 point)
        {
            damage = weapon.attack;
            return damage;
        }

        public virtual float getDamage()
        {
            float d = damage * revision;
            return 0 < d ? d : 0f;
        }

        public virtual void revise(float rev)
        {
            revision = rev;
        }

        public virtual bool isActive()
        {
            return true;
        }

        public virtual void reset()
        {
            damage = 0f;
        }
    }
}
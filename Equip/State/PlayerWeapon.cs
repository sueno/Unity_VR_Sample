using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unity_VR.Equip.State
{

    [System.Serializable]
    public class PlayerWeapon : WeaponStatus
    {

        private float revSpeed = 0.3f;
        private float revPower = 1.5f;
        private float revRotate = 50.0f;

        protected float ps = 0.0f;
        protected Vector3 point = Vector3.zero;
        protected Vector3 pointroot = Vector3.zero;
        protected float pointMagunitude = 0f;
        protected float time = 0f;
        protected float power = 0.0f;
        protected Queue<Vector3> trails = new Queue<Vector3>();

        public override float calc(float time, Vector3 point)
        {
            if (this.trails.Count < 5)
            {
                trails.Enqueue(pointroot - point);
                this.time += Time.deltaTime;
                this.point = point;
                return 0f;
            }

            Vector3 vec = (Vector3)this.trails.Dequeue();
            float border = (revRotate * Vector3.Distance(point, this.point));
            float actual = (System.Math.Abs(Vector3.Angle(this.pointroot - point, vec)));
            if (border < actual)
            {
                trails.Enqueue(pointroot - point);
                this.time += Time.deltaTime;
            }
            else
            {
				this.power = 0f;
                this.time = 0.001f;
                this.pointroot = point;
                this.pointMagunitude = 0f;
                this.trails.Clear();
            }

            this.pointMagunitude += (this.point - point).magnitude;
            this.point = point;
            Vector3 dis = (this.pointroot - this.point);

            ps = (this.pointMagunitude / this.time) - (100.0f / weapon.weight);
            //		Debug.Log((this.pointMagunitude / this.time) +"  ::  "+(100.0f / weapon.weight)+"    :::   "+ps);
            ps = (-revSpeed * ps * ps) + revPower + System.Math.Abs(dis.normalized.y * 1.0f);
            return ps;
        }

        public override float getDamage()
        {
            Vector3 dis = (this.pointroot - this.point);
            float damage = getNaturalDamage(0f);

			damage = ps * 0.8f + (0 < ps ? ps : 0) * damage * 0.2f;

            this.power = power * 0.1f;
            this.time = 0.001f;
            this.pointroot = this.point;
            Debug.Log(damage);
            return (0 < damage ? damage : 0);
        }

        public override bool isActive()
        {
            return 0 < this.trails.Count;
        }

        private float getNaturalDamage(float rev)
        {

			float power = pointMagunitude + this.power;

            float damage = weapon.attack;
            if (10 < power)
            {
                damage *= 1.5f;
            }
            else if (2 < power)
            {
                damage *= (1 + (power - 3) * 0.05f);
            }
            else if (1 < power)
            {
            }
            else if (0.3 < power)
            {
                damage *= (1 - (power - 3) * 0.05f);
            }
            else
            {
                damage *= ((power * 0.45f) * (power * 0.45f));
            }
            return damage;
        }
    }
}
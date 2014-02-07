using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity_VR.Global;
using Unity_VR.Equip;

namespace Unity_VR.Mobs
{

    [System.Serializable]
    public class mobMaterial : MonoBehaviour, ICollisionMaterial
    {

        public int playerID = -1;
        public int PlayerID
        {
            set { playerID = value; }
            get { return playerID; }
        }

        private ICollisionMaterial colObj = null;
        public ICollisionMaterial ColObj
        {
            get { return colObj; }
        }

        public GameObject mob_obj = null;
        public string mob_name = "Mob_Default";

        public string damageObjTag = "WeaponN";
        public float revision = 10.0f;

        public GameObject ParentObj
        {
            get { return this.gameObject; }
        }

        public Mob owner = new Mob();
        public Mob Owner
        {
            set { }
            get { return owner; }
        }

        void Start()
        {
            //		this.tag = "Mob";
            //	this.rigidbody.mass = mob.weight;
        }

        void Update()
        {
            if (this.transform.position.y < -50)
            {
                Destroy(this.gameObject);
            }

        }

        //	void OnControllerColliderHit(ControllerColliderHit col) {
        //		damage(col.gameObject);
        //		//damage(col);
        //		nockbak(col.gameObject);
        //		
        //		damagePositions[col.gameObject.name] = col.transform.position;
        //		damageEffect(col.gameObject,col.transform.position);
        //		Debug.Log("hit "+this.gameObject+" ("+col.gameObject);
        //
        //	}

        void OnCollisionEnter(Collision col)
        {
            damage(col.gameObject);
            //damage(col);
            nockbak(col.gameObject);

            damagePositions[col.gameObject.name] = col.transform.position;
            damageEffect(col.gameObject, col.transform.position);
            Debug.Log("hit " + this.gameObject + " (" + col.gameObject);
        }

        void OnTriggerEnter(Collider col)
        {
            damage(col.gameObject);
            nockbak(col.gameObject);
            damagePositions[col.gameObject.name] = col.transform.position;
            Debug.Log("hit " + this.gameObject + " (" + col.gameObject);
        }
        void OnTriggerStay(Collider col)
        {
            damageEffect(col.gameObject, col.transform.position);
        }


        void damage(GameObject obj)
        {
            Debug.Log("Damage    hit   " + obj);
            WeaponMaterial wm = obj.GetComponent<WeaponMaterial>();
            if (wm != null && 0 <= wm.PlayerID)
            {
                float dmg = obj.GetComponent<WeaponMaterial>().getDamage();
                //		var survive = this.gameObject.GetComponent(MobMaterial).mob.damage(dmg);
                //			Debug.Log("DAMAGE : "+(int)dmg);
                bool survive = this.owner.damage((int)dmg);
                damageMessage((int)dmg);
                if (!survive)
                {
                    //				animation.Play("_big_damage");
                    Destroy(this.gameObject, 0);
                    dethEffect();
                }
            }
        }

        void nockbak(GameObject obj)
        {
            if (obj.tag == damageObjTag)
            {
                float x = obj.transform.position.x - this.transform.position.x;
                float z = obj.transform.position.z - this.transform.position.z;
                float per = x < z ? x : z;
                x = x / per * owner.nockback * this.revision;
                z = z / per * owner.nockback * this.revision;
                Vector3 force = new Vector3(x, 5000, z);
                //		this.rigidbody.AddForce(force);
            }
        }

        public GameObject numObj = null;//InspectorでPrefab/Numberを追加

        void damageMessage(int dmg)
        {//生成する時
            if (numObj != null && 0 < dmg)
            {
                GameObject num = (GameObject)Instantiate(numObj, this.transform.position, Quaternion.Euler(Vector3.zero));

                num.SendMessage("numEvent", dmg);
            }

        }

        public GameObject effObj = null;
        private IDictionary<string, Vector3> damagePositions = new Dictionary<string, Vector3>();

        void damageEffect(GameObject obj, Vector3 pos)
        {
            if (obj.tag == damageObjTag && effObj != null)
            {
                Vector3 damagePosition = (Vector3)damagePositions[obj.name];
                Vector3 vec = (pos - damagePosition);
                Vector3 nvec = vec.normalized / 10.0f;
                int count = (int)(vec.magnitude * 10.0f);
                //		Debug.Log("mag : "+count);
                for (; 0 < count; count--)
                {
                    damagePosition += nvec;
                    GameObject eff = (GameObject)Instantiate(effObj, damagePosition, Quaternion.identity);
                    eff.transform.parent = transform.parent;
                    Destroy(eff, 0.5f);
                }
                damagePositions[obj.name] = damagePosition;
            }
        }


        public GameObject dethObj = null;

        void dethEffect()
        {
            if (dethObj != null)
            {
                GameObject eff = (GameObject)Instantiate(dethObj, this.transform.position, Quaternion.identity);
                Destroy(eff, 2);
            }
        }
    }
}
using UnityEngine;
using System.Collections;

namespace Unity_VR.Item
{

    public class ItemMaterial : MonoBehaviour, IItem
    {


        public virtual GameObject ParentObj
        {
            get { return this.gameObject; }
        }

        public virtual void hold()
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            collider.isTrigger = true;
        }

        public virtual void release()
        {
            if (this)
            {
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;
                collider.isTrigger = false;
            }
        }

        public virtual void use()
        {
        }

        public virtual GameObject getGameObject()
        {
            return this ? this.gameObject : null;
        }

        protected virtual void Awake()
        {
            if (!rigidbody)
            {
                this.gameObject.AddComponent<Rigidbody>();
            }
        }

        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}
using UnityEngine;
using System.Collections;
using Unity_VR.Global;
using Unity_VR.MainPlayer.Motion.Joint;

namespace Unity_VR.Item
{

    [System.Serializable]
    public class EatAbleItem : DefaultItem, IEatAbleItem, JointInterface
    {

        public GameObject useEffect;

        public virtual void use()
        {
            if (useEffect)
            {
                GameObject rootObj = GlobalController.getInstance().MainCharacter.Data.RootObject;
                GameObject effect = (GameObject)GameObject.Instantiate(useEffect, rootObj.transform.position, rootObj.transform.rotation);
                Destroy(effect, 1f);
            }
            Destroy(this.gameObject, 0.1f);
        }

        public virtual void hold()
        {
            base.hold();

            bool f = GlobalController.getInstance().MainCharacter.MotionController.setMotionHandler((JointInterface)this, this.gameObject);
            Debug.Log("regist iEatableItem : " + f);
        }

        protected virtual void Start()
        {
        }


        public virtual void action(float time, Transform targetTransform)
        {
            Debug.Log("hold(" + time + ") : " + base.holding);
            if (!base.holding)
            {
                return;
            }

            if (time == 3f)
            {
                this.use();
            }
        }

        public virtual void resetAction() { }
    }
}
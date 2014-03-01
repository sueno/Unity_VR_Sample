using UnityEngine;
using System.Collections;

namespace Unity_VR.MainPlayer.Motion.Joint
{

    public class HandJoint : JointInterface
    {

        private GameObject target;

        public GameObject keepEffect { set; get; }

        public HandJoint(GameObject obj)
        {
            target = obj;
            if (!keepEffect)
            {
                keepEffect = (GameObject)Resources.Load("Explosions/Shockwave");
            }
        }

        public void action(float time, Transform targetTransform)
        {
            //		GameObject eff = (GameObject)GameObject.Instantiate(keepEffect,target.transform.position,target.transform.rotation);
            //		GameObject.Destroy(eff, 0.1f);
        }

        public void resetAction()
        {
        }

    }

}
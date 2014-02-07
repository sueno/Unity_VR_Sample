using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Unity_VR.MainPlayer;
using Unity_VR.Mobs;

namespace Unity_VR.UseAnothoerLibraryClasses.mmdForUnity
{

    public class MMDJoint
    {


        private List<string> jointNameList =
        new List<string>(){"頭","首","上半身","下半身","__Empty","左腕","左ひじ","左手首","__Empty","__Empty","__Empty","右腕","右ひじ","右手首","__Empty"
		,"__Empty","左足","左ひざ","__左足首","__Empty","右足","右ひざ","__右足首","__Empty","__Empty","__Empty","両目","右目","左目","Expression"};

        private GameObject rootObj;
        private GameObject[] joints;

        public MMDJoint(GameObject rootObj)
        {
            this.rootObj = rootObj;
            joints = new GameObject[jointNameList.Count];
        }

        public GameObject[] registGameObj(GameObject obj)
        {
            foreach (Transform child in obj.transform)
            {
                //			Debug.Log("child name : "+child.name+"   "+jointNameList.Contains(child.name)+"   ("+jointNameList.IndexOf(child.name));
                if (jointNameList.Contains(child.name))
                {
                    joints[jointNameList.IndexOf(child.name)] = child.gameObject;
                }
                registGameObj(child.gameObject);
            }
            return joints;
        }

        public void registListener(int id, GameObject obj, PlayerMaterial playerStatus)
        {
            foreach (Transform child in obj.transform)
            {
                if (child.rigidbody)
                {
                    //				Debug.Log("child name : "+child.name+"   "+jointNameList.Contains(child.name)+"   ("+jointNameList.IndexOf(child.name));
                    MobParts part = child.gameObject.AddComponent<MobParts>();
                    part.PlayerID = id;
                    playerStatus.DataSource = part;
                }
                if (child.collider)
                {
                    Collider c1 = rootObj.GetComponent<CharacterController>();
                    Collider[] collider2 = child.GetComponents<Collider>();
                    //				Debug.Log("IGNORE  : "+c1+"  ::  "+child.collider);
                    //				foreach (Collider c1 in collider1) {
                    foreach (Collider c2 in collider2)
                    {
                        Physics.IgnoreCollision(c1, c2);
                    }
                    //				}
                }
                registListener(id, child.gameObject, playerStatus);
            }
        }
    }
}
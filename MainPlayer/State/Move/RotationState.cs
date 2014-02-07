using UnityEngine;
using System.Collections;

namespace Unity_VR.MainPlayer.State.Move
{

    [System.Serializable]
    public class RotationState
    {

        private Quaternion rotation = Quaternion.Euler(Vector3.zero);
        public Vector3 euler = Vector3.zero;
        private bool changed = false;
        public Vector3 revisionEuler = Vector3.one;

        public RotationState()
        {
        }

        public RotationState(Vector3 rev)
        {
            revisionEuler = rev;
        }

        public void setEuler(Vector3 rot)
        {
            euler = new Vector3(rot.x * revisionEuler.x, rot.y * revisionEuler.y, rot.z * revisionEuler.z);
            changed = true;
        }

        public Vector3 getEuler()
        {
            changed = false;
            return euler;
        }

        public bool isChange()
        {
            return changed;
        }


        public void setRotation(Quaternion rotate)
        {
            rotation = rotate;
            changed = true;
        }

        public Quaternion getRotation()
        {
            return rotation;
        }
    }
}
using UnityEngine;
using System.Collections;

namespace Unity_VR.MainPlayer.State.Move
{

    [System.Serializable]
    public class RotateStatus
    {

        public float rotateRiv = 0.1f;
        public float rotateLimit = 40.0f;
        private float rotateDirection = 0.0f;

        public RotateStatus()
        {
        }

        public void setRotate(float rootx, float x)
        {
            rotateDirection = (x - rootx) * rotateRiv;
        }

        public float getRotate()
        {
            if (-1 * rotateLimit < rotateDirection && rotateDirection < rotateLimit)
            {
                float r = rotateDirection;
                rotateDirection = 0;
                return r;
            }
            return 0f;
        }
    }
}
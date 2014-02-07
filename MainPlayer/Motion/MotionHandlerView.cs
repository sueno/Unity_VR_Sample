using UnityEngine;
using System.Collections;
using Unity_VR.MainPlayer.Motion.Joint;
using Unity_VR.Util;

namespace Unity_VR.MainPlayer.Motion
{

    public class MotionHandlerView : Observer
    {

        private JointInterface joint;

        public MotionHandlerView(JointInterface joint)
        {
            this.joint = joint;
            AddUpdateAction(typeof(MotionHandler), obj => { if (0 < ((MotionHandler)obj).KeepCount) { joint.action(((MotionHandler)obj).KeepCount); } else { joint.resetAction(); } });
        }

    }
}
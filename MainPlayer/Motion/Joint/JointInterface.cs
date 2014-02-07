using UnityEngine;
using System.Collections;

namespace Unity_VR.MainPlayer.Motion.Joint
{

    public interface JointInterface
    {
        void action(int i);
        void resetAction();
    }
}
using UnityEngine;
using System.Collections;

namespace Unity_VR.UseAnothoerLibraryClasses.mmdForUnity.ExpressionState
{

    public class MorphEvent : MorphBaseStatus
    {

        public MorphEvent(MorphManager morphManager, string morphName)
            : base(morphManager, morphName)
        {
        }
        public MorphEvent(MorphManager morphManager, string morphName, float maxTime)
            : base(morphManager, morphName, maxTime)
        {
        }
        public MorphEvent(MorphManager morphManager, string morphName, float maxTime, float weight)
            : base(morphManager, morphName, maxTime, weight)
        {
        }
        public MorphEvent(MorphManager morphManager, string morphName, float maxTime, float weight, float minWeight)
            : base(morphManager, morphName, maxTime, weight, minWeight)
        {
        }

        public override void actionStart()
        {
            if (actionObj)
            {
                start();
            }
        }
        public override void action(float time)
        {
            actionRun(false, time);
        }

    }
}
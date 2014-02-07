using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unity_VR.UseAnothoerLibraryClasses.mmdForUnity.ExpressionState
{

    public class EyeEvent : ExpressionStatus
    {
        private GameObject eyes;

        protected IList<ExpressionStatus> ignoreList = new List<ExpressionStatus>();

        float maxActionTime = 1f;
        float actionTime = 0f;

        public EyeEvent(GameObject eyes)
        {
            this.eyes = eyes;
        }
        public EyeEvent(GameObject eyes, float maxActionTime)
        {
            this.eyes = eyes;
            this.maxActionTime = maxActionTime;
        }

        public bool isEnable()
        {
            return eyes;
        }
        public void actionStart()
        {
            //		eyes.transform.localRotation = Quaternion.Euler(new Vector3(350,350,0));
            actionTime = maxActionTime;
        }
        public void action(float time)
        {
            if (!isActionAble())
            {
                eyes.transform.localRotation = Quaternion.Euler(Vector3.zero);
                actionTime = 0f;
                return;
            }
            if (isActive())
            {
                actionTime -= time;
                //			float xr = (float)(Random.Range(0,20)-10)/10f;
                float yr = (float)(Random.Range(0, 20) - 10) / 10f;
                //			eyes.transform.localRotation = Quaternion.Euler(new Vector3(350f+xr,350f+yr,0f));
                Vector3 angle = eyes.transform.localRotation.eulerAngles;
                eyes.transform.localRotation = Quaternion.Euler(new Vector3(angle.x, angle.y + yr, angle.z));
            }
            else
            {
            }
        }
        public bool isActive()
        {
            return 0f < actionTime;
        }
        public void addIgnoreAction(ExpressionStatus es)
        {
            ignoreList.Add(es);
        }

        public void addIgnoreActions(params ExpressionStatus[] ess)
        {
            foreach (ExpressionStatus es in ess)
            {
                addIgnoreAction(es);
            }
        }

        protected bool isActionAble()
        {
            foreach (ExpressionStatus es in ignoreList)
            {
                if (es.isActive())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
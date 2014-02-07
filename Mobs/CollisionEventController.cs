using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Unity_VR.Global;

namespace Unity_VR.Mobs
{

    public class CollisionEventController
    {

        private IDictionary<Type, CollisionEventInterface> actionDic = new Dictionary<Type, CollisionEventInterface>();

        public void Add(CollisionEventInterface collisionEvent)
        {
            actionDic.Add(collisionEvent.getActionType(), collisionEvent);
        }

        public void action(ICollisionMaterial thisObj, ICollisionMaterial colObj)
        {
            if (colObj != null && actionDic.ContainsKey(colObj.GetType()))
            {
                (actionDic[colObj.GetType()]).action(thisObj, colObj);
            }
        }
    }
}
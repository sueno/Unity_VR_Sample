using UnityEngine;
using System;
using System.Collections;

using Unity_VR.Global;

namespace Unity_VR.Mobs
{

    public interface CollisionEventInterface
    {

        Type getActionType();
        void action(ICollisionMaterial player, ICollisionMaterial colObj);
    }
}
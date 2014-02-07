using UnityEngine;
using System.Collections;
using Unity_VR.Mobs;

namespace Unity_VR.Global
{

    public interface ICollisionMaterial
    {

        int PlayerID
        {
            get;
        }

        string name
        {
            get;
        }

        ICollisionMaterial ColObj
        {
            get;
        }

        GameObject ParentObj
        {
            get;
        }

        Mob Owner
        {
            set;
            get;
        }
    }
}
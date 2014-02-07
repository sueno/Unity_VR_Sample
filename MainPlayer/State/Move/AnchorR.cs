using UnityEngine;
using System.Collections;

namespace Unity_VR.MainPlayer.State.Move
{

    public class AnchorR : SingleAnchor
    {

        public AnchorR(GameObject player, float dist, Vector3 point)
            : base(player, dist, point)
        {
        }
    }
}
using UnityEngine;
using System.Collections;

namespace Unity_VR.MainPlayer.State.Move
{

    public class AnchorL : SingleAnchor
    {

        public AnchorL(GameObject player, float dist, Vector3 point)
            : base(player, dist, point)
        {
            anchorAction(player.transform.position, point);
        }

        private void anchorAction(Vector3 start, Vector3 end)
        {
            // Resourse の引数がオブ弱との名前
            GameObject game = (GameObject)Resources.Load("Explosions/Burst");
            GameObject eff = (GameObject)GameObject.Instantiate(game, end, new Quaternion(1f, 0f, 0f, 0f));
            GameObject.Destroy(eff, 0.4f);
        }
    }
}
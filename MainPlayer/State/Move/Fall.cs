using UnityEngine;
using System.Collections;

namespace Unity_VR.MainPlayer.State.Move
{

    public class Fall : MoveStatus
    {

        float dTime = 0.0f;

        public Fall(GameObject player, float dist, MoveStatus move)
            : base(player, dist)
        {
            moveDirection = move.getMoveDirection();
        }

        public override void setMove(Vector3 rootPos, Vector3 pos)
        {
        }

        public override Vector3 getMove()
        {
            dTime += Time.deltaTime;
            return new Vector3(moveDirection.x, -9.8f * dTime * dTime, moveDirection.z);
        }

    }
}
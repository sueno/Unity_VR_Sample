using UnityEngine;
using System.Collections;

namespace Unity_VR.MainPlayer.State.Move
{

    public class Normal : MoveStatus
    {

        CharacterController controller;

        float dTime = 0.5f;
        float height = -1.0f;

        public Normal(GameObject player, float dist)
            : base(player, dist)
        {
            //		Debug.Log("State:::::::::::::::::Nomal");
            controller = player.GetComponent<CharacterController>();
        }

        public override void setMove(Vector3 rootPos, Vector3 pos)
        {
            moveDirection = new Vector3((pos.x - rootPos.x) * dist, moveDirection.y, (rootPos.z - pos.z) * dist);
        }

        public override Vector3 getMove()
        {
            if (height == -1.0f)
            {
                height = player.transform.position.y;
            }
            else if (player.transform.position.y < height)
            {
                // touch ground
                if (!controller.isGrounded)
                {
                    Debug.Log("no Ground");
                    dTime += Time.deltaTime;
                }
            }
            else
            {
                dTime = 0.5f;
            }
            height = player.transform.position.y;
            //		Debug.Log(player.transform.position.y+" :::: "+height);
            moveDirection = new Vector3(moveDirection.x, (-9.8f * dTime * dTime) + (0 < moveDirection.y ? moveDirection.y : 0f), moveDirection.z);
            Vector3 move = getVec();
            //		moveDirection = new Vector3(moveDirection.x, 0f ,moveDirection.z);
            return move;
        }

        private Vector3 isGround(Vector3 pos, float range)
        {
            float rev = 0.5f;
            Ray ray = new Ray(pos + new Vector3(0f, rev, 0f), new Vector3(0, -1, 0));
            RaycastHit hit = new RaycastHit();
            // 何かにぶつかった
            if (Physics.Raycast(ray, out hit, range + rev))
            {
                return hit.point;
            }
            else
            {
                return Vector3.down;
            }
        }
    }
}
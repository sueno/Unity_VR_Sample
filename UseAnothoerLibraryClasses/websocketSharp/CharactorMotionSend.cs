using UnityEngine;
using System.Collections;
using WebSocketSharp;
using LitJson;

namespace Unity_VR.UseAnothoerLibraryClasses.websocketSharp
{

    public class CharactorMotionSend : MonoBehaviour
    {

        public string hostName = "127.0.0.1";
        public int port = 3000;

        private Queue messageQueue = new Queue();

        WebSocket ws;

        void Awake()
        {
            Connect();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float x = this.transform.position.x;
            float y = this.transform.position.y;
            float z = this.transform.position.z;
            ws.Send("{\"id\":\"\",\"move\":{\"x\":\"" + x + "\",\"y\":\"" + y + "\",\"z\":\"" + z + "\"}}");
        }

        void Connect()
        {
            ws = new WebSocket("ws://" + hostName + ":" + port);
            ws.Connect();
            Debug.Log("Connect to " + ws.Url);
        }


        /**string sendJoint (GameObject target) {
            Transform tf = target.transform;
            string jointPosition =
                "jointPosition:{"
                +"Head:"+VtoJ(tf.FindChild("Head").position)
                +",Neck:"+VtoJ(tf.FindChild("Neck").position)
                //+",Torso:"+VtoJ(tf.FindChild("Torso").position)
                +",Spline1:"+VtoJ(tf.FindChild("Spline1").position)
                //+",Waist:"+VtoJ(tf.FindChild("Waist").position)
                +",Spline:"+VtoJ(tf.FindChild("Spline").position)
                //+",LeftCollar:"+VtoJ(tf.FindChild("LeftCollar").position)
                //+",LeftShoulder:"+VtoJ(tf.FindChild("LeftShoulder").position)
                +",LeftArm:"+VtoJ(tf.FindChild("LeftArm").position)
                //+",LeftElbow:"+VtoJ(tf.FindChild("LeftElbow").position)
                +",LeftForeArm:"+VtoJ(tf.FindChild("LeftForeArm").position)
                //+",LeftWrist:"+VtoJ(tf.FindChild("LeftWrist").position)
                +",LeftHand:"+VtoJ(tf.FindChild("LeftHand").position)
                //+",LeftHand:"+VtoJ(tf.FindChild("LeftHand").position)
                //+",LeftFingertip:"+VtoJ(tf.FindChild("LeftFingertip").position)
                //+",RightCollar:"+VtoJ(tf.FindChild("RightCollar").position)
                //+",RightShoulder:"+VtoJ(tf.FindChild("RightShoulder").position)
                +",RightArm:"+VtoJ(tf.FindChild("RightArm").position)
                //+",RightElbow:"+VtoJ(tf.FindChild("RightElbow").position)
                +",RightForeArm:"+VtoJ(tf.FindChild("RightForeArm").position)
                //+",RightWrist:"+VtoJ(tf.FindChild("RightWrist").position)
                +",RightHand:"+VtoJ(tf.FindChild("RightHand").position)
                //+",RightHand:"+VtoJ(tf.FindChild("RightHand").position)
                //+",RightFingertip:"+VtoJ(tf.FindChild("RightFingertip").position)
                //+",LeftHip:"+VtoJ(tf.FindChild("LeftHip").position)
                +",LeftUpLeg:"+VtoJ(tf.FindChild("LeftUpLeg").position)
                //+",LeftKnee:"+VtoJ(tf.FindChild("LeftKnee").position)
                +",LeftLeg:"+VtoJ(tf.FindChild("LeftLeg").position)
                //+",LeftAnkle:"+VtoJ(tf.FindChild("LeftAnkle").position)
                +",LeftFoot:"+VtoJ(tf.FindChild("LeftFoot").position)
                //+",LeftFoot:"+VtoJ(tf.FindChild("LeftFoot").position)
                //+",RightHip:"+VtoJ(tf.FindChild("RightHip").position)
                +",RightUpLeg:"+VtoJ(tf.FindChild("RightUpLeg").position)
                //+",RightKnee:"+VtoJ(tf.FindChild("RightKnee").position)
                +",RightLeg:"+VtoJ(tf.FindChild("RightLeg").position)
                //+",RightAnkle:"+VtoJ(tf.FindChild("RightAnkle").position)
                +",RightFoot:"+VtoJ(tf.FindChild("RightFoot").position)
                //+",RightFoot:"+VtoJ(tf.FindChild("RightFoot").position)
                +"}";
            string jointRotation =
                "jointRotation:{"
                +"Head:"+QtoJ(tf.FindChild("Head").rotation)
                +",Neck:"+QtoJ(tf.FindChild("Neck").rotation)
                //+",Torso:"+QtoJ(tf.FindChild("Torso").rotation)
                +",Spline1:"+QtoJ(tf.FindChild("Spline1").rotation)
                //+",Waist:"+QtoJ(tf.FindChild("Waist").rotation)
                +",Spline:"+QtoJ(tf.FindChild("Spline").rotation)
                //+",LeftCollar:"+QtoJ(tf.FindChild("LeftCollar").rotation)
                //+",LeftShoulder:"+QtoJ(tf.FindChild("LeftShoulder").rotation)
                +",LeftArm:"+QtoJ(tf.FindChild("LeftArm").rotation)
                //+",LeftElbow:"+QtoJ(tf.FindChild("LeftElbow").rotation)
                +",LeftForeArm:"+QtoJ(tf.FindChild("LeftForeArm").rotation)
                //+",LeftWrist:"+QtoJ(tf.FindChild("LeftWrist").rotation)
                +",LeftHand:"+QtoJ(tf.FindChild("LeftHand").rotation)
                //+",LeftHand:"+QtoJ(tf.FindChild("LeftHand").rotation)
                //+",LeftFingertip:"+QtoJ(tf.FindChild("LeftFingertip").rotation)
                //+",RightCollar:"+QtoJ(tf.FindChild("RightCollar").rotation)
                //+",RightShoulder:"+QtoJ(tf.FindChild("RightShoulder").rotation)
                +",RightArm:"+QtoJ(tf.FindChild("RightArm").rotation)
                //+",RightElbow:"+QtoJ(tf.FindChild("RightElbow").rotation)
                +",RightForeArm:"+QtoJ(tf.FindChild("RightForeArm").rotation)
                //+",RightWrist:"+QtoJ(tf.FindChild("RightWrist").rotation)
                +",RightHand:"+QtoJ(tf.FindChild("RightHand").rotation)
                //+",RightHand:"+QtoJ(tf.FindChild("RightHand").rotation)
                //+",RightFingertip:"+QtoJ(tf.FindChild("RightFingertip").rotation)
                //+",LeftHip:"+QtoJ(tf.FindChild("LeftHip").rotation)
                +",LeftUpLeg:"+QtoJ(tf.FindChild("LeftUpLeg").rotation)
                //+",LeftKnee:"+QtoJ(tf.FindChild("LeftKnee").rotation)
                +",LeftLeg:"+QtoJ(tf.FindChild("LeftLeg").rotation)
                //+",LeftAnkle:"+QtoJ(tf.FindChild("LeftAnkle").rotation)
                +",LeftFoot:"+QtoJ(tf.FindChild("LeftFoot").rotation)
                //+",LeftFoot:"+QtoJ(tf.FindChild("LeftFoot").rotation)
                //+",RightHip:"+QtoJ(tf.FindChild("RightHip").rotation)
                +",RightUpLeg:"+QtoJ(tf.FindChild("RightUpLeg").rotation)
                //+",RightKnee:"+QtoJ(tf.FindChild("RightKnee").rotation)
                +",RightLeg:"+QtoJ(tf.FindChild("RightLeg").rotation)
                //+",RightAnkle:"+QtoJ(tf.FindChild("RightAnkle").rotation)
                +",RightFoot:"+QtoJ(tf.FindChild("RightFoot").rotation)
                //+",RightFoot:"+QtoJ(tf.FindChild("RightFoot").rotation)
                +"}";
            return jointPosition+","+jointRotation;
        }/**/
    }
}
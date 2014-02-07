
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using LitJson;

namespace Unity_VR.UseAnothoerLibraryClasses.websocketSharp
{

    public class SocketClient : MonoBehaviour
    {

        public GameObject player = null;
        private Transform plt;
        public GameObject player_net = null;
        //public GameObject rootPos = null;

        public string hostName = "127.0.0.1";
        public int port = 3000;
        public float sendTime = 0.03f;

        public Vector3 spownPoint = new Vector3(0, 0, 0);
        public Quaternion spownRotation = new Quaternion(0, 0, 0, 0);

        private float dTime = 0.0f;
        private string myID = "";
        private IDictionary<string, GameObject> gameObjs = new Dictionary<string, GameObject>();
        private IDictionary<string, IDictionary<string, Transform>> gameObjTrans = new Dictionary<string, IDictionary<string, Transform>>();
        private IDictionary<string, IDictionary<string, Vector3>> gameObjDefault = new Dictionary<string, IDictionary<string, Vector3>>();

        private Queue messageQueue = new Queue();
        private Queue jsonData = new Queue();

        private WebSocket ws;

        private List<string> sendList =
            new List<string>(){"Head","Neck","Spline","LeftArm","LeftForeArm","LeftHand","RightArm","RightForeArm",
        	"RightHand","LeftUpLeg","LeftLeg","LeftFoot","RightUpLeg","RightLeg","RightFoot","Hips",
			"neck","RS","RUA","RA","RH","LS","LUA","LA","LH","LUL","LL","LF","RUL","RL","RF"};
        private IDictionary<string, Transform> joints = new Dictionary<string, Transform>();
        private IDictionary<string, Vector3> defaultVector3 = new Dictionary<string, Vector3>();


        void Awake()
        {
            Connect();
        }

        // Use this for initialization
        void Start()
        {
            if (player == null)
            {
                player = this.gameObject;
            }
            if (player_net == null)
            {
                player_net = GameObject.FindWithTag("Player_net");
            }
            plt = player.transform;


            joints = registObjTrans(player);
        }

        // Update is called once per frame
        void Update()
        {
            if (dTime > sendTime)
            {
                this.sendPlayerMotion();
                dTime = 0.0f;

                lock (messageQueue.SyncRoot)
                {
                    while (messageQueue.Count > 0)
                    {
                        jsonData.Enqueue(LitJson.JsonMapper.ToObject((string)messageQueue.Dequeue()));
                    }
                }
            }
            dTime += Time.deltaTime;

            lock (jsonData.SyncRoot)
            {
                while (jsonData.Count > 0)
                {
                    LitJson.JsonData d = (LitJson.JsonData)jsonData.Dequeue();
                    //Debug.Log("dataCount : "+jsonData.Count);
                    string type = (string)d["type"];
                    if (type.Equals("move"))
                    {
                        this.move(d);
                    }
                    else if (type.Equals("id"))
                    {
                        myID = (string)d["id"];
                        Debug.Log("your ID : " + myID);
                    }
                    /**else if (type.Equals("regist")) {
                    string id = (string)d["id"];
                    if (!id.Equals(myID)||!id.Equals("")) {
                        this.loginPlayer(id);
                    }
                }*/
                }
            }
        }

        void Connect()
        {
            ws = new WebSocket("ws://" + hostName + ":" + port);


            // called when websocket messages come.
            ws.OnMessage += (sender, e) =>
            {
                messageQueue.Enqueue(e.Data);
                //Debug.Log(e.Data);
            };

            ws.Connect();
            Debug.Log("Connect to " + ws.Url);
        }

        void sendPlayerMotion()
        {
            string a = ("\"type\":\"move\",\"id\":\"" + myID + "\"");
            string position = ("\"position\":{\"x\":\"" + plt.position.x + "\",\"y\":\"" + plt.position.y + "\",\"z\":\"" + plt.position.z + "\"}");
            string rotation = ("\"rotation\":{\"x\":\"" + plt.rotation.x + "\",\"y\":\"" + plt.rotation.y + "\",\"z\":\"" + plt.rotation.z + "\",\"w\":\"" + plt.rotation.w + "\"}");
            ws.Send("{" + a + "," + position + "," + rotation + "," + sendJoint(player) + "" +
                "}");
        }

        void loginPlayer(string id)
        {
            GameObject target = (GameObject)Instantiate(player_net, spownPoint, spownRotation);
            gameObjs.Add(id, target);
            gameObjTrans.Add(id, registObjTrans(gameObjs[id]));
            gameObjDefault.Add(id, registObjVector3(gameObjs[id]));
        }

        void move(LitJson.JsonData move)
        {
            string id = (string)move["id"];
            if (id.Equals(myID) || id.Equals(""))
            {
                return;
            }
            if (!gameObjs.ContainsKey(id))
            {
                loginPlayer(id);
            }
            LitJson.JsonData p = move["position"];
            gameObjs[id].transform.position = new Vector3(float.Parse((string)p["x"]), float.Parse((string)p["y"]), float.Parse((string)p["z"]));
            LitJson.JsonData r = move["rotation"];
            gameObjs[id].transform.rotation = new Quaternion(float.Parse((string)r["x"]), float.Parse((string)r["y"]), float.Parse((string)r["z"]), float.Parse((string)r["w"]));
            setMotion(id, move["joint"]);
        }

        private string VtoJ(Vector3 vec)
        {
            return "{\"x\":\"" + vec.x + "\",\"y\":\"" + vec.y + "\",\"z\":\"" + vec.z + "\"}";
        }
        private Vector3 JtoV(LitJson.JsonData p)
        {
            return new Vector3(float.Parse((string)p["x"]), float.Parse((string)p["y"]), float.Parse((string)p["z"]));
        }
        private string QtoJ(Quaternion qua)
        {
            return "{\"x\":\"" + qua.x + "\",\"y\":\"" + qua.y + "\",\"z\":\"" + qua.z + "\",\"w\":\"" + qua.w + "\"}";
        }
        private Quaternion JtoQ(LitJson.JsonData r)
        {
            return new Quaternion(float.Parse((string)r["x"]), float.Parse((string)r["y"]), float.Parse((string)r["z"]), float.Parse((string)r["w"]));
        }

        void setJoint(LitJson.JsonData position)
        {

        }

        private string sendJoint(GameObject player)
        {
            System.Text.StringBuilder jointData = new System.Text.StringBuilder();
            jointData.Append("\"joint\":{");
            foreach (string key in joints.Keys)
            {
                jointData.Append("\"" + key + "\":" + VtoJ(joints[key].eulerAngles) + ",");
                //joints[key].rotation = JtoQ(rot[key]);
            }
            jointData.Append("\"__RootPosition\":" + VtoJ(player.transform.position) + "}");
            //		Debug.Log(jointData.ToString());
            return jointData.ToString();

        }


        private void childData(GameObject obj, System.Text.StringBuilder jointPosition, System.Text.StringBuilder jointRotation)
        {
            foreach (Transform child in obj.transform)
            {
                if (sendList.Contains(child.name))
                {
                    jointPosition.Append("\"" + child.name + "\":" + VtoJ(child.position) + ",");
                    jointRotation.Append("\"" + child.name + "\":" + QtoJ(child.rotation) + ",");
                }
                childData(child.gameObject, jointPosition, jointRotation);
            }
        }

        private void setMotion(string id, LitJson.JsonData joint)
        {
            IDictionary<string, Transform> trans = gameObjTrans[id];
            IDictionary<string, Vector3> angle = gameObjDefault[id];
            foreach (string key in trans.Keys)
            {
                try
                {
                    trans[key].eulerAngles = (JtoV(joint[key]));
                }
                catch (KeyNotFoundException)
                {
                    Debug.Log("key not found : " + key + "  cased : ");
                }
            }

        }

        private IDictionary<string, Transform> registObjTrans(GameObject obj)
        {
            IDictionary<string, Transform> dic = new Dictionary<string, Transform>();
            foreach (Transform child in obj.transform)
            {
                if (sendList.Contains(child.name))
                {
                    dic.Add(child.name, child.transform);
                }
                IDictionary<string, Transform> d = registObjTrans(child.gameObject);
                foreach (string key in d.Keys)
                {
                    dic.Add(key, d[key]);
                }
            }
            return dic;
        }
        private IDictionary<string, Vector3> registObjVector3(GameObject obj)
        {
            IDictionary<string, Vector3> dic = new Dictionary<string, Vector3>();
            foreach (Transform child in obj.transform)
            {
                if (sendList.Contains(child.name))
                {
                    dic.Add(child.name, child.transform.eulerAngles);
                }
                IDictionary<string, Vector3> d = registObjVector3(child.gameObject);
                foreach (string key in d.Keys)
                {
                    dic.Add(key, d[key]);
                }
            }
            return dic;
        }
    }
}
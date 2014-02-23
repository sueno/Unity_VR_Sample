using UnityEngine;
using System;
using System.Collections;
using Unity_VR.Global;
using Unity_VR.MainPlayer.Motion;
using Unity_VR.MainPlayer.State.Move;
using Unity_VR.UseAnothoerLibraryClasses.LitJSON;
using Unity_VR.UseAnothoerLibraryClasses.websocketSharp;
//using Unity_VR.UseAnothoerLibraryClasses.PropsAnimation;

namespace Unity_VR.MainPlayer
{

    [System.Serializable]
    public class MainCharacterController : MonoBehaviour
    {

        public GameObject mainCamera;

        public bool mainPlayer = false;
        public float dist = 0.02f;
        public bool useRemoteController = false;

        public MainCharacterData data = new MainCharacterData();
        public MainCharacterData Data
        {
            get { return this.data; }
        }

        private MainMotionController motionController;
        public MainMotionController MotionController
        {
            get { return this.motionController; }
        }

        private CharacterController controller;
        public CharacterController Controller
        {
            get { return controller; }
        }

        private MoveStatus moveStatus = null;
        public MoveStatus MoveStatus
        {
            set { this.moveStatus = value; }
            get { return this.moveStatus; }
        }

        private RotateStatus rotateStatus = new RotateStatus();
        public RotateStatus RotateStatus
        {
            get { return rotateStatus; }
        }

        private RotationState[] jointRotations;

//        private MoveAnimatorController moveAnimator;

        public void Awake()
        {
            if (!data.MainPlayer)
            {
                data.MainPlayer = this.gameObject;
            }
            if (!data.RootObject)
            {
                data.RootObject = this.gameObject;
            }

            controller = (CharacterController)data.RootObject.GetComponent<CharacterController>();

            moveStatus = new Normal(data.RootObject, dist);

            int playerID = -1;
            if (mainPlayer)
            {
                GlobalController.getInstance().MainCharacter = this;
                playerID = 0;
                motionController = MainMotionController.AddComponent(data.MainPlayer, this);
            }
            else
            {
                playerID = GlobalController.getInstance().addPlayer(this);
            }
            data.init(playerID);

            if (useRemoteController)
            {
                data.__setJointObject((int)PlayerJoint.RightWrist, null);
                data.__setJointObject((int)PlayerJoint.LeftWrist, null);
                data.MainPlayer.AddComponent<WSHandMotion>();
                data.RightHandController.registGameObj(data.RootObject);
                data.LeftHandController.registGameObj(data.RootObject);
            }

            jointRotations = new RotationState[(int)(Enum.GetNames(typeof(PlayerJoint)).Length)];
            for (int i = 0; i < jointRotations.Length; i++)
            {
                jointRotations[i] = new RotationState();
            }
        }

        void Start()
        {
            if (mainCamera)
            {
                CameraPosition.AddComponent(mainCamera, data.RootObject, data.getJoint((int)PlayerJoint.Head), new Vector3(0f, 0.11f, -0.037f));
            }
            //		Animator ani = GetComponent<Animator>();
            //		moveAnimator = new MoveAnimatorController(ani);

        }

        void LateUpdate()
        {
//            Debug.Log("hogeeeee");

            // move
            Vector3 moveDirection = moveStatus.getMove();
            controller.Move(moveDirection);

            // rotate
            data.rootObject.transform.Rotate(0f, rotateStatus.getRotate() * 0.5f, 0f);

            // landing
            if (moveStatus is Fall && controller.isGrounded)
            {
                moveStatus = new Normal(data.rootObject, dist);
            }

            // Animation
            long rotateFilter = -1;
            //		rotateFilter &= moveAnimator.animation(moveDirection);

            // hand motion (iPhone, Android)
            if (useRemoteController)
            {
                data.RightHandController.rotateJoints();
                data.LeftHandController.rotateJoints();
            }

            string a = "";
            for (int ii = 0; ii < 64; ii++)
            {
                a += rotateFilter >> ii & 1;
            }
//            Debug.Log(a);

            // skeleton motion (kinect)
            for (int i = 0; i < jointRotations.Length; i++)
            {
                long filter = (rotateFilter >> i) & 1;
                if (filter == 1 && jointRotations[i].isChange())
                {
                    //				Debug.Log(i+"   "+jointRotations[i].getRotation());
                    data.setRotation(i, jointRotations[i].getRotation());
                }
            }
        }

        public void move(Vector3 vec)
        {
            controller.Move(vec);
        }

        public void rotate(Vector3 eulerAngle)
        {
            data.RootObject.transform.Rotate(eulerAngle);
        }


        public RotationState[] getJointRotationManager()
        {
            return jointRotations;
        }
        //	public GameObject getJoint(int i) {
        //		return data.getJoint(i);
        //	}
        //	public void setPosition(int i, Vector3 position) {
        //		data.setPosition(i,position);
        //	}
        //	
        //	public void setAngle(int i, Vector3 eulerAngle) {
        //		data.setAngle(i,eulerAngle);
        //	}
        //	
        //	public void setRotation(int i, Quaternion rotation) {
        //		data.setRotation(i,rotation);
        //	}


    }
}

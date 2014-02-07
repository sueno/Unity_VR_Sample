//using UnityEditor;
using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
//[RequireComponent( typeof( MeshRenderer ) )]
//[RequireComponent( typeof( MeshFilter ) )]

namespace Unity_VR.Equip
{

    [System.Serializable]
    public class SimpleMeshRenderer : MonoBehaviour
    {

        public Transform topTransform = null;
        public Transform bottomTransform = null;
        public Material material = null;

        private Vector3 _top = Vector3.zero;
        private Vector3 _bottom = Vector3.zero;

        Mesh mesh;
        MeshFilter meshFilter;

        Vector3[] vertices;
        int[] triangles;
        Vector2[] uvs;


        private GameObject parentObj;
        private bool _active = false;
        public int length = 40;
        private int count = 0;
        private GameObject[] panels;


        void Awake()
        {
            panels = new GameObject[length];
            parentObj = new GameObject("CustomPanel");
            parentObj.transform.position = Vector3.zero;
            parentObj.transform.eulerAngles = Vector3.zero;
        }
        // Use this for initialization
        void Start()
        {
            setPanels();
            renderInvisible();

            _top = topTransform.position;
            _bottom = bottomTransform.position;
        }

        private void setPanels()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                GameObject newGameobject = new GameObject("CustomPanel");
                newGameobject.transform.parent = parentObj.transform;
                newGameobject.transform.position = Vector3.zero;
                newGameobject.transform.eulerAngles = Vector3.zero;

                MeshRenderer meshRenderer = newGameobject.AddComponent<MeshRenderer>();
                meshRenderer.material = material != null ? material : (new Material(Shader.Find("Diffuse")));

                newGameobject.AddComponent<MeshFilter>();

                panels[i] = newGameobject;

            }
        }

        void Update()
        {
            if (_active)
            {
                setMesh(topTransform.position, bottomTransform.position, _top);
                setMesh(_top, bottomTransform.position, _bottom);
            }

            _top = topTransform.position;
            _bottom = bottomTransform.position;
        }


        private Vector3 initVec = Vector3.zero;
        private Quaternion initQtn = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        // Update is called once per frame
        public void setMesh(Vector3 x, Vector3 y, Vector3 z)
        {
            MeshFilter meshFilter = panels[count].GetComponent<MeshFilter>();
            meshFilter.mesh = new Mesh();
            Mesh mesh = meshFilter.sharedMesh;

            mesh.Clear();

            mesh.vertices = new Vector3[3] { x, y, z };
            mesh.triangles = new int[3] { 0, 1, 2 };
            mesh.uv = new Vector2[3] { Vector2.zero, Vector2.right, Vector2.up };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            //		Debug.Log(this.name+" ;; "+count+" ::: "+mesh.vertices[0]+" , "+mesh.vertices[1]+" , "+mesh.vertices[2]);

            meshFilter.sharedMesh = mesh;
            meshFilter.sharedMesh.name = "SimpleMesh";

            count = count + 1 < panels.Length ? count + 1 : 0;
        }

        public void rendererActive(bool flag)
        {
            //		Debug.Log("hogehoge "+flag);
            if (flag)
            {
                renderVisible();
            }
            else
            {
                renderInvisible();
            }
        }
        public void renderVisible()
        {
            parentObj.SetActiveRecursively(true);
            _active = true;
        }
        public void renderInvisible()
        {
            parentObj.SetActiveRecursively(false);
            _active = false;
            if (panels[0] == null)
            {
                setPanels();
            }
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].GetComponent<MeshFilter>().mesh.Clear();
            }
        }

    }

}
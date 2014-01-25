using UnityEngine;
using System.Collections;

public class Wing : MonoBehaviour {

	private GameObject wing = null;
	private bool isActive = false;
	
	
	void Start () {
		wing = new GameObject("CustomPanel");
		wing.name = "Wing";
		wing.transform.parent = this.transform;
		wing.transform.localPosition = new Vector3(0f,0f,-2.0f);
		wing.transform.rotation = this.transform.rotation;
		GameObject[] wings = new GameObject[]{	
			getMeshObject(new Vector3(-0.18f,0.05f,0.2f),new Vector3(-0.2f,0.5f,0f),new Vector3(-0.4f,0.45f,0f),new Vector3(-0.6f,1f,-0.3f)),
			getMeshObject(new Vector3(-0.18f,-0.05f,0.2f),new Vector3(-0.2f,-0.5f,0f),new Vector3(-0.4f,-0.45f,0f),new Vector3(-0.6f,-1f,-0.3f)),
			getMeshObject(new Vector3(-0.1f,0.05f,0.2f),new Vector3(0f,0.3f,0f),new Vector3(0.15f,0.3f,0f),new Vector3(0.3f,0.6f,-0.2f)),
			getMeshObject(new Vector3(-0.1f,-0.05f,0.2f),new Vector3(0f,-0.3f,0f),new Vector3(0.15f,-0.3f,0f),new Vector3(0.3f,-0.6f,-0.2f))
							};
		foreach (GameObject obj in wings) {
			obj.transform.parent = wing.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.rotation = wing.transform.rotation;
		}
	}
	
	void Update () {
		if (isActive) {
			wing.SetActiveRecursively (true);
			isActive = false;
		} else {
			wing.SetActiveRecursively (false);
		}
	}
	
	public void active() {
		isActive = true;
	}
	
	private GameObject getMeshObject (Vector3 x1, Vector3 y1, Vector3 x2, Vector3 y2) {
		GameObject panel = new GameObject("CustomPanel");
			
		MeshRenderer meshRenderer = panel.AddComponent<MeshRenderer>();
		meshRenderer.material = new Material(Shader.Find ("Custom/hoge Vertex Lit"));
		meshRenderer.material.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
		
		MeshFilter meshFilter = panel.AddComponent<MeshFilter>();
		meshFilter.mesh = new Mesh ();
		Mesh mesh = meshFilter.sharedMesh;

		mesh.Clear();
		
		mesh.vertices = new Vector3[]{x1,y1,x2,y2};
		mesh.triangles = new int[]{0,1,2,1,3,2};
		mesh.uv = new Vector2[]{Vector2.zero,Vector2.zero,Vector2.zero,Vector2.zero};
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		
		meshFilter.sharedMesh = mesh;
		meshFilter.sharedMesh.name = "WingMesh";
		
		return panel;
	}
}

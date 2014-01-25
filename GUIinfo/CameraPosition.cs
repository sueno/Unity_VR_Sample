using UnityEngine;
using System.Collections;

//@TODO set automaticary
public class CameraPosition : MonoBehaviour {

	public GameObject rootObject;
	private Vector3 localPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
		this.transform.localPosition = localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(new Vector3(0,rootObject.transform.eulerAngles.y,0));
	}
	void LateUpdate () {
		transform.rotation = Quaternion.Euler(new Vector3(0,rootObject.transform.eulerAngles.y,0));
	}

	public static CameraPosition AddComponent(GameObject obj, GameObject rootObj, GameObject parentObj) {
		CameraPosition cp = obj.AddComponent<CameraPosition>();
		cp.rootObject = rootObj;
		obj.transform.parent = parentObj.transform;
		return cp;
	}
	
	public static CameraPosition AddComponent(GameObject obj, GameObject rootObj, GameObject parentObj, Vector3 local) {
		CameraPosition cp = AddComponent(obj, rootObj, parentObj);
		cp.localPosition = local;
		return cp;
	}
}

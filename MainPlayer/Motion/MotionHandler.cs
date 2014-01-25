using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionHandler : Observable {
	
	private GameObject target;
	
	private Queue<float> motionLog;
	private Vector3 beforePosition;
	private float sum = 0.0f;
	
	private float motionDump = 0.2f;
	private float motionKeepTime = 0.0f;
	private float motionKeepBorder = 0.2f;
	
	private int keepCount = 0;
	
	public MotionHandler(GameObject obj) {
	}
	
	/**
	 * prototype
	 **/
	public float MotionDump {
		set {this.motionDump = value;}
		get {return motionDump;}
	}
	public float MotionKeepTime {
		get {return motionKeepTime;}
	}
	public float MotionKeepBorder {
		get {return motionKeepBorder;}
		set {this.motionKeepBorder = value;}
	}
	public int KeepCount {
		get {return keepCount;}
	}
	
	
	// Use this for initialization
	void Start () {
		motionLog = new Queue<float>();
		for (int i=0;i<10;i++) {
			motionLog.Enqueue(100.0f);
			sum+=100.0f;
		}
		motionDump = (motionDump<=0.0f) ? 0.3f:motionDump;
		motionKeepBorder = (motionKeepBorder<=0.0f) ? 1.0f:motionKeepBorder;
	}
	
	// Update is called once per frame
	void Update () {
		if (!target) {
			this.enabled = false;
			return;
		}
		float mag = (target.transform.position-beforePosition).magnitude;
		motionLog.Enqueue(mag);
		beforePosition = target.transform.position;
		float elem = motionLog.Dequeue();
		sum += (float)(mag-elem);
//		
//		Debug.Log ("hoge "+sum+"     Time "+motionKeepTime +"   Cump "+motionDump+"   IF "+(sum<motionDump));
		
		if (sum<motionDump) {
			motionKeepTime += Time.deltaTime;
		} else {
			motionKeepTime = 0.0f;
			if (0<keepCount) {
				keepCount = 0;
				RaiseUpdate(this);
			}
		}
		if (motionKeepBorder<motionKeepTime) {
			keepCount ++;
			RaiseUpdate(this);
			motionKeepTime = 0.0f;
		}
	}
	
	
	public static MotionHandler AddComponent (GameObject target, GameObject obj) {
		MotionHandler gameObject = target.AddComponent<MotionHandler>();
		gameObject.target = obj;
		gameObject.beforePosition = obj.transform.position;
		return gameObject;
	}
	public static MotionHandler AddComponent (GameObject target, GameObject obj, float keepTime) {
		MotionHandler gameObject = AddComponent(target,obj);
		gameObject.motionKeepBorder = keepTime;
		return gameObject;
	}
}

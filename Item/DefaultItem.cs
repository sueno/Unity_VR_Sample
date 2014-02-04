using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultItem : ItemMaterial, IUseAbleItem {

	private Queue<Vector3> positionQueue = new Queue<Vector3>();

	public virtual void hold() {
		base.hold();
	}

	public virtual void release() {
		base.release();
		Vector3 prevPosition = positionQueue.Dequeue();
		Debug.Log("release : "+(transform.position-prevPosition)*40f/Time.deltaTime);
		rigidbody.AddForce((transform.position-prevPosition)*40f/Time.deltaTime);
	}
	
	public virtual void use() {
	}


	protected virtual void Update() {
		if (10<positionQueue.Count) {
			positionQueue.Dequeue();
		}
		positionQueue.Enqueue(transform.position);
	}
}

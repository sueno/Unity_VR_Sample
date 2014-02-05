using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultItem : ItemMaterial, IUseAbleItem {

	protected bool holding = false;
	private Queue<Vector3> positionQueue = new Queue<Vector3>();

	public virtual void hold() {
		base.hold();
		holding = true;
	}

	public virtual void release() {
		if (!this) {
			return;
		}

		base.release();
		holding = false;
		Vector3 force = Vector3.zero;
		Vector3 prevPosition = transform.position;
		int maxCount = positionQueue.Count;
		while (0<positionQueue.Count) {
			Vector3 dequeuePos =  positionQueue.Dequeue();
			force += (dequeuePos - prevPosition) * (maxCount*0.5f - positionQueue.Count*0.5f);
			prevPosition = dequeuePos;
		}

		Debug.Log("release : "+(force)*500f);
		rigidbody.AddForce((force)*500f);
	}
	
	public virtual void use() {
	}


	protected virtual void Update() {
		if (!holding) {
			return;
		}

		if (10<positionQueue.Count) {
			positionQueue.Dequeue();
		}
		positionQueue.Enqueue(transform.position);
	}
}

﻿using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HandCollision : MonoBehaviour {

	public GameObject holdObj;
	private IList<Type> holdTypeList = new List<Type>(){
		typeof(WeaponMaterial),
		typeof(Item)
	};

	private IList<GameObject> colList = new List<GameObject>();

	void OnCollisionEnter (Collision col) {
		colList.Add(col.gameObject);
	}
	void OnCollisionExit (Collision col) {
		colList.Remove(col.gameObject);
	}
	
	void OnTriggerEnter (Collider col) {
		colList.Add(col.gameObject);
	}	
	void OnTriggerExit (Collider col) {
		colList.Remove(col.gameObject);
	}

	public GameObject getCollisionObj() {
		return getComponent(colList);
	}

	private GameObject getComponent(IList<GameObject> objs) {
//		Debug.Log("Component : "+obj);
		foreach (GameObject obj in objs) {
		//@TODO キャストなどできなくて条件分岐で妥協
//		foreach (Type type in holdTypeList) {
//			Component c = obj.GetComponent(type.Name);
//			Debug.Log("Component1 : "+c);
//			if (c) {
//				PropertyInfo p = type.GetProperty("ParentObj");
//				object e = c as object;
//				Debug.Log("Component2 : "+e);
//				GameObject o = null;
//				p.GetValue(e,null);
//				return (GameObject)o;
//			}
//		}
		WeaponMaterial w = obj.GetComponent<WeaponMaterial>();
		if (w) return w.ParentObj;

		Item i = obj.GetComponent<Item>();
		if (i) return i.ParentObj;

		}
		return null;
	}
}
using UnityEngine;
using System.Collections;

[System.Serializable]
public class EatAbleItem : DefaultItem, IEatAbleItem {

	public GameObject useEffect;

	public virtual void use() {
		if (useEffect) {
			GameObject rootObj = GlobalController.getInstance().MainCharacter.Data.RootObject;
			GameObject effect = (GameObject)GameObject.Instantiate(useEffect, rootObj.transform.position, rootObj.transform.rotation);
			Destroy(effect, 1f);
		}
	}
}

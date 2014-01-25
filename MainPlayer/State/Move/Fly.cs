using UnityEngine;
using System.Collections;

public class Fly : MoveStatus {
	
	Wing wing = null;
	
	public Fly (GameObject player, float dist) : base (player, dist*6.0f){
	}

	public override void setMove (Vector3 rootPos, Vector3 pos) {
		moveDirection = new Vector3((pos.x-rootPos.x)*dist,moveDirection.y,(rootPos.z-pos.z)*dist);
	}
		
	public override Vector3 getMove() {
		if (wing==null) {
			GameObject torso = GlobalController.getInstance().MainCharacter.Data.getJoint((int)PlayerJoint.Torso);
			wing = (Wing)torso.GetComponent<Wing>();
			if (wing==null) {
				torso.AddComponent<Wing>();
				wing = (Wing)torso.GetComponent<Wing>();
			}
		}
		wing.active();
		return getVec();
	}
	
	
	
}

using UnityEngine;
using System.Collections;

public class AnchorLR : MoveStatus {
	
	SingleAnchor right;
	SingleAnchor left;
	
	public AnchorLR (SingleAnchor right, SingleAnchor left) : base ((GameObject)null,0.0f){
		this.right = right;
		this.left = left;
	}
	
	public override void setMove (Vector3 rootPos, Vector3 pos) {
		
	}
		
	public override Vector3 getMove() {
		Vector3 rVec = right.getMove();
		Vector3 lVec = left.getMove();
		
		return (rVec+lVec)*0.5f;
	}
	
	public SingleAnchor getRight () {
		return right;
	}
	
	public SingleAnchor getLeft () {
		return left;
	}
}

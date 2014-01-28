using UnityEngine;
using System.Collections;

public abstract class MoveStatus {
	
	protected GameObject player;
	protected float dist;
	
	protected Vector3 moveDirection = Vector3.zero;
	
	public MoveStatus (GameObject player, float dist) {
		this.player = player;
		this.dist = dist;
	}
	
	public abstract void setMove(Vector3 rootPos, Vector3 pos);
	public abstract Vector3 getMove();
	
	public Vector3 getVec() {
//		Debug.Log(player.transform.eulerAngles.y);
		return  Quaternion.Euler(0,player.transform.eulerAngles.y,0) * (Vector3)(getMoveDirection() * Time.deltaTime);
	}
	public void setMoveDirection (Vector3 pos) {
		moveDirection = pos;
	}

	public void addMoveDirection (Vector3 pos) {
		moveDirection += pos;
	}

	public Vector3 getMoveDirection () {
		Vector3 move = moveDirection;
//		moveDirection = new Vector3(moveDirection.x,0,moveDirection.z);
		return move;
	}

	//@TODO remove method
	public void setHeight(float height) {
		if (moveDirection.y<=0) {
			moveDirection = new Vector3(moveDirection.x,height-moveDirection.y,moveDirection.z);
		}
	}
}

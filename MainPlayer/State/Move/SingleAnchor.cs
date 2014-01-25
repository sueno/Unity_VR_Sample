using System;
using UnityEngine;
using System.Collections;

public class SingleAnchor : MoveStatus
{
		
	private Vector3 point;
		
	public SingleAnchor (GameObject player, float dist, Vector3 point) : base (player, dist)
	{
		this.point = point;
	}
		
	public override void setMove (Vector3 rootPos, Vector3 pos)
	{
	}
	
	public override Vector3 getMove () {
		Vector3 vec = (point - player.transform.position);
		float distance = 7.0f<(Vector3.Magnitude(vec)) ? 7.0f:Vector3.Magnitude(vec);
		moveDirection = vec.normalized * distance;
		Debug.Log("anchor move :"+moveDirection);
		return moveDirection;
	}
}


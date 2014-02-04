using UnityEngine;
using System;
using System.Collections;

public interface CollisionEventInterface {
	
	Type getActionType();
	void action(ICollisionMaterial player, ICollisionMaterial colObj);
}

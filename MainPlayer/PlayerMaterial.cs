using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerMaterial: Observer {
	
	public string mob_name = "Player";
	public Mob mob = new Mob();
	private GameObject mainPlayer;


	/**
	 * Observer method
	 * 
	 **/
	public PlayerMaterial () {
	}

	public void init(GameObject player) {
		mainPlayer = player;
		
		CollisionEventController controller = new CollisionEventController();
		CollisionEventInterface ai = mainPlayer.AddComponent<NPC_AI_01>();
		controller.Add(ai);
		CollisionEventInterface de = new DamageEvent(mob);
		controller.Add(de);
		AddUpdateAction(typeof(MobParts), obj => {controller.action((obj as ICollisionMaterial),(obj as ICollisionMaterial).ColObj);});
		//		CollisionEventInterface ai2 = 
	}

	public void addUpdateAction(Type type, Action<object> updateAction) {
		AddUpdateAction(type,updateAction);
	}
}

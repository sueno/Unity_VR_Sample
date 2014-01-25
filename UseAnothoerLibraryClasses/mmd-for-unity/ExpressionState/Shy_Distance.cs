using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shy_Distance : MorphBaseStatus {
	
	private GameObject player;
	private IList<GameObject> playerList = new List<GameObject>();

	public Shy_Distance(MorphManager morphManager, GameObject player) : base (morphManager,"照れ"){
		this.player = player;
		IList<MainCharacterController> players = GlobalController.getInstance().getPlayers();
		foreach (MainCharacterController p in players) {
			if (p) {
				//				Debug.Log(player.Data.mainPlayer);
				playerList.Add(p.Data.getJoint((int)PlayerJoint.Head));
			}
		}
	}
	
	public override void actionStart(){
	}
	public override void action(float time) {
		actionRun((2f>Vector3.Distance(player.transform.position,playerList[0].transform.position)),time);
	}
}

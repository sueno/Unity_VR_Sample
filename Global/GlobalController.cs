using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalController : MonoBehaviour {
	
	private static GlobalController globalController = new GlobalController();

	private MainCharacterController mainCharacter = null;
	private IList<MainCharacterController> playerList = new List<MainCharacterController>(){null};
	
	private GlobalController () {
	}

	public static GlobalController getInstance () {
		return globalController;
	}
	
	
	
	/**
	 * Main Player
     	 **/
	public MainCharacterController MainCharacter {
		set{this.mainCharacter = value;
			playerList[0] = this.mainCharacter;}
    	get{return this.mainCharacter;}
  	}

	public int addPlayer(MainCharacterController player) {
		playerList.Add(player);
		return playerList.IndexOf(player);
	}
	public MainCharacterController getPlayer(int index) {
		return playerList[index];
	}
	
	public IList<MainCharacterController> getPlayers() {
		return playerList;
	}


}

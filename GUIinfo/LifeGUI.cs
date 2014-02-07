using UnityEngine;
using System.Collections;
using Unity_VR.Mobs;
using Unity_VR.Global;

public class LifeGUI : MonoBehaviour {

	private Mob mob;

	// Use this for initialization
	void Start () {
		mob = GlobalController.getInstance().MainCharacter.Data.playerStatus.mob;
		guiText.fontSize = 20;
		guiText.color = Color.green;
		guiText.transform.position = new Vector3(0.02f,0.98f,0f);
	}
	
	// Update is called once per frame
	void Update () {
		string str = "";
		float max = mob.maxLife/50f;
		float now = mob.life;
		for (float i=0f;i<now;i+=max) {
			str += "/";
		}
		guiText.text = "Life ("+now+"): "+str;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eyes : ExpressionStatus {

	private GameObject eyes;
	private GameObject player;
	private IList<GameObject> playerList = new List<GameObject>();
	
	protected IList<ExpressionStatus> ignoreList = new List<ExpressionStatus>();

	public Eyes (GameObject eyes, GameObject player) {
		this.eyes = eyes;
		this.player = player;
		IList<MainCharacterController> players = GlobalController.getInstance().getPlayers();
		foreach (MainCharacterController p in players) {
			if (p) {
//				Debug.Log(player.Data.mainPlayer);
				playerList.Add(p.Data.getJoint((int)PlayerJoint.Head));
			}
		}
	}

	public bool isEnable() {
		return eyes;
	}
	public void actionStart() {
	}
	public void action(float time) {
//		if (!isActionAble()) {
//			return;
//		}
		//@TODO mock action 2013/12/26
//		Debug.Log(playerList[0]);
		float dis = Vector3.Distance(eyes.transform.position,playerList[0].transform.position);
		
		if (10f<dis) {
			return;
		}

		Vector3 eP = player.transform.TransformDirection(Vector3.forward);
		Vector3 pP = playerList[0].transform.position - eyes.transform.position;
		float angleA = Vector3.Angle(new Vector3(eP.x,0f,eP.z),new Vector3(pP.x,0f,pP.z));
		float angleB = Vector3.Angle(new Vector3(1f,0f,0f),new Vector3(dis,pP.y,0f));
//		Debug.Log(eP+"            "+pP+"       "+dis);
//		Debug.Log("EYE :"+player+"   ANGLE_A :"+angleA+"   ANGLE_B :"+angleB+"   DIS :"+dis);

		if (40f<angleA||15f<angleB) {
			return;
		}

		Vector3 pos = Quaternion.Euler(0,player.transform.eulerAngles.y,0) * pP;
		if (pos.z>0f) {
			angleA *= -1;
		}
		if (pos.y>0f) {
			angleB *= -1;
		}
		Vector3 eyeAngle = new Vector3(angleB*0.2f-3f,angleA*0.2f,0f);
		Debug.Log("EYEANGLE :"+eyeAngle+"   EYE :"+eyes);
		eyes.transform.localRotation = Quaternion.Euler(eyeAngle);

	}
	public bool isActive() {
		return true;
	}
	public void addIgnoreAction(ExpressionStatus es) {
		ignoreList.Add(es);
	}
	
	public void addIgnoreActions(params ExpressionStatus[] ess) {
		foreach (ExpressionStatus es in ess) {
			addIgnoreAction(es);
		}
	}

	protected bool isActionAble() {
		foreach (ExpressionStatus es in ignoreList) {
			if (es.isActive()) {
				return false;
			}
		}
		return true;
	}
}

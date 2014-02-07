using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity_VR.Mobs;
using Unity_VR.MainPlayer;
using Unity_VR.Global;
using Unity_VR.UseAnothoerLibraryClasses.mmdForUnity.ExpressionState;

public class NPC_AI_01 : MonoBehaviour , CollisionEventInterface{

	private MainCharacterData data = null;

	private List<ExpressionStatus> actionList = new List<ExpressionStatus>();
	private IDictionary<string,Action> actionDic = new Dictionary<string,Action>();

	public Type getActionType() {
		return typeof(MobParts);
	}

	public void action(ICollisionMaterial player, ICollisionMaterial colObj) {
//	public void action<MobParts,MobParts>(MobParts player, MobParts colObj) {
//		Debug.Log(player+" ::::: "+colObj);
		ICollisionMaterial partP = player;//.GetComponent<MobParts>();
		ICollisionMaterial partC = colObj;//.GetComponent<MobParts>();
		if (partC!=null&&partC.PlayerID==0) {
			Debug.Log("IS PLAYER");
			if (0<player.name.IndexOf("髪")) {
				actionDic["smile"]();
			}
			if (0<player.name.IndexOf("胸")) {
				actionDic["tear"]();
			}
			if (0<player.name.IndexOf("スカート")) {
				actionDic["angry"]();
			}
		}
	}

	void Start () {
		MainCharacterController cc = this.gameObject.GetComponent<MainCharacterController>();
		if (cc) {
			data = cc.Data;
			MorphManager morphManager = data.getMorphManager();

			// 視線
			ExpressionStatus eyes = new Eyes(data.getJoint((int)PlayerJoint.Eyes),this.gameObject);
			if (eyes.isEnable()) actionList.Add(eyes);
			// まばたき
			ExpressionStatus eyelid = new Blink(morphManager);
			if (eyelid.isEnable()) actionList.Add(eyelid);
			// 照れ
			ExpressionStatus shy = new Shy_Distance(morphManager,this.gameObject);
			if (shy.isEnable()) actionList.Add(shy);

			// 笑顔
			ExpressionStatus wa = new MorphEvent(morphManager,"ワ",1f,0.34f);
			if (wa.isEnable()) actionList.Add(wa);
			ExpressionStatus smile = new MorphEvent(morphManager,"笑い",1f);
			if (smile.isEnable()) actionList.Add(smile);
			actionDic.Add("smile",() => {wa.actionStart();smile.actionStart();});

			// 泣き
			ExpressionStatus tear = new MorphEvent(morphManager,"涙",2f,0.7f);
			if (tear.isEnable()) actionList.Add(tear);
			ExpressionStatus ooo = new MorphEvent(morphManager,"お",0.2f,0.5f);
			if (ooo.isEnable()) actionList.Add(ooo);
			ExpressionStatus zitome = new MorphEvent(morphManager,"じと目",1.8f,0.4f);
			if (zitome.isEnable()) actionList.Add(zitome);
			ExpressionStatus eyeaway = new EyeEvent(data.getJoint((int)PlayerJoint.Eyes),1.0f);
			if (eyeaway.isEnable()) actionList.Add(eyeaway);
			actionDic.Add("tear",() => {tear.actionStart();ooo.actionStart();zitome.actionStart();eyeaway.actionStart();});

			// 怒り
			ExpressionStatus angry = new MorphEvent(morphManager,"怒り",1f,1f);
			if (angry.isEnable()) actionList.Add(angry);
			ExpressionStatus haxu = new MorphEvent(morphManager,"はぅ",1f,0.2f);
			if (haxu.isEnable()) actionList.Add(haxu);
			ExpressionStatus henozi = new MorphEvent(morphManager,"∧",1f,1f);
			if (henozi.isEnable()) actionList.Add(henozi);
			actionDic.Add("angry",() => {angry.actionStart();haxu.actionStart();henozi.actionStart();});

			// ignore 
			eyes.addIgnoreActions(eyeaway);
			eyelid.addIgnoreActions(smile,zitome);
			wa.addIgnoreActions(zitome,ooo,tear);
			smile.addIgnoreActions(zitome,ooo,tear);
			angry.addIgnoreActions(zitome,ooo,tear,wa,smile);
			haxu.addIgnoreActions(zitome,ooo,tear,wa,smile);
			henozi.addIgnoreActions(zitome,ooo,tear,wa,smile);
		}
	}

	void Update() {
		float dTime = Time.deltaTime;
		foreach (ExpressionStatus es in actionList) {
			es.action(dTime);
		}
	}
}

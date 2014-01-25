using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MorphBaseStatus : ExpressionStatus {

	protected MorphBase actionObj;
	protected float actionTime = 0.0f;

	protected float initWeight = 0.0f;

	protected float maxActionTime = 0.1f;
	protected float maxWeight = 1.0f;
	
	protected IList<ExpressionStatus> ignoreList = new List<ExpressionStatus>();
	
	public MorphBaseStatus(MorphManager morphManager, string morphName) {
		morphSet(morphManager,morphName);
	}
	public MorphBaseStatus(MorphManager morphManager, string morphName, float maxActionTime) {
		morphSet(morphManager,morphName);
		this.maxActionTime = maxActionTime;
	}
	public MorphBaseStatus(MorphManager morphManager, string morphName, float maxActionTime, float maxWeight) {
		morphSet(morphManager,morphName);
		this.maxActionTime = maxActionTime;
		this.maxWeight = maxWeight;
	}
	public MorphBaseStatus(MorphManager morphManager, string morphName, float maxActionTime, float maxWeight, float initWeight) {
		morphSet(morphManager,morphName);
		this.maxActionTime = maxActionTime;
		this.maxWeight = maxWeight;
		this.initWeight = initWeight;
	}

	private void morphSet(MorphManager morphManager, string morphName) {
		if (morphManager) {
			MorphBase[] mb = morphManager.morphs;
			foreach (MorphBase m in mb) {
				if (m.name.Equals(morphName)) {
					actionObj = m;
				}
			}
		}
	}

	public bool isEnable() {
		return actionObj;
	}
	
	public abstract void actionStart();
	public abstract void action(float time);

	public bool isActive() {
		return 0.0f<actionTime;
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
//				Debug.Log (this+"  "+es);
				return false;
			}
		}
		return true;
	}

	protected void resetAction() {
		actionObj.group_weight = initWeight;
		actionTime = 0.0f;
	}

	protected void start () {
		actionObj.group_weight = maxWeight;
		actionTime = maxActionTime;
	}

	protected void actionRun(bool judge, float time) {
		if (!isActionAble()) {
			resetAction();
			return;
		}
//		if (this is MorphEvent) Debug.Log (this+" : "+actionTime+"  ("+maxActionTime+")   "+this.actionObj);
		if (isActive()) {
			actionTime -= time;
		} else if (judge) {
			actionObj.group_weight = maxWeight;
			actionTime = maxActionTime;
		} else {
			resetAction();
		}
	}
}

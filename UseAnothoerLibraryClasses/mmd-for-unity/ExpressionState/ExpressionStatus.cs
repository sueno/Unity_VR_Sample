using UnityEngine;
using System;
using System.Collections;

public interface ExpressionStatus {
	bool isEnable();
	void actionStart();
	void action(float time);
	bool isActive();
	void addIgnoreAction(ExpressionStatus es);
	void addIgnoreActions(params ExpressionStatus[] ess);
}

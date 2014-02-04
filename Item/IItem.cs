using UnityEngine;
using System.Collections;

public interface IItem {
	void hold();
	void release();

	void use();

	GameObject getGameObject();
}

public interface IUseAbleItem : IItem {
}

public interface IEatAbleItem : IItem {
}

public interface IEquipAbleItem : IItem {
}
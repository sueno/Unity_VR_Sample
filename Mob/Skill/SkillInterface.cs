using UnityEngine;
using System.Collections;

public interface SkillInterface {

	bool activate(int i);

	bool action();
	float getDamage(float d);
	
	GameObject[] trailEffect();
	GameObject[] hitEffect();
}

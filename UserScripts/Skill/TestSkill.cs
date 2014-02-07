using UnityEngine;
using System.Collections;
using Unity_VR.Mobs.Skill;

public class TestSkill : SkillInterface {

	private int activeTime = 2;

	private int count = 8;
	private GameObject[] trailObject;
	private GameObject[] hitObject;
	
	
	public TestSkill () {
		trailObject = new GameObject[0];
		GameObject hit1 = (GameObject)Resources.Load("Hit_1");
		hitObject = new GameObject[1]{hit1};
	}

	public bool activate(int i) {
		return (i==activeTime);
	}

	public bool action() {
		count--;
//		Debug.Log("##############################    "+count);
		return 0<count;
	}
	public float getDamage(float d) {
		return 200;
	}
	
	public GameObject[] trailEffect() {
		return trailObject;
	}
	public GameObject[] hitEffect() {
		return hitObject;
	}
}

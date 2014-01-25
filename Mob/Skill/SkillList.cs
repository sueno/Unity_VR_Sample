using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SkillList{

	private IList<SkillInterface> skillList;

	public SkillList() {
		skillList = new List<SkillInterface>();
	}

	public SkillInterface skillActivate(int i) {
		foreach (SkillInterface skill in skillList) {
			if (skill.activate(i)) {
				return skill;
			}
		}
		return null;
	}
}

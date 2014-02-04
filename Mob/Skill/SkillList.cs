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

	public void addSkill(SkillInterface skill) {
		skillList.Add(skill);
	}

	public void insertSkill(int index, SkillInterface skill) {
		int i = index<skillList.Count ? index : skillList.Count-1;
		skillList.Insert(i,skill);
	}
}

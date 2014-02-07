using UnityEngine;
using System.Collections;

namespace Unity_VR.Mobs.Skill
{

    public interface SkillInterface
    {

        bool activate(int i);

        bool action();
        float getDamage(float d);

        GameObject[] trailEffect();
        GameObject[] hitEffect();
    }
}
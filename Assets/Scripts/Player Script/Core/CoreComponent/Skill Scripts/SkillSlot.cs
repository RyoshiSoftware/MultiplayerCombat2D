using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] public Skill skill;

    public void SwapSkill(Skill newSkill)
    {
        skill = newSkill;
    }
    
    public void Cooldown(float delta, float coolDownFactor)
    {
        if (skill == null) return ;

        if (skill.isEmergency)
        {
            skill.Cooldown(delta);
        }
        else
        {
            skill.Cooldown(delta * coolDownFactor);
        }
    }
}

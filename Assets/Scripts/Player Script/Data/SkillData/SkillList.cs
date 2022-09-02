using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityList", menuName = "ScriptableObject/AbilityList")]
public class SkillList : ScriptableObject
{
    public List<SkillSlot> skillList;
}

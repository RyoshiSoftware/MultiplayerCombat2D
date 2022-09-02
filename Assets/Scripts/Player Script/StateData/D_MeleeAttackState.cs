using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "D_MeleeAttackState", menuName = "ScriptableObject/Data/D_MeleeAttackState")]
public class D_MeleeAttackState : ScriptableObject
{
    [Header("Attack Component")]
    public string[] attackAnimBoolName;
    public AttackSpawnData attackSpawnData;
    public AttackDamageData damageData;

}

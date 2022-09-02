using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDamageData
{
    [Header("Health Damage")]
    public float baseAttackDamage;
    public UDictionary<CharacterStat, float> damageFactorDictionary;
    public float finalAttackDamage; //todo set private

    [Header("Destroy Blocking Damage")]
    public float blockingGaugeDamage;

    [Header("Knockback")]
    [KnockBackType] public float knockBackStrength;
}

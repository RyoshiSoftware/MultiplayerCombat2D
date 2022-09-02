using UnityEngine;

[CreateAssetMenu(fileName = "D_AuraState", menuName = "ScriptableObject/Data/D_AuraState")]
public class D_AuraState : ScriptableObject
{
    public float chargingTime;
    public Buff abilityFastCoolDownBuff;
}

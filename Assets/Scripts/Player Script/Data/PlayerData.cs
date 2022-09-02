using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header ("Entity")]
    public D_Entity entityData;

    public D_AuraState auraData;
    public D_BlockState blockData;
    public D_ChargeState chargeData;
    public D_DashState dashData;
    public D_FallState fallData;
    public D_FlyState flyData;
    public D_HitState hitData;
    public D_IdleState idleData;
    public D_KnockbackState knockbackData;
    public D_MeleeAttackState meleeAttackState;
    public D_MoveState moveData;
    public D_RangeAttackState rangeAttackState;
    public D_StunState stunData;
    public D_SuperFlightState superFlightData;

    public void Initialize() 
    {
        entityData.Initialize();
    }
}

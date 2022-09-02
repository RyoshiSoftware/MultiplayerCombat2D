using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerHitState : PlayerNegativeState
{
    public PlayerHitState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        ResetState();
        SpawnVFX();
    }

    

    public override void TakeDamage(Vector2 attackDirection, AttackDamageData attackDamageData)
    {
        ResetState();
    }

    public override void ResetState()
    {
        timeToGetOutOfNegativeState = data.hitData.hitRecoverTime;
    }
    

    public override void LogicsUpdate()
    {
        base.LogicsUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerFallState : PlayerNegativeState
{

    public PlayerFallState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        timeToGetOutOfNegativeState = data.fallData.fallRecoverTime;
    }


    public override void Exit()
    {
        base.Exit();
    }


    public override void LogicsUpdate()
    {
        base.LogicsUpdate();

        
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

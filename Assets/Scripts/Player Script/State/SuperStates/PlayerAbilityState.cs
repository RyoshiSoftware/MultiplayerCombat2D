using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{    
    public PlayerAbilityState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();

        stateMachine.DisableChangeState();
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.EnableChangeState();
        player.inputHandler.ResetInput();
        player.ResetOutOfCombatState();
    }

    public override void LogicsUpdate()
    {
        if (isAnimationFinished)
        {
            ChangeToDefaultState();
        }
    }
}

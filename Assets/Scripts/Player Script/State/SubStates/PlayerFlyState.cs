using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

public class PlayerFlyState : PlayerPassiveState
{
    CoroutineHandle flyingCoroutine;

    
    public PlayerFlyState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        flying.onStartFlyCoroutine += stateMachine.DisableChangeState;
        flying.onFinishFlyCoroutine += OnFinishState;
        anim.ChangeToFlyAnimator();

        StartFlyingUp();
    }

    void StartFlyingUp()
    {
        flying.SetFlyPosition();
        
        if (flying.flyHeight <= 0)
        {
            UseInput();
            flyingCoroutine = Timing.RunCoroutine(flying.FlyingUpCoroutine(data.flyData.flySpeed, data.flyData.maxFlyHeight), Segment.Update);
        }
    }

    public override void LogicsUpdate()
    {
        base.LogicsUpdate();

        if (!flying.isFlyingCoroutine)
        {
            ChangeToMoveState();
        }

        StartFlyingDown();
    }

    void ChangeToMoveState()
    {
        if (movementInput.magnitude != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    void StartFlyingDown()
    {
        if (player.inputHandler.flyInput && flying.flyHeight > 0)
        {
            UseInput();

            Timing.KillCoroutines(flyingCoroutine);

            flying.CalculateFlyHeight();
            flyingCoroutine = Timing.RunCoroutine(flying.FlyingDownCoroutine(data.flyData.flySpeed, data.flyData.maxFlyHeight), Segment.Update);
        }
    }

    public override void UseInput()
    {
        player.inputHandler.UseFlyInput();
    }

    public override void Exit()
    {
        base.Exit();

        if (flying.isFlying)
        {
            flying.SetBackToFlyPosition();
        }
        
        flying.onFinishFlyCoroutine -= OnFinishState;
        flying.onStartFlyCoroutine -= stateMachine.DisableChangeState;
    }

    void OnFinishState()
    {
        stateMachine.EnableChangeState();
        
        if (flying.flyHeight <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Hover();
    }

    void Hover()
    {   
        flying.Hover(data.flyData.hoverSpeed, data.flyData.maxHoverHeight);
    }
}

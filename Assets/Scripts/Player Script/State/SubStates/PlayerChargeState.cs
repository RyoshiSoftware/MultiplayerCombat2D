using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeState : PlayerAbilityState
{
    public PlayerChargeState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.aura.SetActive(true);
        player.ResetAuraTransform();
        skillController.ResetSkillCharging();
    }


    public override void Exit()
    {
        base.Exit();

        player.aura.SetActive(false);
    }


    public override void LogicsUpdate()
    {
        base.LogicsUpdate();

        skillController.ChargeSkill();

        if (!player.inputHandler.skillInput)
        {
            ActivateSkill();
        }
    }

    void ActivateSkill()
    {
        skillController.StopCharging();
        stateMachine.ChangeState(player.skillState);
    }
}

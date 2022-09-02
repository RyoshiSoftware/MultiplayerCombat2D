using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNegativeState : PlayerState
{
    protected float timeToGetOutOfNegativeState;
    
    public PlayerNegativeState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.DisableChangeState();
        player.DisableChangeDirection();
    }


    public override void Exit()
    {
        base.Exit();

        stateMachine.EnableChangeState();
        player.EnableChangeDirection();
    }


    public override void LogicsUpdate()
    {
        base.LogicsUpdate();

        UsingEmergencySkill();

        ReduceNegativeStateTime();
    }

    void UsingEmergencySkill()
    {
        if (player.inputHandler.skillInput)
        {
            if (skillController.IsSkillEmergency())
            {
                stateMachine.ChangeState(player.skillState);
            }
        }
    }

    void ReduceNegativeStateTime()
    {
        if (timeToGetOutOfNegativeState >= 0)
        {
            timeToGetOutOfNegativeState -= Time.deltaTime;

            if (timeToGetOutOfNegativeState <= 0)
            {
                GetOutOfNegativeState();
            }
        }
    }

    public virtual void GetOutOfNegativeState()
    {
        ChangeToDefaultState();
        player.ResetOutOfCombatState();
    }

    public override void TakeDamage(Vector2 attackDirection, AttackDamageData attackDamageData)
    {

    }
}

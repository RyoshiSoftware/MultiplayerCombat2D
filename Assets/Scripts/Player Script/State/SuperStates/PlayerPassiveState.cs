using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveState : PlayerState
{
    protected Vector2 movementInput;

    public PlayerPassiveState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
        
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void LogicsUpdate()
    {
        movementInput = player.inputHandler.movementInput;

        if (stateMachine.canChangeState)
        {
            if (player.inputHandler.auraInput && data.auraData.abilityFastCoolDownBuff.isFinished)
            {
                stateMachine.ChangeState(player.auraState);
            }
            else if (player.inputHandler.blockInput)
            {
                stateMachine.ChangeState(player.blockState);
            }
            else if (player.inputHandler.dashInput)
            {
                stateMachine.ChangeState(player.dashState);
            }
            else if (player.inputHandler.meleeAttackInput)
            {
                stateMachine.ChangeState(player.meleeAttackState);
            }
            else if (player.inputHandler.rangeAttackInput)
            {
                stateMachine.ChangeState(player.rangeAttackState);
            }
            else if (player.inputHandler.skillInput)
            {
                if (!CheckIsSkillValid()) return;

                if (skillController.DoesSkillNeedCharging())
                {
                    stateMachine.ChangeState(player.chargeState);
                }
                else
                {
                    stateMachine.ChangeState(player.skillState);
                }
            }
        }
    }

    bool CheckIsSkillValid()
    {
        if (skillController.IsSkillEmergency()) return false;

        if (skillController.DoesSkillNeedTarget() && player.target) return true;

        return false;
    }

    public override void PhysicsUpdate()
    {
        
    }

}

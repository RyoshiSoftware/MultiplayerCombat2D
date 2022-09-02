using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerAbilityState
{
    string skillAnimBoolName;
    
    public PlayerSkillState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        PlaySkillAnimation();

        skillController.onFinishSkill += ChangeStateAfterFinishingSkill;

        Activate();
    }

    void PlaySkillAnimation()
    {
        anim.Play(skillController.GetSkillAnimId());
    }

    void Activate()
    {
        player.ResetOutOfCombatState();

        skillController.ActivateSkill();
    }

    public override void Exit()
    {
        base.Exit();

        skillController.onFinishSkill -= ChangeStateAfterFinishingSkill;
    }

    void ChangeStateAfterFinishingSkill()
    {
        ChangeToDefaultState();
    }
}

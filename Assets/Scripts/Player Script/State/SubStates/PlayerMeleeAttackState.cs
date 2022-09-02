using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttackState : PlayerAbilityState
{
    int punch = AnimationData.Punch;
    int kick = AnimationData.Kick;
    int index;
    
    public PlayerMeleeAttackState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.SetPlayerDirectionToTarget();

        PlayRandomAnimation();
    }

    void PlayRandomAnimation()
    {
        index = Random.Range(0, 2);

        if (index == 0)
        {
            anim.Play(punch);
        }
        else
        {
            anim.Play(kick);
        }
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

    
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        Attack();

        UseInput();

        SpawnVFX();
    }

    void Attack()
    {
        combat.MeleeAttack(data.meleeAttackState.attackSpawnData, data.meleeAttackState.damageData, player.gameObject.layer);
    }

    public override void UseInput()
    {
        player.inputHandler.UseMeleeAttack();
    }

    public override void SpawnVFX()
    {
        if (index == 0)
        {
            vfxController.SpawnVFX(vfxController.data.punchVFX, player.playerDirection);
        }
        else
        {
            vfxController.SpawnVFX(vfxController.data.kickVFX, player.playerDirection);
        }
    }
}

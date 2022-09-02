using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeAttackState : PlayerAbilityState
{
    float fireRateTimeElapse;
    float stopTimeElapse;

    Vector2 movementInput;
    int animIdRangeAttackMove = AnimationData.RangeAttackMove;

    public PlayerRangeAttackState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animId) : base(core, player, data, stateMachine, animId)
    {

    }

    public override void Enter()
    {
        base.Enter();

        fireRateTimeElapse = 0f;
        stopTimeElapse = 0f;

        player.SetPlayerDirectionToTarget();

        SpawnVFX();
    }

    public override void SpawnVFX()
    {
        vfxController.SpawnVFX(vfxController.data.kiBlastVFX, player.playerDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicsUpdate()
    {
        base.LogicsUpdate();

        GetMovementInput();

        GetAttackAndFinsishAttackTime();
    }

    void GetMovementInput()
    {
        movementInput = player.inputHandler.movementInput;
    }

    private void GetAttackAndFinsishAttackTime()
    {
        fireRateTimeElapse -= Time.deltaTime;

        if (player.inputHandler.rangeAttackInput)
        {
            stopTimeElapse = 0;

            if (fireRateTimeElapse <= 0f)
            {
                fireRateTimeElapse = data.rangeAttackState.fireRate;
                Attack();
            }
        }
        else
        {
            stopTimeElapse += Time.deltaTime;
            if (stopTimeElapse >= data.rangeAttackState.stopTimeAfterFire)
            {
                FinishAttack();
            }
        }
    }


    void Attack()
    {
        combat.RangeAttack(data.rangeAttackState.attackSpawnData, data.rangeAttackState.projectileData);
    }

    void FinishAttack()
    {
        ChangeToDefaultState();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Move();
    }

    void Move() 
    {
        movement.Move(movementInput * data.moveData.movementSpeed);

        if (movementInput.magnitude != 0)
        {
            ChangeToKiMoveAnimation();
        }
        else
        {
            ChangeToDefaultAnimation();
        }
    }

    void ChangeToKiMoveAnimation()
    {
        anim.Play(animIdRangeAttackMove);
    }

    void ChangeToDefaultAnimation()
    {
        anim.Play(animId);
    }
}

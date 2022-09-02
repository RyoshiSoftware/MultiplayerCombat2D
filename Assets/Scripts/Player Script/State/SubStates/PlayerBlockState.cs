using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerAbilityState
{
    Vector2 attackDirectionVector;
    
    public PlayerBlockState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
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
        base.LogicsUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!player.inputHandler.blockInput)
        {
            ChangeToDefaultState();
        }
    }

    public override void UseInput()
    {
        
    }

    public override void TakeDamage(Vector2 attackDirection, AttackDamageData attackDamageData)
    {
        this.attackDirectionVector = attackDirection;
        
        if (CheckingIfAttackFromBehind(attackDirection))
        {
            stateMachine.ChangeState(player.stunState);
        }
        else
        {
            TakeBlockingGaugeDamage(attackDamageData);

            SpawnVFX();
        }
    }

    bool CheckingIfAttackFromBehind(Vector2 attackDirection)
    {
        return Vector2.Angle(player.playerDirectionVector, attackDirection) < 45;
    }

    void TakeBlockingGaugeDamage(AttackDamageData attackDamageData)
    {
        data.entityData.blockingCurrentGauge -= attackDamageData.blockingGaugeDamage;

        if (data.entityData.blockingCurrentGauge <= 0)
        {
            player.SetPlayerDirectionToAttack(attackDirectionVector);

            stateMachine.ChangeState(player.stunState);

            SpawnBreakBlockVFX();
        }
    }

    public override void SpawnVFX()
    {
        Direction blockDirection = GetBlockSpawnDirection(attackDirectionVector);

        vfxController.SpawnVFX(vfxController.data.blockEplosionVFX, blockDirection);
        vfxController.SpawnVFX(vfxController.data.blockKiBlastVFX, blockDirection);
    }

    void SpawnBreakBlockVFX()
    {
        vfxController.SpawnDefaultVFX(vfxController.data.doubleShockWaveVFX);
    }


    Direction GetBlockSpawnDirection(Vector2 attackDirection)
    {
        return HelperMethods.GetReverseDirection(HelperMethods.GetDirectionFromVector(attackDirection.x, attackDirection.y));
    }
}

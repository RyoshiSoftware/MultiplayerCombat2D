using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerKnockbackState : PlayerNegativeState
{
    float spawnWindVFXTimeElapse;

    VFX vfxToSpawn;
    KnockBackType knockBackType;
    
    public PlayerKnockbackState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        FallFromFlying();
        ResetState();
        SpawnVFX();
    }

    void FallFromFlying()
    {
        if (flying.isFlying)
        {
            Timing.RunCoroutine(flying.FlyingDownCoroutine(data.flyData.flySpeed, data.flyData.maxFlyHeight));
        }
    }

    public override void TakeDamage(Vector2 attackDirection, AttackDamageData attackDamageData)
    {
        ResetState();
    }

    public override void ResetState()
    {
        if (knockBackType == KnockBackType.strong)
        {
            timeToGetOutOfNegativeState = data.knockbackData.strongKnockBackRecoverTime;
            vfxToSpawn = vfxController.data.strongKnockbackVFX;
        }
        else if (knockBackType == KnockBackType.medium)
        {
            timeToGetOutOfNegativeState = data.knockbackData.mediumKnockBackRecoverTime;
            vfxToSpawn = vfxController.data.mediumKnockbackVFX;
        }
    }

    public override void SpawnVFX()
    {
        vfxController.SpawnVFX(vfxController.data.strongKnockbackVFX, player.playerDirection);
    }


    public override void Exit()
    {
        base.Exit();
    }


    public override void LogicsUpdate()
    {
        base.LogicsUpdate();

        ElapseTimeToSpawnWindVFX();

        StartSlowingDown();
    }

    public override void GetOutOfNegativeState()
    {
        if (knockBackType != KnockBackType.strong)
        {
            base.GetOutOfNegativeState();
        }
        else
        {
            player.ResetOutOfCombatState();
            stateMachine.ChangeState(player.fallState);
        }
    }


    void ElapseTimeToSpawnWindVFX()
    {
        spawnWindVFXTimeElapse -= Time.deltaTime;

        if (spawnWindVFXTimeElapse <= 0)
        {
            spawnWindVFXTimeElapse = data.hitData.spawnWindTime;

            SpawnWindVFX();
        }
    }

    void SpawnWindVFX()
    {
        vfxController.SpawnVFX(vfxController.data.knockbackWindVFX, player.playerDirection);
    }

    
    void StartSlowingDown()
    {
        if (CheckingToStartRecover())
        {
            Timing.RunCoroutine(SlowingDownCoroutine());
        }
    }

    bool CheckingToStartRecover()
    {
        return timeToGetOutOfNegativeState <= data.knockbackData.strongKnockBackRecoverTime / 2;
    }

    IEnumerator<float> SlowingDownCoroutine()
    {
        float decreaseVelocityFactor = data.knockbackData.velocityDecreaseFactor;

        while (movement.GetVelocity().magnitude > 0.01f)
        {
            movement.MultiplyVelocityFactor(decreaseVelocityFactor);
            yield return Timing.WaitForOneFrame;
        }
    }


    public void SetKnockbackType(KnockBackType knockBackType)
    {
        this.knockBackType = knockBackType;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperFlightState : PlayerPassiveState
{
    float spawnVFXTimeElapse;
    
    public PlayerSuperFlightState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        player.aura.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.aura.SetActive(false);
    }

    public override void LogicsUpdate()
    {
        base.LogicsUpdate();

        ElapseTimeToSpawnVFX();

        if (movementInput.magnitude == 0)
        {
            ChangeToDefaultState();
        }
    }

    void ElapseTimeToSpawnVFX()
    {
        spawnVFXTimeElapse -= Time.deltaTime;

        if (spawnVFXTimeElapse <= 0)
        {
            spawnVFXTimeElapse = data.superFlightData.vfxTimeElapse;

            SpawnVFX();
        }
    }

    public override void SpawnVFX()
    {
        vfxController.SpawnVFX(vfxController.data.airDoubleVFX, player.playerDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Move();
        ChangeAuraPosition();
    }

    void Move() 
    {
        movement.Move(movementInput * data.superFlightData.movementSpeed);
    }

    void ChangeAuraPosition()
    {
        player.ChangeAuraPosition();
    }
}

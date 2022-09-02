using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAuraState : PlayerAbilityState
{
    float timeToFinish = 0;
    
    public PlayerAuraState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animName) : base(core, player, data, stateMachine, animName)
    {
    }


    public override void Enter()
    {
        base.Enter();

        player.aura.SetActive(true);

        timeToFinish = data.auraData.chargingTime;
    }


    public override void Exit()
    {
        base.Exit();

        player.aura.SetActive(false);
    }


    public override void LogicsUpdate()
    {
        base.LogicsUpdate();

        timeToFinish -= Time.deltaTime;

        CheckingIfFinishCharging();

        if (!player.inputHandler.auraInput)
        {
            ChangeToDefaultState();
        }
    }

    void CheckingIfFinishCharging()
    {
        if (timeToFinish <= 0)
        {
            buffableEntity.AddBuff(data.auraData.abilityFastCoolDownBuff);
            UseInput();
            
            ChangeToDefaultState();
        }
    }

    public override void UseInput()
    {
        player.inputHandler.UseAuraInput();
    }
}

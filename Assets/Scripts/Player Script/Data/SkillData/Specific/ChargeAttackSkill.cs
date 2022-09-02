using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[CreateAssetMenu(fileName = "ChargeAttackSkill", menuName = "ScriptableObject/Skill/ChargeAttackSkill")]
public class ChargeAttackSkill : Skill
{    
    protected FlightController flying;
    
    [Header("Charge Attack component")]
    [Header("Time component")]
    public float chargeTime;
    public float chargeTimeElapse; 
    public float maxActiveTime;
    public float activeTimeElapse; 


    [Header("Rotate component")]
    public float rotationMaxAngle;
    public float rotationSpeed;
    public float turningWaitTime;


    [Header("Move component")]
    public float chargeVelocity;

    [Header("Fly component")]
    public D_FlyState flyData;


    [Header("Attack component")]
    public AttackDamageData minAttackData;
    public AttackDamageData maxAttackData;
    [SerializeField] AttackDamageData attackDataHolder;
    public Vector2 attackBoxSize;

    
    [Header("Charging component")]
    CoroutineHandle chargingCoroutineHandle;
    CoroutineHandle turningCoroutineHandle;
    CoroutineHandle flyingCoroutineHandle;
    Vector2 movingDirection;

    public override void Initialize(Player player, Core core)
    {
        base.Initialize(player, core);

        flying = core.GetCoreComponent<FlightController>();
        animId = AnimationData.ChargeAttack;
    }

    public override void ResetSkillCharging()
    {
        chargeTimeElapse = 0;
        activeTimeElapse = activeTime;
    }

    public override void Activate()
    {
        base.Activate();

        activeTimeElapse = Mathf.Lerp(activeTime, maxActiveTime, chargeTimeElapse / chargeTime);
        
        ActivateCoroutine(player);
    }

    void ActivateCoroutine(Player player)
    {
        chargingCoroutineHandle = Timing.RunCoroutine(ChargingPlayer(player.target));
        turningCoroutineHandle = Timing.RunCoroutine(Turning(player.target));

        CheckIfNeedToChangeLayer();
    }

    IEnumerator<float> ChargingPlayer(Transform target) 
    {
        player.SetPlayerDirectionToTarget();
        movingDirection = (target.position - player.transform.position).normalized;

        while (true)
        {
            movement.SetVelocity(movingDirection * chargeVelocity);
            yield return Timing.WaitForOneFrame;
        }
    }

    IEnumerator<float> Turning(Transform target)
    {
        while (true)
        {
            float targetAngle = Vector2.SignedAngle(target.position - player.transform.position, movingDirection);

            if (Mathf.Abs(targetAngle) <= rotationMaxAngle)
            {
                if (targetAngle < 0)
                {
                    movingDirection = Quaternion.Euler(0, 0, rotationSpeed) * movingDirection;
                }
                else if (targetAngle > 0)
                {
                    movingDirection = Quaternion.Euler(0, 0, -rotationSpeed) * movingDirection;
                }
            }

            yield return Timing.WaitForSeconds(turningWaitTime);
        }
    }

    void CheckIfNeedToChangeLayer()
    {
        if (player.gameObject.layer > player.target.gameObject.layer)
        {
            flyingCoroutineHandle = Timing.RunCoroutine(flying.FlyingDownCoroutine(flyData.flySpeed, flyData.maxFlyHeight));
        }
        else if (player.gameObject.layer < player.target.gameObject.layer)
        {
            flyingCoroutineHandle = Timing.RunCoroutine(flying.FlyingUpCoroutine(flyData.flySpeed, flyData.maxFlyHeight));
        }
    }

    public override void StartCharging()
    {
        chargeTimeElapse = Mathf.Clamp(chargeTimeElapse + Time.deltaTime, 0, chargeTime);
    }

    public override void StopCharging()
    {
        base.StopCharging();

        CalculateAttackData();
    }

    void CalculateAttackData()
    {
        attackDataHolder.finalAttackDamage = Mathf.Lerp(minAttackData.finalAttackDamage, maxAttackData.finalAttackDamage, chargeTimeElapse / chargeTime);
        attackDataHolder.knockBackStrength = Mathf.Lerp(minAttackData.knockBackStrength, maxAttackData.knockBackStrength, chargeTimeElapse / chargeTime);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlaying) 
        {
            Deactivate();
            
            CheckIfTargetInRange();
        }
    }


    void CheckIfTargetInRange()
    {
        combat.CheckDamageInBoxRange(attackDataHolder, attackBoxSize, player.target.gameObject.layer);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        movement.StopMoving();

        Timing.KillCoroutines(chargingCoroutineHandle);
        Timing.KillCoroutines(turningCoroutineHandle);
    }
}

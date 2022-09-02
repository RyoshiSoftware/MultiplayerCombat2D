using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    #region State

    public PlayerAuraState auraState {get; private set;}
    public PlayerBlockState blockState {get; private set;}
    public PlayerChargeState chargeState {get; private set;}
    public PlayerDashState dashState {get; private set;}
    public PlayerFallState fallState {get; private set;}
    public PlayerFlyState flyState {get; private set;}
    public PlayerHitState hitState {get; private set;}
    public PlayerIdleState idleState {get; private set;}
    public PlayerKnockbackState knockbackState {get; private set;}
    public PlayerMeleeAttackState meleeAttackState {get; private set;}
    public PlayerMoveState moveState {get; private set;}
    public PlayerRangeAttackState rangeAttackState {get; private set;}
    public PlayerSkillState skillState {get; private set;}
    public PlayerStunState stunState {get; private set;}
    public PlayerSuperFlightState superFlightState {get; private set;}

    #endregion

    #region VFX

    public GameObject aura;

    #endregion


    #region Component
    [SerializeField] private PlayerData data;
    
    public Core core {get; private set;}

    private AnimationController anim;
    public BoxCollider2D col {get; private set;}
    public LayerController colController {get; private set;}
    [SerializeField] public PlayerInputHandler inputHandler;
    public PlayerStateMachine stateMachine {get; private set;}
    public SpriteRenderer sprite {get; private set;}
    public VFXController vfxController {get; private set;}

    #endregion

    #region Other Component

    [SerializeField] public Direction playerDirection;
    public Vector2 playerDirectionVector {get; private set;}
    public bool canChangeDirection {get; private set;}

    #endregion

    #region Target

    public Transform target;
    public Vector2 targetDirection {get; private set;}

    #endregion

    public Action<int> onChangeLayer;


    #region Unity callback method
    protected virtual void Awake() 
    {
        Getcomponent();
    }

    protected virtual void Getcomponent()
    {
        col = GetComponent<BoxCollider2D>();
        core = GetComponentInChildren<Core>();
        //inputHandler = GetComponent<PlayerInputHandler>(); //todo use serialize instead for tesing
        sprite = GetComponentInChildren<SpriteRenderer>();
        vfxController = GetComponent<VFXController>();

        stateMachine = new PlayerStateMachine();
    }

    protected virtual void Start() 
    {
        GetCoreComponent();
        CreateNewState();
        Initialize();
    }

    void GetCoreComponent()
    {
        anim = core.GetCoreComponent<AnimationController>();
        colController = core.GetCoreComponent<LayerController>();
    }

    void Initialize()
    {
        data.Initialize();
        PlayerInitialize();
        InitializeAllBuffs();
        
        stateMachine.Initialize(idleState);
    }

    void PlayerInitialize()
    {
        playerDirection = Direction.Down;
        playerDirectionVector = Vector2.down;
        EnableChangeDirection();
    }

    void InitializeAllBuffs()
    {
        core.GetCoreComponent<BuffableEntity>().InitializeAllBuffs(data);
    }

    protected virtual void CreateNewState()
    {
        auraState = new PlayerAuraState(core, this, data, stateMachine, AnimationData.Aura);
        blockState = new PlayerBlockState(core, this, data, stateMachine, AnimationData.Block);
        chargeState = new PlayerChargeState(core, this, data, stateMachine, AnimationData.Charge);
        dashState = new PlayerDashState(core, this, data, stateMachine, AnimationData.Dash);
        fallState = new PlayerFallState(core, this, data, stateMachine, AnimationData.Fall);
        flyState = new PlayerFlyState(core, this, data, stateMachine, AnimationData.Fly);
        hitState = new PlayerHitState(core, this, data, stateMachine, AnimationData.Hit);
        idleState = new PlayerIdleState(core, this, data, stateMachine, AnimationData.Idle);
        knockbackState = new PlayerKnockbackState(core, this, data, stateMachine, AnimationData.Knockback);
        meleeAttackState = new PlayerMeleeAttackState(core, this, data, stateMachine, -1);
        moveState = new PlayerMoveState(core, this, data, stateMachine, AnimationData.Move);
        rangeAttackState = new PlayerRangeAttackState(core, this, data, stateMachine, AnimationData.RangeAttack);
        skillState = new PlayerSkillState(core, this, data, stateMachine, -1);
        stunState = new PlayerStunState(core, this, data, stateMachine, AnimationData.Hit);
        superFlightState = new PlayerSuperFlightState(core, this, data, stateMachine, AnimationData.SuperFlight);
    }

    protected virtual void Update() 
    {
        stateMachine.currentState.LogicsUpdate();

        SetPlayerDirectionFromMovementInput();

        PassiveAttributeUpdate();
    }

    protected virtual void PassiveAttributeUpdate()
    {
        data.entityData.Cooldown(Time.deltaTime);
    }

    protected virtual void FixedUpdate() 
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    #endregion

    #region Collision

    public void DisableCollider()
    {
        col.enabled = false;
    }

    public void EnableCollider()
    {
        col.enabled = true;
    }

    #endregion

    #region State method

    public void ResetOutOfCombatState()
    {
        data.entityData.SuperFlightResetTime();
    }

    public bool IsInBlockState()
    {
        return stateMachine.currentState == blockState;
    }

    public bool IsStun()
    {
        return stateMachine.currentState == stunState;
    }
    
    #endregion

    #region Animation

    void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    #endregion

    #region Set Player Direction

    public void EnableChangeDirection()
    {
        canChangeDirection = true;
    }

    public void DisableChangeDirection()
    {
        canChangeDirection = false;
    }

    public void SetPlayerDirectionFromMovementInput()
    {
        Vector2 movementInput = inputHandler.movementInput;

        if (movementInput.magnitude != 0)
        {
            SetPlayerDirection(HelperMethods.GetDirectionFromVector(movementInput.x, movementInput.y));
        }
    }

    public void SetPlayerDirectionToTarget()
    {
        if (target != null)
        {
            targetDirection = (target.position - transform.position);

            SetPlayerDirection(HelperMethods.GetDirectionFromVector(targetDirection.x, targetDirection.y));
        }
    }

    public void SetPlayerDirectionToAttack(Vector2 attackDirecionVector)
    {
        Direction attackDirection = HelperMethods.GetDirectionFromVector(attackDirecionVector.x, attackDirecionVector.y);
        SetPlayerDirection(HelperMethods.GetReverseDirection(attackDirection));
    }

    public void SetPlayerDirection(Direction direction)
    {
        if (!canChangeDirection) return;

        playerDirection = direction;
        switch (playerDirection)
        {
            case Direction.Up:
                playerDirectionVector = Vector2.up;
                break;
            case Direction.Down:
                playerDirectionVector = Vector2.down;
                break;
            case Direction.Left:
                playerDirectionVector = Vector2.left;
                break;
            case Direction.Right:
                playerDirectionVector = Vector2.right;
                break;
        }

        anim.SetFloat("vertical", playerDirectionVector.y);
        anim.SetFloat("horizontal", playerDirectionVector.x);
    }

    #endregion

    #region Aura

    public void ChangeAuraPosition()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        Vector2 position = new Vector2(0, 0); 
        switch(playerDirection)
        {
            case Direction.Up:
            {
                rotation = Settings.leftRotation;
                position = data.entityData.auraUpPos;
                break;
            }
            case Direction.Down:
            {
                rotation = Settings.rightRotation;
                position = data.entityData.auraDownPos;
                break;
            }
            case Direction.Left:
            {
                rotation = Settings.downRotation;
                position = data.entityData.auraLeftPos;
                break;
            }
            case Direction.Right:
            {
                rotation = Settings.upRotation;
                position = data.entityData.auraRightPos;
                break;
            }
        }

        aura.transform.rotation = rotation;
        aura.transform.position = position + (Vector2)transform.position;
    }

    public void ResetAuraTransform()
    {
        aura.transform.localPosition = data.chargeData.auraPosition;
        aura.transform.rotation = data.chargeData.auraRotation;
    }

    #endregion

    #region OnDrawGizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(data.meleeAttackState.attackSpawnData.upPosition + (Vector2)transform.position, data.meleeAttackState.attackSpawnData.attackRadius);
        Gizmos.DrawWireSphere(data.meleeAttackState.attackSpawnData.downPosition + (Vector2)transform.position, data.meleeAttackState.attackSpawnData.attackRadius);
        Gizmos.DrawWireSphere(data.meleeAttackState.attackSpawnData.rightPosition + (Vector2)transform.position, data.meleeAttackState.attackSpawnData.attackRadius);
        Gizmos.DrawWireSphere(data.meleeAttackState.attackSpawnData.leftPosition + (Vector2)transform.position, data.meleeAttackState.attackSpawnData.attackRadius);
    }

    #endregion
}

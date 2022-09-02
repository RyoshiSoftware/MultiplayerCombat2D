using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Core core;

    #region Core component

    protected Movement movement
    {
        get
        {
            if (_movement == null)
            {
                _movement = core.GetCoreComponent<Movement>();
            }
            return _movement;
        }
        private set {}
    }
    private Movement _movement;

    protected FlightController flying
    {
        get
        {
            if (_flying == null)
            {
                _flying = core.GetCoreComponent<FlightController>();
            }
            return _flying;
        }
        private set {}
    }
    private FlightController _flying;


    protected Combat combat
    {
        get 
        {
            if (_combat == null)
            {
                _combat = core.GetCoreComponent<Combat>();
            }
            return _combat;
        }
        private set {}
    }
    private Combat _combat;

    protected VFXController vfxController
    {
        get 
        {
            if (_vfxController == null)
            {
                _vfxController = core.GetCoreComponent<VFXController>();
            }
            return _vfxController;
        }
        private set {}
    }
    private VFXController _vfxController;

    protected BuffableEntity buffableEntity
    {
        get 
        {
            if (_buffableEntity == null)
            {
                _buffableEntity = core.GetCoreComponent<BuffableEntity>();
            }
            return _buffableEntity;
        }
        private set {}
    }
    private BuffableEntity _buffableEntity;

    protected SkillController skillController
    {
        get 
        {
            if (_skillController == null)
            {
                _skillController = core.GetCoreComponent<SkillController>();
            }
            return _skillController;
        }
        private set {}
    }
    private SkillController _skillController;

    protected AnimationController anim
    {
        get 
        {
            if (_anim == null)
            {
                _anim = core.GetCoreComponent<AnimationController>();
            }
            return _anim;
        }
        private set {}
    }
    private AnimationController _anim;

    #endregion

    protected Player player;
    protected PlayerData data;
    protected PlayerStateMachine stateMachine;

    protected int animId;
    protected bool isAnimationFinished;

    public PlayerState(Core core, Player player, PlayerData data, PlayerStateMachine stateMachine, int animId)
    {
        this.core = core;
        this.player = player;
        this.data = data;
        this.stateMachine = stateMachine;
        this.animId = animId;

        movement = core.GetCoreComponent<Movement>();
        combat = core.GetCoreComponent<Combat>();
        vfxController = core.GetCoreComponent<VFXController>();
        anim = core.GetCoreComponent<AnimationController>();
    }

    public virtual void Enter()
    {
        anim.playerState = this;

        PlayAnimation();
        AddEvent();
    }

    public virtual void AddEvent() 
    {
        combat.onTakeDamage += TakeDamage;
    }

    void PlayAnimation()
    {
        if (animId == -1) return;

        anim.Play(animId);
    }

    public virtual void Exit() 
    {
        isAnimationFinished = false;
        
        RemoveEvent();
    }

    public virtual void RemoveEvent()
    {
        combat.onTakeDamage -= TakeDamage;
    }

    public void ChangeToDefaultState()
    {
        if (!flying.isFlying)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            stateMachine.ChangeState(player.flyState);
        }
    }

    public virtual void ResetState(){}

    public virtual void LogicsUpdate() 
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void TakeDamage(Vector2 attackDirection, AttackDamageData attackDamageData)
    {
        player.SetPlayerDirectionToAttack(attackDirection);
        
        FindNegativeStateToChange(attackDamageData);
    }

    void FindNegativeStateToChange(AttackDamageData attackDamageData)
    {
        KnockBackType knockBackType = HelperMethods.GetKnockBackType(attackDamageData.knockBackStrength);

        if (knockBackType == KnockBackType.weak)
        {
            stateMachine.ChangeState(player.hitState);
        }
        else
        {
            player.knockbackState.SetKnockbackType(knockBackType);
            stateMachine.ChangeState(player.knockbackState);
        }
    }

    public virtual void AnimationTrigger()
    {

    }

    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
    }

    public virtual void UseInput()
    {

    }

    public virtual void SpawnVFX()
    {

    }

    public virtual void ChangeLayer(int index)
    {
        player.onChangeLayer(index);
    }
}

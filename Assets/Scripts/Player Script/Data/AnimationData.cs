using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationData
{
    #region State

    public readonly static int Aura = Animator.StringToHash("Aura");
    public readonly static int Block = Animator.StringToHash("Block");
    public readonly static int Charge = Animator.StringToHash("Charge");
    public readonly static int Dash = Animator.StringToHash("Dash");
    public readonly static int Fall = Animator.StringToHash("Fall");
    public readonly static int Fly = Animator.StringToHash("Fly");
    public readonly static int Hit = Animator.StringToHash("Hit");
    public readonly static int Idle = Animator.StringToHash("Idle");
    public readonly static int Kick = Animator.StringToHash("Kick");
    public readonly static int Knockback = Animator.StringToHash("Knock");
    public readonly static int Move = Animator.StringToHash("Move");
    public readonly static int Punch = Animator.StringToHash("Punch");
    public readonly static int RangeAttack = Animator.StringToHash("Range Attack");
    public readonly static int RangeAttackMove = Animator.StringToHash("Range Attack Move");
    public readonly static int SuperFlight = Animator.StringToHash("Super Flight");

    #endregion

    #region Skill

    public readonly static int ChargeAttack = Animator.StringToHash("Charge Attack");
    public readonly static int Teleport = Animator.StringToHash("Teleport");

    #endregion
}

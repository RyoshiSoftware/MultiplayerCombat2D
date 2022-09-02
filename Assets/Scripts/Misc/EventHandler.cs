using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void StatDelegate(UDictionary<CharacterStat, int> statDictionary, bool isTesting); //todo remove bool

public static class EventHandler
{
    public static event Action<AnimatorOverrideController> ChangeAnimatorController;
    public static void CallChangeAnimatorController(AnimatorOverrideController animToChange)
    {
        if (ChangeAnimatorController != null)
            ChangeAnimatorController(animToChange);
    }

    public static event Action<Transform> LockOnAction;
    public static void CallLockOnAction(Transform targetTransform)
    {
        if (LockOnAction != null)
            LockOnAction(targetTransform);
    }

    public static event Action LockOffAction;
    public static void CallLockOffAction()
    {
        if (LockOffAction != null)
            LockOffAction();
    }

    public static event Action<SkillSlot> AbilityCooldownAction;
    public static void CallAbilityCooldownAction(SkillSlot abilityToAdd)
    {
        if (AbilityCooldownAction != null)
            AbilityCooldownAction(abilityToAdd);
    }

    // Assign stat point
    public static event Action<int> PhysiqueAssignPointAction;
    public static void CallPhysiqueAssignPointAction(int physique)
    {
        if (PhysiqueAssignPointAction != null)
            PhysiqueAssignPointAction(physique);
    }

    public static event Action<int> ForceAssignPointAction;
    public static void CallForceAssignPointAction(int force)
    {
        if (ForceAssignPointAction != null)
            ForceAssignPointAction(force);
    }

    // Assign damage for skills, attack, ...
    public static event StatDelegate OnChangedStatAction;
    public static void CallOnChangedStatAction(UDictionary<CharacterStat, int> statDictionary, bool isTesting) //todo remove istesting
    {
        if (OnChangedStatAction != null)
            OnChangedStatAction(statDictionary, isTesting);
    }
}

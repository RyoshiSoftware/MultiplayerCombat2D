using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : CoreComponent
{
    [System.Serializable]
    public struct AnimatorData 
    {
        public RuntimeAnimatorController defaultAnim;
        public AnimatorOverrideController flyAnim;
    }
    
    Animator anim;
    SpriteRenderer sprite;

    public AnimatorData data;
    public PlayerState playerState;


    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetBackToDefaultAnimator()
    {
        if (anim.runtimeAnimatorController == data.defaultAnim) return;
        
        anim.runtimeAnimatorController = data.defaultAnim;
        ChangeSortingLayer(0);
    }


    public void ChangeToFlyAnimator()
    {
        if (anim.runtimeAnimatorController == data.flyAnim) return;

        anim.runtimeAnimatorController = data.flyAnim;
        ChangeSortingLayer(10);
    }

    void ChangeSortingLayer(int value) 
    {
        sprite.sortingOrder = value;
    }

    public void SetFloat(string animName, float value)
    {
        anim.SetFloat(animName, value);
    }

    public void Play(int id)
    {
        anim.Play(id);
    }

    public void AnimationTrigger()
    {
        playerState.AnimationTrigger();
    }

    public void AnimationFinishTrigger()
    {
        playerState.AnimationFinishTrigger();
    }
}

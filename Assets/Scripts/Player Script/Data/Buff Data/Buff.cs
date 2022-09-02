using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    [Header("Buff component")]
    public float duration;

    public bool isDurationStack;

    public bool isEffectStack;

    // These are updating parameters (not need to assign)
    [Header("Abstract method")]  
    public float durationElapse;
    public int effectStacks;
    public bool isFinished;

    public void Tick(float delta)
    {
        durationElapse -= delta;
        if (durationElapse <= 0)
        {
            End();
            isFinished = true;
        }
    }

    public virtual void Activate() 
    {
        isFinished = false;
        
        if (isDurationStack || durationElapse <= 0)
        {
            durationElapse += duration;
        }

        if (isEffectStack || durationElapse <= 0)
        {
            ApplyEffect();
            
            effectStacks++;
        }

        
    }

    public virtual void Initialize(Core core, PlayerData data)
    {
        isFinished = true;
        durationElapse = 0;
    }

    protected abstract void ApplyEffect();
    public abstract void End();
}

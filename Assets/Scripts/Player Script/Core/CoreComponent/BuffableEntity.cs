using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffableEntity : CoreComponent
{
    [SerializeField] List<Buff> buffDictionary = new List<Buff>();

    public void InitializeAllBuffs(PlayerData data)
    {
        foreach(Buff buff in buffDictionary)
        {
            buff.Initialize(core, data);
        }
    }
    
    void Update() 
    {
        CoolDownBuff();
    }

    public void AddBuff(Buff buff)
    {
        if (buffDictionary.Contains(buff))
        {
            buff.Activate();
        }
    }

    // Updated every frame
    void CoolDownBuff()
    {
        foreach(Buff buff in buffDictionary)
        {
            if (buff.isFinished)
            {
                continue;
            }
            
            buff.Tick(Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core core;
    
    protected virtual void Awake() 
    {
        core = GetComponentInParent<Core>();

        AddCoreComponentToList();
    }

    void AddCoreComponentToList()
    {
        if (core != null)
        {
            core.AddCoreComponent(this);
        }
        else
        {
            Debug.LogError("There is no core");
        }
    }

    public virtual void LogicsUpdate() {}
    public virtual void PhysicsUpdate() {}

}

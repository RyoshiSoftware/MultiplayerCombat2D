using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    List<CoreComponent> coreComponentList = new List<CoreComponent>();

    public void LogicsUpdate()
    {
        foreach(CoreComponent component in coreComponentList)
        {
            component.LogicsUpdate();
        }
    }

    public void PhysicsUpdate()
    {
        foreach(CoreComponent component in coreComponentList)
        {
            component.PhysicsUpdate();
        }
    }

    public void AddCoreComponent(CoreComponent component)
    {
        if (coreComponentList.Contains(component)) return;

        coreComponentList.Add(component);
    }

    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var component = coreComponentList.OfType<T>().FirstOrDefault();

        if (component == null) 
        {
            Debug.LogError("There is not error on this object");
        }

        return component;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    public ObjectSpawner spawner;
    public Animator anim;

    void Awake() 
    {
        anim = GetComponent<Animator>();
    }
    
    // Used in animation clip
    void Release()
    {
        // Reset animator
        anim.Rebind();
        anim.Update(0f);
        
        spawner.pool.Release(gameObject);
    }

    public void ReleaseObject()
    {
        spawner.pool.Release(gameObject);
    }
}

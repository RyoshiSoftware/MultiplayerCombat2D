using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    public ObjectPool<GameObject> pool;
    public int amountToPool;
    GameObject prefabParent;

    void Awake() {
        pool = new ObjectPool<GameObject>(CreateObject, GetObjectFromPool, ReleaseObjectToPool, DestroyObject, false, amountToPool, amountToPool);
    }

    GameObject CreateObject()
    {
        if (prefabParent == null)
        {
            prefabParent = new GameObject(prefab.name);
        }

        return Instantiate(prefab, prefabParent.transform);
    }

    void GetObjectFromPool(GameObject go)
    {
        go.SetActive(true);
    }

    void ReleaseObjectToPool(GameObject go) 
    {
        go.SetActive(false);
    }

    void DestroyObject(GameObject go)
    {
        Destroy(go);
    }
}

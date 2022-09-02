using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectSpawnerManager : MonoBehaviour
{
    public List<ObjectSpawner> objectSpawnerList;

    void Start() 
    {
        objectSpawnerList = new List<ObjectSpawner>(GetComponentsInChildren<ObjectSpawner>());
    }

    public GameObject GetObjectFromPool(GameObject go)
    {
        foreach(ObjectSpawner objectSpawner in objectSpawnerList)
        {
            if (objectSpawner.prefab == go)
            {
                GameObject objectToSpawn = objectSpawner.pool.Get();

                if (objectToSpawn.TryGetComponent<SpawnableObject>(out SpawnableObject vfxAnim))
                {
                    vfxAnim.spawner = objectSpawner;
                }

                return objectToSpawn;
            }
        }

        return null;
    }
}

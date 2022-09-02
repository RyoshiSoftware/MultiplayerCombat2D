using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : CoreComponent
{
    Player player;
    Vector2 spawnPos;
    Quaternion spawnRot;

    [Header("VFX Component")]
    [SerializeField] public VFXData data;
    public ObjectSpawnerManager vfxSpawner;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>(); 
    }

    public void SpawnDefaultVFX(VFX vfx, Transform transform = null)
    {
        spawnPos = GetVFXDefaultPosition(vfx, transform);
        spawnRot = Quaternion.identity;

        SpawnGOWithRotation(vfx.prefab, spawnPos, spawnRot);
    } 

    Vector2 GetVFXDefaultPosition(VFX vfx, Transform transform = null)
    {
        if (transform == null)
        {
            return vfx.offset + this.transform.position;
        }
        
        return vfx.offset + transform.position;
    }


    public void SpawnVFX(VFX vfx, Direction direction) 
    {
        // spawnPos = GetVFXSpawnPosition(vfx, direction);
        // spawnRot = GetVFXSpawnRotation(vfx, direction);

        if (vfx.isSprite)
        {
            // SpawnGOWithAnimation(vfx.prefab, spawnPos, direction.ToString());
        }
        else
        {
            // SpawnGOWithRotation(vfx.prefab, spawnPos, spawnRot);
        }
    }

    Vector2 GetVFXSpawnPosition(VFX vfx, Direction direction)
    {
        Vector2 spawnPos = new Vector2(0, 0);
        
        switch (direction)
        {
            case Direction.Up:
                spawnPos = vfx.upPos + (Vector2)transform.position;
                break;

            case Direction.Down:
                spawnPos = vfx.downPos + (Vector2)transform.position;
                break;

            case Direction.Left:
                spawnPos = vfx.leftPos + (Vector2)transform.position;
                break;

            case Direction.Right:
                spawnPos = vfx.rightPos + (Vector2)transform.position;
                break;

            default:
                break;
        }

        return spawnPos;
    }   

    Quaternion GetVFXSpawnRotation(VFX vfx, Direction direction)
    {
        Quaternion spawnRot = Quaternion.Euler(0, 0, 0);
        
        switch (direction)
        {
            case Direction.Up:
                spawnRot = Settings.upRotation;
                break;

            case Direction.Down:
                spawnRot = Settings.downRotation;
                break;

            case Direction.Left:
                spawnRot = Settings.leftRotation;
                break;

            case Direction.Right:
                spawnRot = Settings.rightRotation;
                break;

            default:
                break;
        }

        if (vfx.startRotation.z != 0)
        {
            spawnRot *= vfx.startRotation;
        }

        return spawnRot;
    }

    public GameObject SpawnGOWithAnimation(GameObject go, Vector2 position, string animBoolName)
    {
        GameObject spawnGO = vfxSpawner.GetObjectFromPool(go);
        spawnGO.transform.position = position;
        spawnGO.GetComponent<Animator>().SetBool(animBoolName, true);
        return spawnGO;
    }

    public GameObject SpawnGOWithRotation(GameObject go, Vector2 position, Quaternion rotation)
    {
        GameObject spawnGO = vfxSpawner.GetObjectFromPool(go);
        spawnGO.transform.position = position;
        spawnGO.transform.rotation = rotation;
        return spawnGO;
    }
}

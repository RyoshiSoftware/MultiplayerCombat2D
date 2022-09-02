using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFX", menuName = "ScriptableObject/VFX")]
public class VFX : ScriptableObject
{
    [Header("Game Object")]
    public GameObject prefab;
    
    public bool isSprite;

    public float spawnDelay;

    [Header("Start position and rotation")]
    public Quaternion startRotation;
    public Vector3 offset;

    [Header("Position in 4 direction (not is sprite)")]
    public Vector2 upPos;
    public Vector2 downPos;
    public Vector2 leftPos;
    public Vector2 rightPos;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData 
{
    public GameObject prefab;
    public float velocity;
    public float liveTime;

    [Header("Attack")]
    public AttackDamageData attackDamageData;

    [Header("Feature")]
    public bool isHoming;

    [Header("Rotation")]
    public float spreadRotation;
    public float rotationMaxAngle;
    public float rotationSpeed;
}

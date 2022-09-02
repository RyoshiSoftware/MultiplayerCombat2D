using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackSpawnData
{
    [Header("Radius")]
    public float attackRadius;
    
    [Header("Position")]
    public Vector2 upPosition;
    public Vector2 downPosition;
    public Vector2 leftPosition;
    public Vector2 rightPosition;
}

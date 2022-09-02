using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "D_DashState", menuName = "ScriptableObject/Data/D_DashState")]
public class D_DashState : ScriptableObject
{
    
    [Header("Dash component")]
    public float dashVelocity;
    public float dashCoolDown;



    [Header("Attack component")]
    public float baseAttackDamage; // todo add damage
    public float knockBackStrength;
    public float toAttackMinValue; // Decide when to attack

    [Header("Destroy Gauge component")]
    public float attackBlockingGaugeDamage;

    [Header("VFX component")]
    public VFX dashVFX; 
    public VFX dashWindVFX; 
}

using UnityEngine;

[CreateAssetMenu(fileName = "D_RangedAttackState", menuName = "ScriptableObject/Data/D_RangedAttackState")]
public class D_RangeAttackState : ScriptableObject
{
    [Header("Attack component")]
    public AttackSpawnData attackSpawnData;
    public AttackDamageData damageData;
    public float fireRate;
    public float stopTimeAfterFire;


    [Header("Projectile component")]
    public ProjectileData projectileData;

    [Header("Vfx Component")]
    public VFX kiBlastVFX;
    public VFX explosionVFX;
    public VFX doubleBigExplosionVFX;
    public VFX holeVFX;
}

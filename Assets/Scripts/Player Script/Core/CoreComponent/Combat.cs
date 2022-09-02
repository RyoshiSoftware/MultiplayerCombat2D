using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable
{
    Player player;
    Movement movement;
    VFXController vfxController;
    BoxCollider2D col;

    public Action<Vector2, AttackDamageData> onTakeDamage;

    Vector2 attackPosition;
    Vector2 attackDirectionVector;
    Quaternion attackRotation;
    
    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
        col = GetComponent<BoxCollider2D>();
    }

    void Start() 
    {
        movement = core.GetCoreComponent<Movement>();
        vfxController = core.GetCoreComponent<VFXController>();
    }

    #region Deal Damage

    #region MeleeAttack

    public void MeleeAttack(AttackSpawnData attackSpawnData, AttackDamageData attackDamageData, int attackLayer)
    {
        GetAttackPosition(attackSpawnData);
        GetAttackDirection();

        CheckAndDealMeleeDamage(attackSpawnData, attackDamageData, attackLayer);
    }

    public Vector2 GetAttackPosition(AttackSpawnData attackData)
    {
        switch(player.playerDirection)
        {
            case Direction.Up:
                attackPosition = attackData.upPosition + (Vector2)player.transform.position;
                break;

            case Direction.Down:
                attackPosition = attackData.downPosition + (Vector2)player.transform.position;
                break;

            case Direction.Left:
                attackPosition = attackData.leftPosition + (Vector2)player.transform.position;
                break;

            case Direction.Right:
                attackPosition = attackData.rightPosition + (Vector2)player.transform.position;
                break;

            default:
                break;
        }

        return attackPosition;
    }

    void GetAttackDirection()
    {
        attackDirectionVector = player.playerDirectionVector;
    }

    void CheckAndDealMeleeDamage(AttackSpawnData attackSpawnData, AttackDamageData attackDamageData, int attackLayer)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(attackPosition, attackSpawnData.attackRadius);
        
        CheckDamageArea(collider2DArray, attackDamageData, attackLayer);
    }

    #endregion

    void CheckDamageArea(Collider2D[] collider2DArray, AttackDamageData attackDamageData, int attackLayer)
    {
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (IsTargetValid(collider2D, attackLayer, out IDamageable idamageable))
            {
                DealDamage(idamageable, attackDirectionVector, attackDamageData, attackLayer);
            }
        }
    }

    bool IsTargetValid(Collider2D collider2D, int attackLayer, out IDamageable idamageable)
    {
        idamageable = null;

        if (collider2D == col) 
        {
            return false;
        }

        idamageable = collider2D.gameObject.GetComponent<IDamageable>();

        if (idamageable == null) return false;

        if (attackLayer != 0 && attackLayer != idamageable.GetLayer()) return false;

        return true;
    }

    public void DealDamage(IDamageable idamageable, Vector2 attackDirection, AttackDamageData attackDamageData, int attackLayer)
    {
        idamageable.TakeDamage(attackDirectionVector, attackDamageData);
    }

    public void CheckDamageInBoxRange(AttackDamageData attackDamageData, Vector2 boxSize, int attackLayer)
    {
        GetAttackDirection();
        
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(transform.position, boxSize, 0);

        CheckDamageArea(collider2DArray, attackDamageData, attackLayer);
    }


    #region Range Attack

    public void RangeAttack(AttackSpawnData attackSpawnData, ProjectileData projectileData)
    {
        GetAttackPosition(attackSpawnData);
        GetAttackRotation(projectileData);

        SpawnProjectile(projectileData);
    }

    void SpawnProjectile(ProjectileData projectileData)
    {
        GameObject projectile = vfxController.SpawnGOWithRotation(projectileData.prefab, attackPosition, attackRotation);
        projectile.GetComponent<Projectile>().Initialize(projectileData, this, vfxController, player.target);
    }

    public void GetAttackRotation(ProjectileData projectileData)
    {
        switch (player.playerDirection)
        {
            case Direction.Up:
                attackRotation = Settings.upRotation;
                break;

            case Direction.Down:
                attackRotation = Settings.downRotation;
                break;

            case Direction.Left:
                attackRotation = Settings.leftRotation;
                break;

            case Direction.Right:
                attackRotation = Settings.rightRotation;
                break;

            default:
                break;
        }

        AddSpreadRotation(projectileData.spreadRotation);
    }

    void AddSpreadRotation(float spreadRotation)
    {
        attackRotation *= Quaternion.Euler(0, 0, UnityEngine.Random.Range(-spreadRotation, spreadRotation));
    }

    #endregion

    #endregion

    #region Take Damage

    public void TakeDamage(Vector2 attackDirecionVector, AttackDamageData attackDamageData)
    {
        onTakeDamage?.Invoke(attackDirecionVector, attackDamageData);

        if (player.IsInBlockState())
        {
            //TakeBlockDamage(attackDirecionVector, attackDamage, knockBackStrength);
            return;
        }
        
        KnockBack(attackDirecionVector, attackDamageData);

        SpawnHitVFX();
    }

    void KnockBack(Vector2 knockbackDirectionVector, AttackDamageData attackDamageData)
    {
        movement.AddForce(knockbackDirectionVector.normalized * attackDamageData.knockBackStrength, ForceMode2D.Impulse);
    }

    void SpawnHitVFX()
    {
        vfxController.SpawnDefaultVFX(vfxController.data.hitVFX);
    }

    #endregion

    public int GetLayer()
    {
        return player.gameObject.layer;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    ProjectileData projectileData;
    Rigidbody2D rb;
    Combat combat;
    VFXController vfxController;
    SpawnableObject vfxAnim; // Used for release gameobject;

    Transform target;
    bool targetInRange;


    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        vfxAnim = GetComponent<SpawnableObject>();
    }

    public void Initialize(ProjectileData projectileData, Combat combat, VFXController vfxController, Transform target = null)
    {
        this.projectileData = projectileData;
        this.combat = combat;
        this.vfxController = vfxController;
        this.target = target;
        
        SetGameObjectLayer();

        StartCoroutine(ExploseCoroutine(projectileData.liveTime));
    }

    void SetGameObjectLayer()
    {
        if (target != null)
        {
            gameObject.layer = target.gameObject.layer;
        }
        else
        {
            gameObject.layer = combat.GetLayer();
        }
    }

    IEnumerator ExploseCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ExploseWithoutHit();
    }

    void ExploseWithoutHit()
    {
        SpawnExplosionVFX();

        SpawnHoleVFX();

        vfxAnim.ReleaseObject();
    }

    private void FixedUpdate() 
    {
        Move();
        Homing();
    }

    void Move() 
    {
        rb.velocity = transform.right * projectileData.velocity * Time.deltaTime;
    }

    void Homing()
    {
        if (target != null)
        {
            float targetAngle = Vector2.SignedAngle(target.position - transform.position, transform.right);

            if (CheckingIfTargetInRange(targetAngle))
            {
                RotateProjectile(targetAngle);
            }
        }
    }

    bool CheckingIfTargetInRange(float angle)
    {
        return Mathf.Abs(angle) <= projectileData.rotationMaxAngle;
    }

    void RotateProjectile(float angle)
    {
        if (angle < 0)
        {
            transform.Rotate(0, 0, projectileData.rotationSpeed * Time.deltaTime);
        }
        else if (angle > 0)
        {
            transform.Rotate(0, 0, -projectileData.rotationSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (!IsTargetValid(other)) return;
        
        DealDamage(other);
    }

    bool IsTargetValid(Collider2D other)
    {
        if (other.gameObject == combat.gameObject) return false;

        IDamageable idamgeable = other.GetComponent<IDamageable>();

        if (idamgeable == null) return false;
        
        if (idamgeable.GetLayer() != gameObject.layer) return false;

        return true;
    }


    void DealDamage(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage(transform.right, projectileData.attackDamageData);

            Explose();
        }
    }

    void Explose()
    {
        SpawnExplosionVFX();

        vfxAnim.ReleaseObject();
    }

    void SpawnExplosionVFX()
    {
        vfxController.SpawnDefaultVFX(vfxController.data.explosionVFX, transform);
        vfxController.SpawnDefaultVFX(vfxController.data.doubleBigExplosionVFX, transform);
    }

    void SpawnHoleVFX()
    {
        vfxController.SpawnDefaultVFX(vfxController.data.holeVFX, transform);
    }
}

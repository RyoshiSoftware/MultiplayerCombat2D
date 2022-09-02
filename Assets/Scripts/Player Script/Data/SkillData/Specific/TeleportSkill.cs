using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[CreateAssetMenu(fileName = "TeleportSkill", menuName = "ScriptableObject/Skill/TeleportSkill")]
public class TeleportSkill : Skill 
{
    [Header("Teleport component")]
    public float teleportVelocity;
    public float teleportVelocityDecreaseFactor;

    [Header("VFX Component")]
    public VFX teleportVFX;

    public override void Initialize(Player player, Core core)
    {
        base.Initialize(player, core);

        animId = AnimationData.Teleport;
    }

    public override void Activate()
    {
        base.Activate();
        
        Timing.RunCoroutine(TeleportCoroutine(), Segment.FixedUpdate);
    }

    private IEnumerator<float> TeleportCoroutine()
    {
        SpawnVFX();
        
        BeforeTeleport();

        movement.SetVelocity(player.playerDirectionVector * teleportVelocity);

        while (movement.GetVelocityMagnitude() > 1f)
        {
            yield return Timing.WaitForOneFrame;

            movement.MultiplyVelocityFactor(teleportVelocityDecreaseFactor);
        }

        Deactivate();

        yield return Timing.WaitForOneFrame;

        AfterTeleportFinish();

        SpawnVFX();
    }

    public override void SpawnVFX()
    {
        vfxController.SpawnGOWithRotation(teleportVFX.prefab, player.transform.position, Quaternion.identity);
    }

    void BeforeTeleport()
    {
        player.DisableCollider();
        player.sprite.enabled = false;
    }

    void AfterTeleportFinish()
    {
        player.EnableCollider();
        player.sprite.enabled = true;

        movement.StopMoving();
    }

    public override void LogicsUpdate()
    {
        
    }
}
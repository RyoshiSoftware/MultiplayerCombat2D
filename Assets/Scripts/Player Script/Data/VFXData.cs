using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFXData", menuName = "ScriptableObject/Data/VFXData")]
public class VFXData : ScriptableObject
{
    [Header("Block")]
    public VFX doubleShockWaveVFX;
    public VFX blockEplosionVFX;
    public VFX blockKiBlastVFX;

    [Header("Dash")]
    public VFX dashVFX;
    public VFX dashWindVFX;

    [Header("Hit")]
    public VFX hitVFX;
    
    [Header("Knock Back")]
    public VFX knockbackWindVFX;
    public VFX mediumKnockbackVFX;
    public VFX strongKnockbackVFX;

    [Header("Melee Attack")]
    public VFX punchVFX;
    public VFX kickVFX;


    [Header("Ranged Attack")]
    public VFX kiBlastVFX;
    public VFX explosionVFX;
    public VFX doubleBigExplosionVFX;
    public VFX holeVFX;

    [Header("Super Flight")]
    public VFX airDoubleVFX;
}

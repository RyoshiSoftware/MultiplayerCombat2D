using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "D_KnockbackState", menuName = "ScriptableObject/Data/D_KnockbackState")]
public class D_KnockbackState : ScriptableObject
{
    public float mediumKnockBackRecoverTime;
    public float strongKnockBackRecoverTime;
    public float velocityDecreaseFactor;

    public float spawnWindTime;
}

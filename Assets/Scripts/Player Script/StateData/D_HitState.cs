using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "D_HitState", menuName = "ScriptableObject/Data/D_HitState")]
public class D_HitState : ScriptableObject
{
    public float hitRecoverTime;

    public float spawnWindTime;
}

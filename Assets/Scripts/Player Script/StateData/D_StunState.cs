using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "D_StunState", menuName = "ScriptableObject/Data/D_StunState")]
public class D_StunState : ScriptableObject
{
    public float stunTime;
    public Material flashMaterial;
}

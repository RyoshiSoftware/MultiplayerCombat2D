using UnityEngine;

[CreateAssetMenu(fileName = "D_SuperFlightState", menuName = "ScriptableObject/Data/D_SuperFlightState")]
public class D_SuperFlightState : D_MoveState
{
    [Header("VFX component")]
    public VFX airDoubleVFX;
    public float vfxTimeElapse = 3f;
}

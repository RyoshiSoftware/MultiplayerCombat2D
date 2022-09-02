using UnityEngine;

[CreateAssetMenu(fileName = "D_FlyState", menuName = "ScriptableObject/Data/D_FlyState")]
public class D_FlyState : ScriptableObject
{
    public AnimatorOverrideController flyAnimatorOverrideController;
    public float flySpeed;
    public float maxFlyHeight;
    public float hoverSpeed;
    public float maxHoverHeight;
}

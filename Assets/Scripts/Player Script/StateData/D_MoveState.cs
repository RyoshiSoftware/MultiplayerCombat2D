using UnityEngine;

[CreateAssetMenu(fileName = "D_MoveState", menuName = "ScriptableObject/Data/D_MoveState")]
public class D_MoveState : ScriptableObject
{
    [Header("Move component")]
    public float movementSpeed;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    void TakeDamage(Vector2 attackDirectionVector, AttackDamageData attackDamageData);
    int GetLayer();
}

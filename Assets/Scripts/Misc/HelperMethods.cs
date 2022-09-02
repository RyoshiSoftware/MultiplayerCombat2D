using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public static class HelperMethods 
{
    #region Direction

    public static Direction GetDirectionFromVector(float x, float y)
    {
        Direction direction = Direction.Up;

        if (Mathf.Abs(y) >= Mathf.Abs(x))
        {
            if (y > 0)
            {
                direction = Direction.Up;
            }
            else if (y < 0)
            {
                direction = Direction.Down;
            }
            else if (x > 0)
            {
                direction = Direction.Right;
            }
            else if (x < 0)
            {
                direction = Direction.Left;
            }
        }
        else
        {
            if (x > 0)
            {
                direction = Direction.Right;
            }
            else if (x < 0)
            {
                direction = Direction.Left;
            }
            else if (y > 0)
            {
                direction = Direction.Up;
            }
            else if (y < 0)
            {
                direction = Direction.Down;
            }
        }

        return direction;
    }

    public static Direction GetReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Left:
                return Direction.Right;
            case Direction.Down:
                return Direction.Up;
            case Direction.Right:
                return Direction.Left;
        }

        return Direction.Null;
    } 

    #endregion

    #region Quaternion Method


    #endregion

    public static KnockBackType GetKnockBackType(float knockBackStrength)
    {
        if (knockBackStrength >= Settings.minStrongKnockBack)
        {
            return KnockBackType.strong;
        }
        else if (knockBackStrength >= Settings.minMediumKnockBack)
        {
            return KnockBackType.medium;
        }

        return KnockBackType.weak;
    }

    #region Attack

    public static void Attack(Collision2D other, Vector2 attackDirection, float attackDamage, float blockingGaugeDamage, float knockBackStrength)
    {
        IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
        if (idamageable != null)
        {
            // idamageable.TakeDamage(attackDirection, attackDamage, knockBackStrength);
            // idamageable.TakeBlockingGaugeDamage(blockingGaugeDamage);
        }
    }

    public static void Attack(Collider2D other, Vector2 attackDirection, float attackDamage, float blockingGaugeDamage, float knockBackStrength)
    {
        IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
        if (idamageable != null)
        {
            // idamageable.TakeDamage(attackDirection, attackDamage, knockBackStrength);
            // idamageable.TakeBlockingGaugeDamage(blockingGaugeDamage);
        }
    }

    #endregion
}

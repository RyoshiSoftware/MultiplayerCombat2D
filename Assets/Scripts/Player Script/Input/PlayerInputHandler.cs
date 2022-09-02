using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 movementInput {get; protected set;}
    public bool auraInput {get; protected set;}
    public bool blockInput {get; protected set;}
    public bool dashInput {get; protected set;}
    public bool flyInput {get; protected set;}
    public bool meleeAttackInput {get; protected set;}
    public bool rangeAttackInput {get; protected set;}

    public Action onLockOn;

    public Action<int> onSkillInputChange;
    public bool skillInput {get; protected set;}

    public void ResetInput()
    {
        blockInput = false;
        dashInput = false;
        meleeAttackInput = false;
        rangeAttackInput = false;

        ResetSkillInput();
    }

    void ResetSkillInput()
    {   
        skillInput = false;
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAuraInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            auraInput = true;
        }

        if (context.canceled)
        {
            auraInput = false;
        }
    }

    public void UseAuraInput()
    {
        auraInput = false;
    }

    public void OnBlockInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            blockInput = true;
        }

        if (context.canceled)
        {
            blockInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            dashInput = true;
        }
        
    }

    public void UseDash()
    {
        dashInput = false;
    }

    public void OnFlyInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            flyInput = true;
        }
    }

    public void UseFlyInput()
    {
        flyInput = false;
    }

    public void OnLockOnInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onLockOn?.Invoke();
        }
    }

    public void OnMeleeAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            meleeAttackInput = true;
        }
    }

    public void UseMeleeAttack()
    {
        meleeAttackInput = false;
    }

    public void OnRangeAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rangeAttackInput = true;
        }
        
        if (context.canceled)
        {
            rangeAttackInput = false;
        }
    }

    void OnSkillInput(InputAction.CallbackContext context, int index)
    {
        onSkillInputChange?.Invoke(index);

        if (context.started)
        {
            skillInput = true;
        }

        if (context.canceled)
        {
            skillInput = false;
        }
    }

    public void OnSkill0Input(InputAction.CallbackContext context) 
    {
        OnSkillInput(context, 0);
    }

    public void OnSkill1Input(InputAction.CallbackContext context) 
    {
        OnSkillInput(context, 1);
    }
}

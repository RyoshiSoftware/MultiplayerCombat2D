using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingInput : PlayerInputHandler
{
    void Update() 
    {
        OnMovementInput();
        OnBlockInput();
        OnMeleeAttackInput();
        OnRangeAttackInput();
        OnDashInput();
        OnSkill0Input();
        OnSkill1Input();
        OnFlyInput();
    }

    private void OnFlyInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            flyInput = true;
        }
    }

    public void OnMovementInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal1"), Input.GetAxisRaw("Vertical1"));
    }

    public void OnBlockInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            blockInput = true;
        }

        if (Input.GetKeyUp(KeyCode.Keypad0))
        {
            blockInput = false;
        }
    }

    public void OnDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            dashInput = true;
        }
    }

    public void OnMeleeAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            meleeAttackInput = true;
        }
    }

    public void OnRangeAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            rangeAttackInput = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            rangeAttackInput = false;
        }
    }

    void OnSkillInput(int index)
    {
        onSkillInputChange?.Invoke(index);
        skillInput = true;
    }

    void OnSkillRelease()
    {
        skillInput = false;
    }

    public void OnSkill0Input() 
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            OnSkillInput(0);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad7))
        {
            OnSkillRelease();
        }
    }

    public void OnSkill1Input() 
    {
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            OnSkillInput(1);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad8))
        {
            OnSkillRelease();
        }
    }
}

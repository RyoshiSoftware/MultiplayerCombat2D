using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState;
    public bool canChangeState {get; private set;}

    public void Initialize(PlayerState firstState)
    {
        currentState = firstState;
        currentState.Enter();

        canChangeState = true;
    }
    
    public void ChangeState(PlayerState newState) 
    {
        if (currentState == newState)
        {
            currentState.ResetState();
            return;
        }
        
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void EnableChangeState()
    {
        canChangeState = true;
    }

    public void DisableChangeState()
    {
        canChangeState = false;
    }
}

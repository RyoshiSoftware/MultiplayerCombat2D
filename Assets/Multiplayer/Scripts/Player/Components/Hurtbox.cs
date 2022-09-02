using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private string meleeAttackingState = "MeleeAttackingState";
    private string movingState = "MovingState";
    private string targettingState = "TargettingState";
    private string rangedAttackingState = "RangedAttackingState";
    private string blockingState = "BlockingState";
    private string SpiritBurstingState = "SpiritBurstingState";
    private string hitBoxUp = "HitboxUp";
    private string hitBoxDown = "HitboxDown";
    private string hitBoxLeft = "HitboxLeft";
    private string hitBoxRight = "HitboxRight";


    private string damageTakenState = "DamageTakenState";

    private int meleeAttackingStateHash;
    private int movingStateHash;
    private int targettingStateHash;
    private int rangedAttackingStateHash;
    private int blockingStateHash;
    private int spiritBurstingStateHash;

    private int damageTakenStateHash;

    private void Start()
    {
        meleeAttackingStateHash = meleeAttackingState.GetHashCode();
        movingStateHash = movingState.GetHashCode();
        targettingStateHash = targettingState.GetHashCode();
        rangedAttackingStateHash = rangedAttackingState.GetHashCode();
        damageTakenStateHash = damageTakenState.GetHashCode();
        blockingStateHash = blockingState.GetHashCode();
        spiritBurstingStateHash = SpiritBurstingState.GetHashCode();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        #region NoteToSelf
        
        //Remember to add a layer check here

        #endregion
        
        #region CheckIfBlocked
        if (collider.name == hitBoxUp && playerController.blockingState.blockingIndex == 2)
        {
            return;
        }

        if (collider.name == hitBoxDown && playerController.blockingState.blockingIndex == 1)
        {
            return;
        }

        if (collider.name == hitBoxRight && playerController.blockingState.blockingIndex == 3)
        {
            return;
        }

        if (collider.name == hitBoxLeft && playerController.blockingState.blockingIndex == 4)
        {
            return;
        }
        #endregion

        #region DealWithDamage - This needs to be done over properly once an implementation of health and damage are in play
        if (collider.tag == "HitBox")
        {
            if (playerController.meleeAttackingState.isCurrentState) 
            {
                playerController.SwitchState(meleeAttackingStateHash, damageTakenStateHash);
            }
            else if (playerController.movingState.isCurrentState)
            {
                playerController.SwitchState(movingStateHash, damageTakenStateHash);
            }
            else if (playerController.targettingState.isCurrentState)
            {
                playerController.SwitchState(targettingStateHash, damageTakenStateHash);
            }
            else if (playerController.rangedAttackingState.isCurrentState)
            {
                playerController.SwitchState(rangedAttackingStateHash, damageTakenStateHash);
            }
            else if (playerController.blockingState.isCurrentState)
            {
                playerController.SwitchState(blockingStateHash, damageTakenStateHash);
            }
            else if (playerController.spiritBurstingState.isCurrentState)
            {
                playerController.SwitchState(spiritBurstingStateHash, damageTakenStateHash);
            }
        }
        #endregion
    }
}

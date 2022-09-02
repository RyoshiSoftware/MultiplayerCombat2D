using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class SpiritBurstingState : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        private Animator animator;

        private float lastHorizontal;
        private float lastVertical;

        public bool isCurrentState;
        public bool wasTargetting;

        private string thisState = "SpiritBurstingState";
        private string movingState = "MovingState";
        private string targettingState = "TargettingState";

        private readonly int chargingHash = Animator.StringToHash("charging");
        private readonly int movingHash = Animator.StringToHash("moving");
        private readonly int lastHorizontalHash = Animator.StringToHash("lasthorizontal");
        private readonly int lastVerticalHash = Animator.StringToHash("lastvertical");

        private int thisStateHash;
        private int nextStateHash;
        private int movingStateHash;
        private int targettingStateHash;

        [ClientCallback]
        private void Start()
        {
            animator = playerController.animator;

            thisStateHash = thisState.GetHashCode();
            movingStateHash = movingState.GetHashCode();
            targettingStateHash = targettingState.GetHashCode();
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) { return; }

            if (playerController.inputManager.SpiritBurstReleasedThisFrame())
            {
                EndSpiritBurst();
            }
        }

        public void SpiritBursting()
        {
            if (!hasAuthority) { return; }

            animator.SetBool(movingHash, false);

            if (wasTargetting)
            {
                lastHorizontal = playerController.targettingState.lastHorizontal;
                lastVertical = playerController.targettingState.lastVertical;
            }
            else 
            {
                lastHorizontal = playerController.movingState.lastHorizontal;
                lastVertical = playerController.movingState.lastVertical;
            }

            animator.SetFloat(lastHorizontalHash, lastHorizontal);
            animator.SetFloat(lastVerticalHash, lastVertical);

            animator.SetBool(chargingHash, true);
        }

        private void EndSpiritBurst()
        {
            animator.SetBool(chargingHash, false);
            animator.SetBool(movingHash, true);

            if (wasTargetting)
            {
                wasTargetting = false;
                nextStateHash = targettingStateHash;
            }
            else
            {
                nextStateHash = movingStateHash;
            }

            playerController.SwitchState(thisStateHash, nextStateHash);
        }
    }
}


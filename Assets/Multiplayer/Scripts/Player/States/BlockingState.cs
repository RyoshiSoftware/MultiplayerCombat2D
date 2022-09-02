using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class BlockingState : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        private Animator animator;

        private float lastHorizontal;
        private float lastVertical;

        public bool isCurrentState;
        public bool wasTargetting;

        private string thisState = "BlockingState";
        private string movingState = "MovingState";
        private string targettingState = "TargettingState";

        private readonly int blockingHash = Animator.StringToHash("blocking");
        private readonly int movingHash = Animator.StringToHash("moving");
        private readonly int lastHorizontalHash = Animator.StringToHash("lasthorizontal");
        private readonly int lastVerticalHash = Animator.StringToHash("lastvertical");
        
        private int thisStateHash;
        private int nextStateHash;
        private int movingStateHash;
        private int targettingStateHash;

        public int blockingIndex;

        #region Client

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

            if (playerController.inputManager.BlockReleasedThisFrame())
            {
                EndBlock();
            }
        }

        public void Blocking()
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

            animator.SetBool(blockingHash, true);
        }

        private void SetBlockUp()
        {
            blockingIndex = 1;
        }
        
        private void SetBlockDown()
        {
            blockingIndex = 2;
        }

        private void SetBlockLeft()
        {
            blockingIndex = 3;
        }

        private void SetBlockRight()
        {
            blockingIndex = 4;
        }

        private void EndBlock()
        {
            blockingIndex = 0;

            animator.SetBool(blockingHash, false);
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

        #endregion
    }
}


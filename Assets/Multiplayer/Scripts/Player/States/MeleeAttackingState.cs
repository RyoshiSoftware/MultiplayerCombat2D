using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D

{
    public class MeleeAttackingState : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        private Animator animator;

        private float lastHorizontal;
        private float lastVertical;

        public bool isCurrentState;
        public bool wasTargetting;
        private bool canCombo;


        private string thisState = "MeleeAttackingState";
        private string nextState;
        private string movingState = "MovingState";
        private string targettingState = "TargettingState";

        private readonly int meleeattackingHash = Animator.StringToHash("meleeattacking");
        private readonly int meleeAttackHash = Animator.StringToHash("MeleeAttack");
        private readonly int meleeattack2Hash = Animator.StringToHash("MeleeAttack2");
        private readonly int meleeattack3Hash = Animator.StringToHash("MeleeAttack3");
        private readonly int locomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int movingHash = Animator.StringToHash("moving");
        private readonly int lastHorizontalHash = Animator.StringToHash("lasthorizontal");
        private readonly int lastVerticalHash = Animator.StringToHash("lastvertical");

        private int thisStateHash;
        private int nextStateHash;
        private int movingStateHash;
        private int targettingStateHash;
        public int attackIndex;


        #region Server

        [Command]
        private void CmdEnableHitBox(int hitBoxIndex)
        {
            RpcEnableHitbox(hitBoxIndex);
        }

        #endregion

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
            if (!isCurrentState) { return; }

            if (playerController.inputManager.MeleeAttackPressedThisFrame())
            {
                if (canCombo && attackIndex < 4)
                {
                    MeleeAttacking();
                }
            }

            if (!canCombo)
            {
                StartCoroutine(EndMeleeAttack());
            }
        }

        public void MeleeAttacking()
        {
            canCombo = true;
            
            StartCoroutine(ComboTimer());

            animator.SetBool(movingHash, false);

            if (attackIndex == 1)
            {
                animator.Play(meleeAttackHash);
            }
            else if (attackIndex == 2)
            {
                animator.Play(meleeattack2Hash);
            }
            else if (attackIndex == 3)
            {
                animator.Play(meleeattack3Hash);
            }

            attackIndex ++;

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

            if (lastVertical > 0)
            {
                CmdEnableHitBox(0);
            }
            else if (lastVertical < 0)
            {
                CmdEnableHitBox(1);
            }
            else if (lastHorizontal < 0)
            {
                CmdEnableHitBox(2);
            }
            else if (lastHorizontal > 0)
            {
                CmdEnableHitBox(3);
            }
        }

        private IEnumerator ComboTimer()
        {
            yield return new WaitForSeconds(0.7f);

            canCombo = false;
        }

        private IEnumerator EndMeleeAttack()
        {
            foreach(Collider2D hitbox in playerController.hitBoxes)
            {
                hitbox.enabled = false;
            }

            yield return new WaitForSeconds(0.2f);

            animator.SetBool(movingHash, true);

            animator.Play(locomotionBlendTreeHash);

            nextStateHash = movingStateHash;

            if(wasTargetting == true && playerController.targettingState.hasTarget)
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

        [ClientRpc]
        private void RpcEnableHitbox(int hitBoxIndex)
        {
            playerController.hitBoxes[hitBoxIndex].enabled = true;
        }
    }
}

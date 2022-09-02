using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class FlightDashingState : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        private Animator animator;

        private Vector2 motionVector;

        private float horizontal;
        private float vertical;
        private float speed = 10f;

        public bool isCurrentState;
        private bool dashing;

        private string thisState = "FlightDashingState";
        private string movingState = "MovingState";

        private readonly int horizontalHash = Animator.StringToHash("horizontal");
        private readonly int verticalHash = Animator.StringToHash("vertical");
        private readonly int dashingHash = Animator.StringToHash("dashing");
        private readonly int flyingHash = Animator.StringToHash("flying");

        private int thisStateHash;
        private int movingStateHash;


        private void Start()
        {
            animator = playerController.animator;

            thisStateHash = thisState.GetHashCode();
            movingStateHash = movingState.GetHashCode();
        }

        private void Update()
        {
            if (!hasAuthority) { return; }
            if (!isCurrentState) { return; }

            Dashing();

            if (playerController.inputManager.DashReleasedThisFrame() || motionVector == new Vector2(0, 0))
            {
                EndDash();
            }
        }

        private void FixedUpdate()
        {
            if (!hasAuthority) { return; }
            if (!isCurrentState) { return; }

            Dash();
        }

        private void Dashing()
        {
            if (!playerController.aura.activeInHierarchy) 
            {
                playerController.CmdSetAuraActive(true);
            }

            horizontal = playerController.inputManager.GetPlayerMovement().x;
            vertical = playerController.inputManager.GetPlayerMovement().y;

            motionVector = new Vector2(horizontal, vertical);
            animator.SetFloat(horizontalHash, horizontal);
            animator.SetFloat(verticalHash, vertical);

            dashing = horizontal != 0 || vertical != 0;

            animator.SetBool(dashingHash, true);
        }

        private void Dash()
        {
            if (isLocalPlayer)
            {
                playerController.rigidBody2D.velocity = motionVector * speed;
            }
        }

        private void EndDash()
        {
            playerController.CmdSetAuraActive(false);
            animator.SetBool(dashingHash, false);
            animator.SetBool(flyingHash, true);
            playerController.SwitchState(thisStateHash, movingStateHash);
        }
    }
}

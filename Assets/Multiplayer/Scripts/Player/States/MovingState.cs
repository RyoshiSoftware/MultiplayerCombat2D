using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class MovingState : NetworkBehaviour
    {
        [Header("Serialized Fields")]
        [SerializeField] private PlayerController playerController;

        [Header("Params")]
        private Rigidbody2D rigidBody2D;
        private Animator animator;
        private InputManager inputManager;

        private Vector2 motionVector;

        private float horizontal;
        private float vertical;
        public float lastHorizontal { get; private set; }
        public float lastVertical { get; private set; }
        private float speed;
        private float bobStep = 0.01f; 
        private float bobRange = 0.5f;


        public bool isCurrentState;
        private bool moving;
        private bool bobCap;

        private string thisState = "MovingState";
        private string meleeAttackingState = "MeleeAttackingState";
        private string rangedAttackingState = "RangedAttackingState";
        private string targettingState = "TargettingState";
        private string blockingState = "BlockingState";
        private string spiritBurstingState = "SpiritBurstingState";
        private string flightDashingState = "FlightDashingState";
        private string emergencyTeleportingState = "EmergencyTeleportingState";

        [Header("Hashes")]
        private readonly int locomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int idleBlendTreeHash = Animator.StringToHash("Idle");
        private readonly int horizontalHash = Animator.StringToHash("horizontal");
        private readonly int verticalHash = Animator.StringToHash("vertical");
        private readonly int lastHorizontalHash = Animator.StringToHash("lasthorizontal");
        private readonly int lastVerticalHash = Animator.StringToHash("lastvertical");
        private readonly int movingHash = Animator.StringToHash("moving");
        private readonly int flyingHash = Animator.StringToHash("flying");

        private int thisStateHash;
        private int nextStateHash;
        private int meleeAttackStateHash;
        private int rangedAttackingStateHash;
        private int targettingStateHash;
        private int blockingStateHash;
        private int spiritBurstingStateHash;
        private int flightDashingStateHash;
        private int emergencyTeleportingStateHash;



        [ClientCallback]
        private void Start()
        {
            rigidBody2D = playerController.rigidBody2D;
            animator = playerController.animator;
            inputManager = playerController.inputManager;
            speed = playerController.speed;

            thisStateHash = thisState.GetHashCode();
            meleeAttackStateHash = meleeAttackingState.GetHashCode();
            rangedAttackingStateHash = rangedAttackingState.GetHashCode();
            targettingStateHash = targettingState.GetHashCode();
            blockingStateHash = blockingState.GetHashCode();
            spiritBurstingStateHash = spiritBurstingState.GetHashCode();
            flightDashingStateHash = flightDashingState.GetHashCode();
            emergencyTeleportingStateHash = emergencyTeleportingState.GetHashCode();
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) { return; }
            if (!isCurrentState) { return; }

            Moving();
            Idling();

            if (inputManager.MeleeAttackPressedThisFrame())
            {
                rigidBody2D.velocity = new Vector2(0f, 0f);
                animator.SetBool(movingHash, false);
                nextStateHash = meleeAttackStateHash;
                CallStateSwitch();
            }

            if (inputManager.RangedAttackPressedThisFrame())
            {
                rigidBody2D.velocity = new Vector2(0f, 0f);
                animator.SetBool(movingHash, false);
                nextStateHash = rangedAttackingStateHash;
                CallStateSwitch();
            }

            if (inputManager.TargetPressedThisFrame() && playerController.targettingState.hasTarget)
            {
                nextStateHash = targettingStateHash;
                CallStateSwitch();
            }

            if (inputManager.BlockPressedThisFrame())
            {
                rigidBody2D.velocity = new Vector2(0f, 0f);
                animator.SetBool(movingHash, false);
                nextStateHash = blockingStateHash;
                CallStateSwitch();
            }

            if (inputManager.SpiritBurstPressedThisFrame())
            {
                rigidBody2D.velocity = new Vector2(0f, 0f);
                animator.SetBool(movingHash, false);
                nextStateHash = spiritBurstingStateHash;
                CallStateSwitch();
            }

            if (inputManager.FlyPressedThisFrame())
            {
                if (!playerController.isFlying)
                {
                    animator.SetBool(flyingHash, true);
                    playerController.SwitchToFlight();
                }
                else if (playerController.isFlying)
                {
                    animator.SetBool(flyingHash, false);
                    playerController.SwitchToGrounded();
                }
            }

            if (inputManager.DashPressedThisFrame() && playerController.isFlying)
            {
                animator.SetBool(flyingHash, false);
                nextStateHash = flightDashingStateHash;
                CallStateSwitch();
            }

            if (inputManager.TeleportPressedThisFrame())
            {
                if (!playerController.emergencyTeleportingState.canTeleport) { return; }

                nextStateHash = emergencyTeleportingStateHash;
                CallStateSwitch();
            }
        }

        [ClientCallback]
        private void FixedUpdate()
        {
            if (!hasAuthority) { return; }
            if (!isCurrentState) { return; }

            if (playerController.isFlying)
            {
                if (!bobCap)
                {
                    transform.position = Vector2.MoveTowards(
                        transform.position,
                        new Vector2(transform.position.x, transform.position.y + bobRange),
                        bobStep);

                    bobRange = bobRange + 0.1f;

                    if (bobRange >= 2) { bobCap = true; }
                }
                else 
                {
                    transform.position = Vector2.MoveTowards(
                        transform.position,
                        new Vector2(transform.position.x, transform.position.y - bobRange),
                        bobStep);

                    bobRange = bobRange - 0.1f;

                    if (bobRange <= 1) { bobCap = false; }
                }
            }

            Move();
        }

        private void Moving()
        {
            horizontal = inputManager.GetPlayerMovement().x;
            vertical = inputManager.GetPlayerMovement().y;

            motionVector = new Vector2(horizontal, vertical);
            animator.SetFloat(horizontalHash, horizontal);
            animator.SetFloat(verticalHash, vertical);

            moving = horizontal != 0 || vertical != 0;

            if (!playerController.isFlying)
            {
                animator.SetBool(movingHash, moving);
            }
        }

        private void Idling()
        {
            if (!playerController.isFlying)
            {
                animator.SetBool(movingHash, moving);
            }
            
            if (horizontal != 0 || vertical != 0)
            {
                lastHorizontal = horizontal;
                lastVertical = vertical;
                animator.SetFloat(lastHorizontalHash, horizontal);
                animator.SetFloat(lastVerticalHash, vertical);
            }
        }

        private void Move()
        {
            if (isLocalPlayer)
            {
                rigidBody2D.velocity = motionVector * speed;
            }
        }

        private void CallStateSwitch()
        {
            playerController.SwitchState(thisStateHash, nextStateHash);
        }
    }
}

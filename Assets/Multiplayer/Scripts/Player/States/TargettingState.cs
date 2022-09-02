using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class TargettingState : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [field: SerializeField] public GameObject reticle { get; private set; }
        [SerializeField] private TargetZone targetZone;

        private Rigidbody2D rigidBody2D;
        private Animator animator;
        private InputManager inputManager;

        private Vector2 motionVector;
        public Vector2 attackTranslation;

        private float horizontal;
        private float vertical;
        public float lastHorizontal { get; private set; }
        public float lastVertical { get; private set; }
        private float speed;
        private float bobStep = 0.01f;
        private float bobRange = 0.5f;

        public bool isCurrentState;
        public bool hasTarget;
        private bool moving;
        private bool bobCap;

        private string thisState = "TargettingState";
        private string movingState = "MovingState";
        private string meleeAttackingState = "MeleeAttackingState";
        private string rangedAttackingState = "RangedAttackingState";
        private string blockingState = "BlockingState";
        private string spiritBurstingState = "SpiritBurstingState";

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
        private int movingStateHash;
        private int meleeAttackingStateHash;
        private int rangedAttackingStateHash;
        private int blockingStateHash;
        private int spiritBurstingStateHash;



        [ClientCallback]
        private void Start()
        {
            rigidBody2D = playerController.rigidBody2D;
            animator = playerController.animator;
            inputManager = playerController.inputManager;
            speed = playerController.targettingSpeed;

            thisStateHash = thisState.GetHashCode();
            movingStateHash = movingState.GetHashCode();
            meleeAttackingStateHash = meleeAttackingState.GetHashCode();
            rangedAttackingStateHash = rangedAttackingState.GetHashCode();
            blockingStateHash = blockingState.GetHashCode();
            spiritBurstingStateHash = spiritBurstingState.GetHashCode();
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) { return; }
            if (!isCurrentState) { return; }

            if (!reticle.activeInHierarchy && hasTarget)
            {
                reticle.SetActive(true);
            }

            Moving();
            Idling();  

            if (inputManager.CancelPressedThisFrame())
            {
                nextStateHash = movingStateHash;
                CallStateSwitch();
            }          

            if (inputManager.MeleeAttackPressedThisFrame())
            {
                attackTranslation = new Vector2(
                    reticle.transform.position.x - transform.position.x, 
                    reticle.transform.position.y - transform.position.y);

                lastHorizontal = attackTranslation.x;
                lastVertical = attackTranslation.y;

                rigidBody2D.velocity = new Vector2(0f, 0f);
                animator.SetBool(movingHash, false);
                nextStateHash = meleeAttackingStateHash;
                CallStateSwitch();
            }

            if (inputManager.RangedAttackPressedThisFrame())
            {
                // attackTranslation = new Vector2(
                //     reticle.transform.position.x - transform.position.x,
                //     reticle.transform.position.y - transform.position.y);

                //     lastHorizontal = reticle.transform.position.x;
                //     lastVertical = attackTranslation.y;

                    rigidBody2D.velocity = new Vector2(0f, 0f);
                    animator.SetBool(movingHash, false);
                    nextStateHash = rangedAttackingStateHash;
                    CallStateSwitch();
            }

            if (inputManager.BlockPressedThisFrame())
            {
                attackTranslation = new Vector2(
                    reticle.transform.position.x - transform.position.x,
                    reticle.transform.position.y - transform.position.y);

                    lastHorizontal = attackTranslation.x;
                    lastVertical = attackTranslation.y;

                    rigidBody2D.velocity = new Vector2(0f, 0f);
                    animator.SetBool(movingHash, false);
                    nextStateHash = blockingStateHash;
                    CallStateSwitch();
            }

            if (inputManager.SpiritBurstPressedThisFrame())
            {
                attackTranslation = new Vector2(
                    reticle.transform.position.x - transform.position.x,
                    reticle.transform.position.y - transform.position.y);

                    lastHorizontal = attackTranslation.x;
                    lastVertical = attackTranslation.y;

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
        }

        [ClientCallback]
        private void FixedUpdate()
        {
            if (!hasAuthority) { return; }
            if(!isCurrentState) { return; }

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

            animator.SetBool(movingHash, moving);
        }

        private void Idling()
        {
            animator.SetBool(movingHash, moving);

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
            if (nextStateHash == meleeAttackingStateHash)
            {
                playerController.meleeAttackingState.wasTargetting = true;
            }
            else if (nextStateHash == rangedAttackingStateHash)
            {
                playerController.rangedAttackingState.wasTargetting = true;
            }
            else if (nextStateHash == blockingStateHash)
            {
                playerController.blockingState.wasTargetting = true;
            }
            else if (nextStateHash == spiritBurstingStateHash)
            {
                playerController.spiritBurstingState.wasTargetting = true;
            }
            else 
            {
                reticle.SetActive(false);
            }
            
            playerController.SwitchState(thisStateHash, nextStateHash);
        }
    }
}
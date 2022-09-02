using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using System.Linq;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class PlayerController : NetworkBehaviour
    {
        [field: SerializeField] public float speed { get; private set; }
        [field: SerializeField] public float targettingSpeed { get; private set;}
        [field: SerializeField] public InputManager inputManager { get; private set; }
        [SerializeField] private GameObject hitBoxGroup;
        [SerializeField] private GameObject hurtBox;
        [field: SerializeField] public GameObject aura { get; private set; }
        [SerializeField] private Transform projectileSpawnLocationGroup;
        [SerializeField] private Transform flightMarker;
        [SerializeField] private Transform groundMarker;

        public Rigidbody2D rigidBody2D { get; private set; }
        public Animator animator { get; private set; }
        public MovingState movingState { get; private set; }
        public MeleeAttackingState meleeAttackingState { get; private set; }
        public RangedAttackingState rangedAttackingState { get; private set;}
        public DamageTakenState damageTakenState { get; private set; }
        public TargettingState targettingState { get; private set; }
        public BlockingState blockingState { get; private set; }
        public SpiritBurstingState spiritBurstingState { get; private set; }
        public FlightDashingState flightDashingState { get; private set; }
        public EmergencyTeleportingState emergencyTeleportingState { get; private set; }
        private Collider2D hurtBoxTrigger;
        public List <Collider2D> hitBoxes;
        public List <Transform> projectileSpawnLocations;

        public bool moving;
        public bool isFlying;

        private string movingStateString = "MovingState";
        private string meleeAttackingStateString = "MeleeAttackingState";
        private string rangedAttackingStateString = "RangedAttackingState";
        private string damageTakenStateString = "DamageTakenState";
        private string targettingStateString = "TargettingState";
        private string blockingStateString = "BlockingState";
        private string spiritBurstingStateString = "SpiritBurstingState";
        private string flightDashingStateString = "FlightDashingState";
        private string emergencyTeleportingStateString = "EmergencyTeleportingState";

        private int movingStateHash;
        private int meleeAttackingStateHash;
        private int rangedAttackingStateHash;
        private int damageTakenStateHash;
        private int targettingStateHash;
        private int blockingStateHash;
        private int spiritBurstingStateHash;
        private int flightDashingStateHash;
        private int emergencyTeleportingStateHash;

        private Vector2 currentTargetPos;

        #region Server

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
        }

        [Command]
        private void CmdSwitchLayer(string currentLayer)
        {
            if (currentLayer == "Grounded")
            {
                RpcSwitchLayer("Flying");
            }
            else if (currentLayer == "Flying")
            {
                RpcSwitchLayer("Grounded");
            }
        }

        [Command]
        public void CmdSetAuraActive(bool isEnabled)
        {
            RpcSetAuraActive(isEnabled);
        }


        #endregion

        #region Client

        [ClientCallback]
        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            movingState = GetComponent<MovingState>();
            meleeAttackingState = GetComponent<MeleeAttackingState>();
            rangedAttackingState = GetComponent<RangedAttackingState>();
            damageTakenState = GetComponent<DamageTakenState>();
            targettingState = GetComponent<TargettingState>();
            blockingState = GetComponent<BlockingState>();
            spiritBurstingState = GetComponent<SpiritBurstingState>();
            flightDashingState = GetComponent<FlightDashingState>();
            emergencyTeleportingState = GetComponent<EmergencyTeleportingState>();
        }

        [ClientCallback]
        private void Start()
        {
            hitBoxes = hitBoxGroup.GetComponentsInChildren<Collider2D>().ToList();
            projectileSpawnLocations = projectileSpawnLocationGroup.GetComponentsInChildren<Transform>().ToList();

            projectileSpawnLocations.RemoveAt(0);

            movingStateHash = movingStateString.GetHashCode();
            meleeAttackingStateHash = meleeAttackingStateString.GetHashCode();
            rangedAttackingStateHash = rangedAttackingStateString.GetHashCode();
            damageTakenStateHash = damageTakenStateString.GetHashCode();
            targettingStateHash = targettingStateString.GetHashCode();
            blockingStateHash = blockingStateString.GetHashCode();
            spiritBurstingStateHash = spiritBurstingStateString.GetHashCode();
            flightDashingStateHash = flightDashingStateString.GetHashCode();
            emergencyTeleportingStateHash = emergencyTeleportingStateString.GetHashCode();

            movingState.isCurrentState = true;
        }

        [ClientCallback]
        private void Update()
        {
            if(!hasAuthority) { return; }
        }

        [ClientCallback]
        private void FixedUpdate()
        {
            if (!hasAuthority) { return; }
        }

        public void SwitchState(int currentStateHash, int newStateHash)
        {
            #region DealWithCurrentState

            if (currentStateHash == movingStateHash) 
            {
                movingState.isCurrentState = false;
            }

            if (currentStateHash == meleeAttackingStateHash)
            {
                meleeAttackingState.isCurrentState = false;
            }

            if (currentStateHash == rangedAttackingStateHash)
            {
                rangedAttackingState.isCurrentState = false;
            }

            if (currentStateHash == damageTakenStateHash)
            {
                damageTakenState.isCurrentState = false;
            }

            if (currentStateHash == targettingStateHash)
            {
                targettingState.isCurrentState = false;
            }

            if (currentStateHash == blockingStateHash)
            {
                blockingState.isCurrentState = false;
            }

            if (currentStateHash == spiritBurstingStateHash)
            {
                spiritBurstingState.isCurrentState = false;
            }

            if (currentStateHash == flightDashingStateHash)
            {
                flightDashingState.isCurrentState = false;
            }

            if (currentStateHash == emergencyTeleportingStateHash)
            {
                emergencyTeleportingState.isCurrentState = false;
            }

            #endregion

            #region DealWithNewState

            if (newStateHash == meleeAttackingStateHash)
            {
                meleeAttackingState.attackIndex = 0;
                meleeAttackingState.isCurrentState = true;
                meleeAttackingState.MeleeAttacking();
            }

            if (newStateHash == rangedAttackingStateHash)
            {
                rangedAttackingState.isCurrentState = true;
                rangedAttackingState.RangedAttacking();
            }

            if (newStateHash == movingStateHash)
            {
                movingState.isCurrentState = true;
            }

            if (newStateHash == damageTakenStateHash)
            {
                damageTakenState.isCurrentState = true;
                damageTakenState.EnterDamageTakenState();
            }

            if (newStateHash == targettingStateHash)
            {
                targettingState.isCurrentState = true;
            }

            if (newStateHash == blockingStateHash)
            {
                blockingState.isCurrentState = true;
                blockingState.Blocking();
            }

            if (newStateHash == spiritBurstingStateHash)
            {
                spiritBurstingState.isCurrentState = true;
                spiritBurstingState.SpiritBursting();
            }

            if (newStateHash == flightDashingStateHash)
            {
                flightDashingState.isCurrentState = true;
            }

            if (newStateHash == emergencyTeleportingStateHash)
            {
                emergencyTeleportingState.isCurrentState = true;
                emergencyTeleportingState.Teleporting();
            }

            #endregion
        }

        public void SwitchToFlight()
        {
            isFlying = true;
            CmdSwitchLayer("Grounded");
        }

        public void SwitchToGrounded()
        {
            isFlying = false;
            CmdSwitchLayer("Flying");
        }

        [ClientRpc]
        private void RpcSwitchLayer(string newLayer)
        {
            gameObject.layer = LayerMask.NameToLayer(newLayer);
        }

        [ClientRpc]
        private void RpcSetAuraActive(bool isEnabled)
        {
            aura.SetActive(isEnabled);
        }

        #endregion
    }
}
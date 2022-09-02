using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class EmergencyTeleportingState : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Transform teleportLocation;
        private SpriteRenderer spriteRenderer;

        public bool isCurrentState;
        public bool canTeleport;
        public bool wasTargetting;

        private string thisState = "EmergencyTeleportingState";
        private string movingState = "MovingState";
        private string targettingState = "TargettingState";

        private int thisStateHash;
        private int nextStateHash;
        private int movingStateHash;
        private int targettingStateHash;

        #region Server

        [Command]
        private void CmdDisableSprite()
        {
            RpcSetSprite(false);
        }

        [Command]
        private void CmdEnableSprite()
        {
            RpcSetSprite(true);
        }

        #endregion 

        #region Client

        [ClientCallback]
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        [ClientCallback]
        private void Start()
        {
            thisStateHash = thisState.GetHashCode();
            movingStateHash = movingState.GetHashCode();
            targettingStateHash = targettingState.GetHashCode();

            canTeleport = true;
        }

        [ClientCallback]
        private void Update()
        {
            if (!spriteRenderer.enabled)
            {
                StartCoroutine(ResetSprite());
            }
        }

        public void Teleporting()
        {
            if (!hasAuthority) { return; }

            transform.position = teleportLocation.position;
            canTeleport = false;
            StartCoroutine(TeleportingCooldown());
            CmdDisableSprite();
            EndTeleport();
        }

        private IEnumerator TeleportingCooldown()
        {
            yield return new WaitForSeconds(16f);

            canTeleport = true;
        }

        private IEnumerator ResetSprite()
        {
            yield return new WaitForSeconds(0.3f);

            CmdEnableSprite();
        }

        private void EndTeleport()
        {
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

        [ClientRpc]
        private void RpcSetSprite(bool isEnabled)
        {
            spriteRenderer.enabled = isEnabled;
        }

        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using RyoshiSoftware.Multiplayer.NetworkSpawnables;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class RangedAttackingState : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        
        [SerializeField] private GameObject kiBlastPrefab;
        [SerializeField] private float spawnPositionOffsetx = -2f;
        [SerializeField] private float spawnPositionOffsety = -2f;
        private Animator animator;
        private KiBlast kiBlastRef;
        private GameObject kiBlastInstance;
        
        private float lastHorizontal;
        private float lastVertical;

        public bool isCurrentState;
        public bool wasTargetting;


        private string thisState = "RangedAttackingState";
        private string nextState;
        private string movingState = "MovingState";
        private string targettingState = "TargettingState";

        private readonly int rangedAttackingHash = Animator.StringToHash("rangedattacking");
        private readonly int movingHash = Animator.StringToHash("moving");
        private readonly int lastHorizontalHash = Animator.StringToHash("lasthorizontal");
        private readonly int lastVerticalHash = Animator.StringToHash("lastvertical");

        private int thisStateHash;
        private int nextStateHash;
        private int movingStateHash;
        private int targettingStateHash;

        #region Server

        [Command]
        private void CmdSpawnProjectile(Vector2 spawnPosition, float zRotation)
        {
            Vector2 kiSpawnPosition = new Vector2(spawnPosition.x + spawnPositionOffsetx, spawnPosition.y + spawnPositionOffsety);
            kiBlastInstance = Instantiate(kiBlastPrefab, kiSpawnPosition, Quaternion.Euler(new Vector3(0, 0, zRotation)));
            NetworkServer.Spawn(kiBlastInstance, connectionToClient);
        }

        #endregion

        #region Client

        [ClientCallback]
        void Start()
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

            if (playerController.inputManager.RangedAttackReleasedThisFrame())
            {
                StartCoroutine(EndRangedAttack());
            }
        }

        public void RangedAttacking()
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

            animator.SetBool(rangedAttackingHash, true);
        }

        private void SpawnKiBlast(int currentDirection)
        {
            if (!hasAuthority) { return; }

            if (currentDirection == 0)
            {
                CmdSpawnProjectile(playerController.projectileSpawnLocations[0].position, 90);
            }
            else if (currentDirection == 1)
            {
                CmdSpawnProjectile(playerController.projectileSpawnLocations[1].position, -90);
            }
            else if (currentDirection == 2)
            {
                CmdSpawnProjectile(playerController.projectileSpawnLocations[2].position, 180);
            }
            else if (currentDirection == 3)
            {
                CmdSpawnProjectile(playerController.projectileSpawnLocations[3].position, 0);
            }
        }

        private void LaunchKiUp()
        {
            SpawnKiBlast(0);
        }

        private void LaunchKiDown()
        {
            SpawnKiBlast(1);
        }

        private void LaunchKiLeft()
        {
            SpawnKiBlast(2);
        }

        private void LaunchKiRight()
        {
            SpawnKiBlast(3);
        }

        private IEnumerator EndRangedAttack()
        {
            yield return new WaitForSeconds(0.2f);

            animator.SetBool(rangedAttackingHash, false);
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

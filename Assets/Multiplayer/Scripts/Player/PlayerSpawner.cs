using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Transform playerSpawnPoint = null;

    #region Server

    [Command]
    private void CmdSpawnPlayer()
    {
        GameObject playerInstance = Instantiate(
            playerPrefab, 
            playerSpawnPoint.position, playerSpawnPoint.rotation
            );

            NetworkServer.Spawn(playerInstance, connectionToClient);
    }

    #endregion
    
    #region Client

    [ClientCallback]
    private void Start()
    {
        CmdSpawnPlayer();
    }

    #endregion
}

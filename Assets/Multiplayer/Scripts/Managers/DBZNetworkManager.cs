using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DBZNetworkManager : NetworkManager
{
    [SerializeField] private GameObject kiBlastSpawnerPrefab;

    public override void Start()
    {
        base.Start();
#if UNITY_SERVER || UNITY_EDITOR
        StartServer();
#endif
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
    }

}

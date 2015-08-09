using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Util;

public class CustomNetManager : NetworkManager
{
    public GameSetup gameSetup;
    public Transform[] spawnPoints;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject player = (GameObject)Instantiate(base.playerPrefab, spawnPoint.position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        gameSetup.RpcSetupUI(player);
        gameSetup.GiveInitialWeapon(player);
        gameSetup.SyncPlayerWeapons(conn, player);
        gameSetup.RpcSetupPlayerIds();
        gameSetup.SetupGameOver(this, player);

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        client.RegisterHandler(MessageTypes.SetupMessage, GameSetup.OnSetupWeapon);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);

        gameSetup.RpcSetupUI(null);
    }

    public void StartGame()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        NetworkManager.singleton.StartClient();
    }
}


using Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Util;

public class CustomNetManager : NetworkManager
{
    public GameSetup gameSetup;
    public Transform[] spawnPoints;

    public delegate void PlayerJoined();
    public static event PlayerJoined OnPlayerJoined;
    public delegate void PlayerLeft();
    public static event PlayerLeft OnPlayerLeft;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject player = (GameObject)Instantiate(base.playerPrefab, spawnPoint.position, Quaternion.identity);
        player.name = "Player " + conn.connectionId;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        gameSetup.SendSetupUi(player);
        gameSetup.GiveInitialWeapon(player);
        gameSetup.SyncPlayerWeapons(player);
        gameSetup.RpcSetupPlayerIds();
        gameSetup.SetupGameOver(conn, this, player);

        if (OnPlayerJoined != null)
        {
            OnPlayerJoined();
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        client.RegisterHandler(MessageTypes.SetupUi, GameSetup.OnSetupUi);
        client.RegisterHandler(MessageTypes.SetupWeapons, GameSetup.OnSetupWeapon);
        client.RegisterHandler(MessageTypes.SetupGameOver, GameSetup.OnSetupGameOver);
        client.RegisterHandler(MessageTypes.GrantExperience, PlayerLevel.OnPlayerExperience);
        client.RegisterHandler(MessageTypes.GrantPerk, PlayerPerks.OnShowUi);
        client.RegisterHandler(MessageTypes.PerkDone, PlayerPerks.OnHideUi);

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);

        if (OnPlayerLeft != null)
        {
            OnPlayerLeft();
        }
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


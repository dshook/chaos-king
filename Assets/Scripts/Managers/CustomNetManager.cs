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

    public delegate void PlayerJoined(GameObject player);
    public static event PlayerJoined OnPlayerJoined;
    public delegate void PlayerLeft();
    public static event PlayerLeft OnPlayerLeft;

    void Awake()
    {
        DontDestroyOnLoad(this);
        NetworkManager.singleton = this;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject player = (GameObject)Instantiate(base.playerPrefab, spawnPoint.position, Quaternion.identity);
        player.name = "Player " + conn.connectionId;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        //gameSetup.SendSetupUi(player);
        //gameSetup.GiveInitialWeapon(player);
        //gameSetup.SyncPlayerWeapons(player);
        //gameSetup.RpcSetupPlayerIds();
        //gameSetup.SetupGameOver(conn, this, player);

        if (OnPlayerJoined != null)
        {
            OnPlayerJoined(player);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        //base.OnClientConnect(conn);

        ClientScene.AddPlayer(conn, 0);
        client.RegisterHandler(MessageTypes.SetupUi, GameSetup.OnSetupUi);
        client.RegisterHandler(MessageTypes.SetupWeapons, GameSetup.OnSetupWeapon);
        client.RegisterHandler(MessageTypes.SetupGameOver, GameSetup.OnSetupGameOver);
        client.RegisterHandler(MessageTypes.GrantExperience, PlayerLevel.OnPlayerExperience);
        client.RegisterHandler(MessageTypes.GrantPerk, PlayerPerks.OnShowUi);
        client.RegisterHandler(MessageTypes.PerkDone, PlayerPerks.OnHideUi);
        client.RegisterHandler(MessageTypes.PlayerDamage, PlayerHealth.OnTakeDamage);
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

    public int port = 7777;

    public void StartGame()
    {
        networkPort = port;
        //StartHost();
        NetworkManager.singleton.networkPort = port;
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame(string serverIp)
    {
        NetworkManager.singleton.networkAddress = serverIp;
        NetworkManager.singleton.networkPort = port;

        NetworkManager.singleton.StartClient();
    }
}


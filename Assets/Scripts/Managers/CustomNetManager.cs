using Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Util;

public class CustomNetManager : NetworkManager
{
    public GameSetup gameSetup;

    public delegate void PlayerJoined(GameObject player);
    public static event PlayerJoined OnPlayerJoined;
    public delegate void PlayerLeft();
    public static event PlayerLeft OnPlayerLeft;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(base.playerPrefab, Vector3.zero, Quaternion.identity);
        player.name = "Player " + conn.connectionId;
        Debug.Log(player.name + " joined");
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        client.RegisterHandler(MessageTypes.SetupUi, GameSetup.OnSetupUi);
        client.RegisterHandler(MessageTypes.SetupWeapons, GameSetup.OnSetupWeapon);
        client.RegisterHandler(MessageTypes.SetupGameOver, GameSetup.OnSetupGameOver);
        client.RegisterHandler(MessageTypes.GrantExperience, PlayerLevel.OnPlayerExperience);
        client.RegisterHandler(MessageTypes.GrantPerk, PlayerPerks.OnShowUi);
        client.RegisterHandler(MessageTypes.PerkDone, PlayerPerks.OnHideUi);
        client.RegisterHandler(MessageTypes.PlayerDamage, PlayerHealth.OnTakeDamage);

        if (OnPlayerJoined != null)
        {
            OnPlayerJoined(player);
        }
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

    public void JoinGame(string ipAddress)
    {
        NetworkManager.singleton.networkAddress = ipAddress;
        NetworkManager.singleton.networkPort = port;

        NetworkManager.singleton.StartClient();
    }
}


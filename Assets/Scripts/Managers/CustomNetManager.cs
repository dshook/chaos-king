using UnityEngine;
using UnityEngine.Networking;
using Util;

public class CustomNetManager : NetworkManager
{
    public GameSetup gameSetup;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //Vector3 spawnPos = Vector3.right * conn.connectionId;
        var spawnPos = Vector3.zero;
        GameObject player = (GameObject)Instantiate(base.playerPrefab, spawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        gameSetup.RpcSetupUI(player);
        gameSetup.GiveInitialWeapon(player);
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


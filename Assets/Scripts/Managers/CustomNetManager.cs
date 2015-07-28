using UnityEngine;
using UnityEngine.Networking;

public class CustomNetManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //Vector3 spawnPos = Vector3.right * conn.connectionId;
        var spawnPos = Vector3.zero;
        GameObject player = (GameObject)Instantiate(base.playerPrefab, spawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
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


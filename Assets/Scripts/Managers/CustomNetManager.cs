using UnityEngine;
using UnityEngine.Networking;
using UI;

public class CustomNetManager : NetworkManager
{
    public GameObject LevelUI;
    public GameObject AmmoUI;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //Vector3 spawnPos = Vector3.right * conn.connectionId;
        var spawnPos = Vector3.zero;
        GameObject player = (GameObject)Instantiate(base.playerPrefab, spawnPos, Quaternion.identity);
        SetupUI(player);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);

        SetupUI(null);
    }

    public void StartGame()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        NetworkManager.singleton.StartClient();
    }

    void SetupUI(GameObject player)
    {
        if (LevelUI == null)
        {
            Debug.LogError("Couldn't set up UI with null objects");
            return;
        }
        var levelUIComponent = LevelUI.GetComponent<LevelUI>();
        levelUIComponent.player = player;

        var ammoUIComponent = AmmoUI.GetComponent<AmmoUI>();
        ammoUIComponent.player = player;
    }
}


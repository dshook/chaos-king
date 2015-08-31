using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UI;
using Util;
using Player;

public class GameSetup : NetworkBehaviour
{
    public GameObject LevelUI;
    public GameObject AmmoUI;
    public GameObject HealthUI;

    public GameObject StartingWeaponPrefab;
    public Transform[] spawnPoints;

    public void Awake()
    {
        CustomNetManager.OnPlayerJoined += SetupPlayer;
    }

    void SetupPlayer(GameObject player)
    {
        player.transform.position = GetNextSpawnpoint();

        SendSetupUi(player);
        GiveInitialWeapon(player);
        SyncPlayerWeapons(player);
        //RpcSetupPlayerIds();
        SetupGameOver(player);
    }

    public void SetupUI(GameObject player)
    {
        if (LevelUI == null)
        {
            Debug.LogError("Couldn't set up UI with null objects");
            return;
        }
        var levelUIComponent = LevelUI.GetComponent<LevelUI>();
        if (levelUIComponent.player == null)
        {
            levelUIComponent.player = player;
        }

        var ammoUIComponent = AmmoUI.GetComponent<AmmoUI>();
        if (ammoUIComponent.player == null)
        {
            ammoUIComponent.player = player;
        }

        var healthUIComponent = HealthUI.GetComponent<HealthUI>();
        if (healthUIComponent.player == null)
        {
            healthUIComponent.player = player;
        }
    }

    public void SendSetupUi(GameObject player)
    {
        var msg = new PlayerMessage()
        {
            player = player
        };
        var connectionId = player.GetComponent<NetworkIdentity>().connectionToClient.connectionId;

        NetworkServer.SendToClient(connectionId, MessageTypes.SetupUi, msg);
    }

    public static void OnSetupUi(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<PlayerMessage>();
        var gameSetup = GameObject.Find("GameSetup").GetComponent<GameSetup>();
        gameSetup.SetupUI(msg.player);
    }


    //[ClientRpc]
    //public void RpcSetupPlayerIds()
    //{
    //    var players = GameObject.FindGameObjectsWithTag("Player");
    //    foreach (var player in players)
    //    {
    //        var playerTextMesh = player.GetComponentInChildren<TextMesh>();
    //        var identity = player.GetComponent<NetworkIdentity>();
    //        if (playerTextMesh != null && identity != null)
    //        {
    //            var networkId = identity.netId;
    //            playerTextMesh.text = networkId.ToString();
    //        }
    //    }
    //}

    public void GiveInitialWeapon(GameObject player)
    {
        Vector3 offMap = new Vector3(0, -100, 0);
        var startingWeapon = (GameObject)Instantiate(StartingWeaponPrefab, offMap, Quaternion.identity);
        NetworkServer.Spawn(startingWeapon);
        var weaponPickup = startingWeapon.GetComponent<WeaponPickup>();
        weaponPickup.RpcPickupWeapon(player);
    }

    /// <summary>
    /// When a client joins they need to have all the existing players weapons set up so they
    /// are in the right part of the hierarchy and pointers set up
    /// </summary>
    public void SyncPlayerWeapons(GameObject newPlayer)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        var alreadyConnectedPlayers = players.Where(x => x.gameObject != newPlayer);
        var connectionId = newPlayer.GetComponent<NetworkIdentity>().connectionToClient.connectionId;

        foreach (var connectedPlayer in alreadyConnectedPlayers)
        {
            var weapon = connectedPlayer.transform.FindChild("Weapon").GetComponentInChildren<NetworkIdentity>().gameObject;

            var msg = new WeaponSetupMessage()
            {
                weapon = weapon,
                player = connectedPlayer
            };

            NetworkServer.SendToClient(connectionId, MessageTypes.SetupWeapons, msg);
        }
    }


    public static void OnSetupWeapon(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<WeaponSetupMessage>();

        var weaponPickup = msg.weapon.GetComponent<WeaponPickup>();
        weaponPickup.PickupWeapon(msg.player);
    }

    public void SetupGameOver(GameObject player)
    {
        var gameOver = player.GetComponent<PlayerRespawn>();
        gameOver.gameSetup = this;

        var msg = new PlayerMessage();
        msg.player = player;

        var connectionId = player.GetComponent<NetworkIdentity>().connectionToClient.connectionId;

        NetworkServer.SendToClient(connectionId, MessageTypes.SetupGameOver, msg);
    }

    public static void OnSetupGameOver(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<PlayerMessage>();

        var gameOver = msg.player.GetComponent<PlayerRespawn>();
        gameOver.anim = GameObject.Find("HUDCanvas").GetComponent<Animator>();
    }

    public Vector3 GetNextSpawnpoint()
    {
        if (spawnPoints.Length == 0)
        {
            return Vector3.zero;
        }
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        return spawnPoint.position;
    }

    public class WeaponSetupMessage : MessageBase
    {
        public GameObject weapon;
        public GameObject player;
    }

    public class PlayerMessage : MessageBase
    {
        public GameObject player;
    }
}

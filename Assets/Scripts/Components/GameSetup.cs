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

    public GameObject StartingWeaponPrefab;

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


    [ClientRpc]
    public void RpcSetupPlayerIds()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            var playerTextMesh = player.GetComponentInChildren<TextMesh>();
            var identity = player.GetComponent<NetworkIdentity>();
            if (playerTextMesh != null && identity != null)
            {
                var networkId = identity.netId;
                playerTextMesh.text = networkId.ToString();
            }
        }
    }

    public void GiveInitialWeapon(GameObject player)
    {
        var startingWeapon = (GameObject)Instantiate(StartingWeaponPrefab, player.transform.position, Quaternion.identity);
        NetworkServer.Spawn(startingWeapon);

        RpcPickupInitialWeapon(startingWeapon, player);
    }

    [ClientRpc]
    void RpcPickupInitialWeapon(GameObject weapon, GameObject player)
    {
        PickupWeapon(weapon, player);
    }

    static void PickupWeapon(GameObject weapon, GameObject player)
    {
        var playerWeapon = player.transform.FindChild("Weapon");

        weapon.transform.parent = playerWeapon;

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        //activate players new weapon
        var weaponShoot = weapon.GetComponentInChildren<IShoot>();
        var playerShooting = player.GetComponent<PlayerShooting>();
        weaponShoot.Enable(playerShooting);
        playerShooting.SetGun(weaponShoot);

        weapon.SetActive(true);
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

            var msg = new WeaponSetupMessage() {
                weapon = weapon,
                player = connectedPlayer
            };

            NetworkServer.SendToClient(connectionId, MessageTypes.SetupWeapons, msg);
        }
    }


    public static void OnSetupWeapon(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<WeaponSetupMessage>();

        PickupWeapon(msg.weapon, msg.player);
    }

    public void SetupGameOver(NetworkConnection newConn, CustomNetManager netManager, GameObject newPlayer)
    {
        var gameOver = newPlayer.GetComponent<GameOverManager>();
        gameOver.netManager = netManager;

        var msg = new PlayerMessage();
        msg.player = newPlayer;

        newConn.Send(MessageTypes.SetupGameOver, msg);
    }

    public static void OnSetupGameOver(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<PlayerMessage>();

        var gameOver = msg.player.GetComponent<GameOverManager>();
        gameOver.anim = GameObject.Find("HUDCanvas").GetComponent<Animator>();
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

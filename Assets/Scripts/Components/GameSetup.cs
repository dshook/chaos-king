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

    [ClientRpc]
    public void RpcSetupUI(GameObject player)
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

    public class WeaponSetupMessage : MessageBase
    {
        public GameObject weapon;
        public GameObject player;
    }

    /// <summary>
    /// When a client joins they need to have all the existing players weapons set up so they
    /// are in the right part of the hierarchy and pointers set up
    /// </summary>
    public void SyncPlayerWeapons(NetworkConnection newPlayerConnection, GameObject newPlayer)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        var alreadyConnectedPlayers = players.Where(x => x.gameObject != newPlayer);

        foreach (var connectedPlayer in alreadyConnectedPlayers)
        {
            var weapon = connectedPlayer.transform.FindChild("Weapon").GetComponentInChildren<NetworkIdentity>().gameObject;

            var weaponNet = weapon.GetComponent<NetworkIdentity>().netId;
            var playerNet = connectedPlayer.GetComponent<NetworkIdentity>().netId;

            var msg = new WeaponSetupMessage();
            msg.weapon = weapon;
            msg.player = connectedPlayer;

            newPlayerConnection.Send(MessageTypes.SetupMessage, msg);
        }
    }


    public static void OnSetupWeapon(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<WeaponSetupMessage>();

        PickupWeapon(msg.weapon, msg.player);
    }
}

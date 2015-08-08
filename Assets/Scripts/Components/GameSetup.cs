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

    public void GiveInitialWeapon(GameObject player)
    {
        var startingWeapon = (GameObject)Instantiate(StartingWeaponPrefab, player.transform.position, Quaternion.identity);
        NetworkServer.Spawn(startingWeapon);

        RpcPickupInitialWeapon(startingWeapon, player);
    }

    [ClientRpc]
    void RpcPickupInitialWeapon(GameObject weapon, GameObject player)
    {
        Debug.Log("Client given initial weapon");
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

}

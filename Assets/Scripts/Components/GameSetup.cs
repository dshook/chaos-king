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
        startingWeapon.GetComponent<BoxCollider>().enabled = false;
        var actualWeapon = startingWeapon.transform.FindChild("Weapon").gameObject;
        NetworkServer.Spawn(actualWeapon);

        RpcPickupInitialWeapon(actualWeapon, player);
        Destroy(startingWeapon, 2.0f);
    }

    [ClientRpc]
    void RpcPickupInitialWeapon(GameObject weapon, GameObject player)
    {
        var playerWeapon = player.transform.FindChild("Weapon");
        weapon.transform.position = playerWeapon.transform.position;

        //destroy players current weapon and grab pickups weapon
        weapon.transform.parent = player.transform;

        weapon.transform.localRotation = Quaternion.identity;

        //activate players new weapon
        var weaponShoot = weapon.GetComponentInChildren<IShoot>();
        var playerShooting = player.GetComponent<PlayerShooting>();
        weaponShoot.Enable(playerShooting);
        playerShooting.SetGun(weaponShoot);

        weapon.SetActive(true);

        Destroy(playerWeapon.gameObject);
    }

}

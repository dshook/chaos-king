using UnityEngine;
using UnityEngine.Networking;
using Player;

public class WeaponPickup : NetworkBehaviour
{
    GameObject weapon;

    void Awake()
    {
        weapon = transform.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.CompareTag("Player"))
        {
            var player = other.gameObject;
            RpcPickupWeapon(player);
        }
    }

    [ClientRpc]
    public void RpcPickupWeapon(GameObject player)
    {
        PickupWeapon(player);
    }

    public void PickupWeapon(GameObject player)
    {
        //store various weapon positions and rotations
        var playerWeapon = player.transform.FindChild("Weapon");

        var currentWeapon = playerWeapon.GetComponentInChildren<IShoot>();

        weapon.transform.parent = playerWeapon;

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        //disable unneeded components 
        weapon.GetComponent<BoxCollider>().enabled = false;
        weapon.GetComponent<WeaponMovement>().enabled = false;
        weapon.GetComponent<WeaponPickup>().enabled = false;
        weapon.GetComponent<KillTimer>().enabled = false;

        //activate players new weapon
        var weaponShoot = weapon.GetComponent<IShoot>();
        var playerShooting = player.GetComponent<PlayerShooting>();
        weaponShoot.Enable(playerShooting);
        playerShooting.SetGun(weaponShoot);

        weapon.SetActive(true);

        if (currentWeapon != null)
        {
            currentWeapon.Disable();
            Destroy(((Component)currentWeapon).gameObject);
        }
    }
}

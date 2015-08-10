using UnityEngine;
using UnityEngine.Networking;
using Player;

public class WeaponPickup : NetworkBehaviour
{
    GameObject weapon;

    void Awake()
    {
        weapon = transform.FindChild("Weapon").gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.CompareTag("Player"))
        {
            var player = other.gameObject;
            RpcPickupWeapon(player);

            //destroy pickup
            Destroy(gameObject, 0.5f);
        }
    }

    [ClientRpc]
    public void RpcPickupWeapon(GameObject player)
    {
        //store various weapon positions and rotations
        var playerWeapon = player.transform.FindChild("Weapon");

        if (!playerWeapon)
        {
            Debug.Log("No weapon to pick up");
            return;
        }

        var currentWeapon = (Component)playerWeapon.GetComponentInChildren<IShoot>();

        weapon.transform.parent = playerWeapon;

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        //activate players new weapon
        var weaponShoot = weapon.GetComponentInChildren<IShoot>();
        var playerShooting = player.GetComponent<PlayerShooting>();
        weaponShoot.Enable(playerShooting);
        playerShooting.SetGun(weaponShoot);

        weapon.SetActive(true);

        Destroy(currentWeapon.gameObject);
    }
}

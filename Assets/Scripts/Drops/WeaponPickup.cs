using UnityEngine;
using Player;

public class WeaponPickup : MonoBehaviour
{
    GameObject weapon;

    void Awake()
    {
        weapon = transform.FindChild("Weapon").gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other;
            //store various weapon positions and rotations
            var playerWeapon = player.transform.FindChild("Weapon");
            var playerWeaponPosition = playerWeapon.transform.position;

            //destroy players current weapon and grab pickups weapon
            Destroy(playerWeapon.gameObject);
            weapon.transform.parent = player.transform;

            weapon.transform.position = playerWeaponPosition;
            weapon.transform.localRotation = Quaternion.identity;

            //activate players new weapon
            var weaponShoot = weapon.GetComponent<IShoot>();
            var playerShooting = player.GetComponent<PlayerShooting>();
            weaponShoot.Enable(playerShooting);
            playerShooting.SetGun(weaponShoot);

            weapon.SetActive(true);

            var killTimer = weapon.GetComponent<KillTimer>();
            Destroy(killTimer);

            //destroy pickup
            Destroy(gameObject, 0.1f);
        }
    }
}

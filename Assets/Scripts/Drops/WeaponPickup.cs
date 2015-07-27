using UnityEngine;
using Player;

public class WeaponPickup : MonoBehaviour
{

    GameObject player;
    GameObject weapon;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weapon = transform.FindChild("Weapon").gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
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

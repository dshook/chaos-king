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
            Vector3 playerGunPosition = playerWeapon.transform.position;
            //Vector3 gunMeshPosition = playerWeapon.FindChild("Gun").position;
            //var gunRotation = playerWeapon.FindChild("Gun").rotation;

            //Vector3 gunPosition = player.transform.position;
            //Vector3 gunMeshPosition = playerWeapon.FindChild("GunMesh").position;
            var gunMeshRotation = playerWeapon.FindChild("GunMesh").rotation;
            var gunRotation = player.transform.rotation;

            //destroy players current weapon and grab pickups weapon
            Destroy(playerWeapon.gameObject);
            weapon.transform.parent = player.transform;

            //fix new weapons posistion and rotation
            weapon.transform.position = playerGunPosition;
            weapon.transform.rotation = gunRotation;
            //weapon.transform.FindChild("Gun").position = gunMeshPosition;
            //weapon.transform.FindChild("GunMesh").rotation = gunMeshRotation;

            //activate players new weapon
            var weaponShoot = weapon.GetComponent<IShoot>();
            var playerShooting = player.GetComponent<PlayerShooting>();
            weaponShoot.Enable(playerShooting);
            playerShooting.SetGun(weaponShoot);

            weapon.SetActive(true);

            //destroy pickup
            Destroy(gameObject, 0.1f);
        }
    }
}

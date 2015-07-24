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
            var playerWeapon = player.transform.FindChild("Weapon");
            Vector3 gunPosition = playerWeapon.transform.position;
            //Vector3 gunMeshPosition = playerWeapon.FindChild("Gun").position;
            //var gunRotation = playerWeapon.FindChild("Gun").rotation;

            //Vector3 gunPosition = player.transform.position;
            //Vector3 gunMeshPosition = playerWeapon.FindChild("GunMesh").position;
            var gunMeshRotation = playerWeapon.FindChild("GunMesh").rotation;
            var gunRotation = player.transform.rotation;

            Destroy(playerWeapon.gameObject);

            weapon.transform.parent = player.transform;
            weapon.transform.position = gunPosition;
            //weapon.transform.rotation = gunRotation;
            //weapon.transform.FindChild("Gun").position = gunMeshPosition;
            weapon.transform.FindChild("GunMesh").rotation = gunMeshRotation;

            var weaponShoot = weapon.GetComponent<IShoot>();
            var playerShooting = player.GetComponent<PlayerShooting>();
            weaponShoot.Enable(playerShooting);
            playerShooting.SetGun(weaponShoot);

            weapon.SetActive(true);

            Destroy(gameObject, 0.1f);
        }
    }
}

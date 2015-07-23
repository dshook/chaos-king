using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour {

	GameObject player;
	GameObject weapon;

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
		weapon = transform.FindChild("Weapon").gameObject;
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player)
		{
            var playerWeapon = player.transform.FindChild("Weapon");
			Vector3 gunPosition = playerWeapon.position;
			Vector3 gunMeshPosition = playerWeapon.FindChild("Gun").position;
            var gunRotation = playerWeapon.FindChild("Gun").rotation;

			Destroy(playerWeapon.gameObject);

			weapon.transform.parent = player.transform;
			weapon.transform.position = gunPosition;
			weapon.transform.FindChild("Gun").position = gunMeshPosition;
            weapon.transform.FindChild("Gun").rotation = gunRotation;
			weapon.SetActive(true);

			Destroy (gameObject, 0.1f);
		}
	}
}

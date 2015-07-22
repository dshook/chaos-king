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
			Vector3 gunPosition = player.transform.FindChild("Weapon").position;
			Vector3 gunMeshPosition = player.transform.FindChild("Weapon").FindChild("Gun").position;

			Destroy(player.transform.FindChild("Weapon").gameObject);

			weapon.transform.parent = player.transform;
			weapon.transform.position = gunPosition;
			weapon.transform.FindChild("Gun").position = gunMeshPosition;
			weapon.SetActive(true);

			Destroy (gameObject, 0.1f);
		}
	}
}

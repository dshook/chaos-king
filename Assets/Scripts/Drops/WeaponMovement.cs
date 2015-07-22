using UnityEngine;
using System.Collections;

public class WeaponMovement : MonoBehaviour {
	
	public float rotationSpeed = 6f;
	public float bounceSpeed = 6f;

	Rigidbody weaponRigidbody;

	void Awake(){
		weaponRigidbody = GetComponent<Rigidbody> ();
	}
	
	void FixedUpdate(){
		Float ();
	}
	
	void Float(){
		float currentAngle = weaponRigidbody.rotation.eulerAngles.y;
		weaponRigidbody.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * rotationSpeed), Vector3.up);
	}

	void PickUp(){
	}
}

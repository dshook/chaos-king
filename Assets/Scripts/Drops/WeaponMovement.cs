using UnityEngine;
using System.Collections;

public class WeaponMovement : MonoBehaviour {
	
	public float rotationSpeed = 6f;
	public float bounceSpeed = 6f;

	void Awake(){
	}
	
	void FixedUpdate(){
		Float ();
	}
	
	void Float(){
		float currentAngle = transform.rotation.eulerAngles.y;
		transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * rotationSpeed), Vector3.up);
	}

	void PickUp(){
	}
}

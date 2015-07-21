﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
	public int enemiesPierced = 5;


    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    PlayerLevel playerLevel;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
        playerLevel = GetComponentInParent<PlayerLevel>();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(CrossPlatformInputManager.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

		for(var p = 0; p < enemiesPierced; p++){

        	if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        	{
            	EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            	if(enemyHealth != null)
            	{
            	    enemyHealth.TakeDamage (damagePerShot, shootHit.point, playerLevel);
            	}
            	gunLine.SetPosition (1, shootHit.point);
				shootRay.origin = shootHit.point;
        	}
        	else
        	{
            	gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
				break;
        	}
		}
    }
}

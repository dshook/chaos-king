﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
	public int enemiesPierced = 5;
	public int shotFired = 5;

    public Material gunLineMaterial;


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

	List<GameObject> gunLineList;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
        playerLevel = GetComponentInParent<PlayerLevel>();
        gunLineList = new List<GameObject>();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(CrossPlatformInputManager.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
			for(var p = 0; p < shotFired; p++){
            	Shoot (Random.Range(-30,30));
			}
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLight.enabled = false;

        foreach(var line in gunLineList) {
            Destroy(line, 0.1f);
        }
        gunLineList.Clear();
    }


    void Shoot (int angle)
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        var gunLineObject = createLine();
        var gunLine = gunLineObject.GetComponent<LineRenderer>();
        gunLineList.Add(gunLineObject);

        shootRay.origin = transform.position;
		shootRay.direction = Quaternion.Euler(0,angle,0) * transform.forward;

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

    GameObject createLine() {
        var emptyObject = new GameObject();
        emptyObject.transform.parent = this.transform;
        var gunLine = emptyObject.AddComponent<LineRenderer>();
        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);
        gunLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        gunLine.receiveShadows = false;
        gunLine.material = gunLineMaterial;
        gunLine.useLightProbes = true;
        gunLine.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbes;
        gunLine.SetWidth(0.05f, 0.05f);
        gunLine.useWorldSpace = true;

        return emptyObject;
    }
}

﻿using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace Player
{
    public class ShotgunShooting : MonoBehaviour
    {
        public int damagePerShot = 20;
        public float timeBetweenBullets = 0.15f;
        public float range = 100f;
        public int enemiesPierced = 5;
        public int shotFired = 5;
        public int spreadAngle = 20;
		public int maxAmmo = 8;
		public int currentAmmo = 0;
		Slider ammoSlider;
		public float reloadSpeed = 0.5f;
		bool reloading = false;

        public Material gunLineMaterial;


        float timer;
        Ray shootRay;
        RaycastHit shootHit;
        int shootableMask;
        ParticleSystem gunParticles;
        PlayerLevel playerLevel;
        AudioSource gunAudio;
        Light gunLight;
        float effectsDisplayTime = 0.2f;

        GameObject[] gunLines;


        void Awake ()
        {
        }
		
		void Start(){
			shootableMask = LayerMask.GetMask ("Shootable");
			gunLines = new GameObject[shotFired];
			gunParticles = GetComponent<ParticleSystem> ();
			gunAudio = GetComponent<AudioSource> ();
			gunLight = GetComponent<Light> ();
			playerLevel = GetComponentInParent<PlayerLevel>();
			for(int i = 0; i < shotFired; i++) {
				gunLines[i] = createLine();
			}
			ammoSlider = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("AmmoUI").gameObject.GetComponentInChildren<Slider> ();
			currentAmmo = maxAmmo;
			ammoSlider.maxValue = maxAmmo;
			ammoSlider.value = currentAmmo;
		}


        void Update ()
        {
            timer += Time.deltaTime;

			if (reloading == true && timer >= reloadSpeed) {
				currentAmmo = maxAmmo;
				ammoSlider.maxValue = maxAmmo;
				ammoSlider.value = currentAmmo;
				reloading = false;
			}
            if(CrossPlatformInputManager.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            {
				if(currentAmmo > 0){
	                for(var p = 0; p < shotFired; p++){
	                    Shoot (Random.Range(-spreadAngle,spreadAngle), p);
	                }
					currentAmmo--;
					ammoSlider.maxValue = maxAmmo;
					ammoSlider.value = currentAmmo;
				}
				else{
					reloading = true;
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

            foreach(var line in gunLines) {
                var lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.enabled = false;
            }
        }


        void Shoot (int angle, int shotIndex)
        {
            timer = 0f;

            gunAudio.Play ();

            gunLight.enabled = true;

            gunParticles.Stop ();
            gunParticles.Play ();

            var gunLineObject = gunLines[shotIndex];
            var gunLine = gunLineObject.GetComponent<LineRenderer>();
            gunLine.SetPosition (0, transform.position);
            gunLine.enabled = true;

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
}
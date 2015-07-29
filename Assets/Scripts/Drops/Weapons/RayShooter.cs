using UnityEngine;
using UnityEngine.Networking;
using Player;

namespace Weapons
{
    /// <summary>
    /// Base class for all weapons that use raycasts to shoot
    /// </summary>
    public class RayShooter : NetworkBehaviour
    {
        public int damagePerShot = 1;
        public float timeBetweenBullets = 0.5f;
        public float range = 100f;
        public int enemiesPierced = 0;
        public int shotFired = 1;
        public int spreadAngle = 0;
        public float reloadSpeed = 1.5f;

        public int maxAmmo {
            get { return playerShooting.maxAmmo;  }
            set { playerShooting.maxAmmo = value; }
        }
        public int currentAmmo {
            get { return playerShooting.currentAmmo;  }
            set { playerShooting.currentAmmo = value; }
        }

        public Material gunLineMaterial;
        public PlayerShooting playerShooting;

        bool reloading = false;

        protected float shootTimer;
        Ray shootRay;
        RaycastHit shootHit;
        ParticleSystem gunParticles;
        PlayerLevel playerLevel;
        AudioSource gunAudio;
        Light gunLight;
        float effectsDisplayTime = 0.4f;

        GameObject[] gunLines;


        void Awake()
        {
        }

        protected void Start()
        {
            gunLines = new GameObject[shotFired];
            gunParticles = GetComponentInChildren<ParticleSystem>();
            gunAudio = GetComponent<AudioSource>();
            gunLight = GetComponentInChildren<Light>();
            playerLevel = GetComponentInParent<PlayerLevel>();
            for (int i = 0; i < shotFired; i++)
            {
                gunLines[i] = createLine();
            }
            currentAmmo = maxAmmo;
        }

        public void Enable(PlayerShooting ps)
        {
            playerShooting = ps;
            currentAmmo = maxAmmo;
            enabled = true;
        }

        void Update()
        {
            shootTimer += Time.deltaTime;

            if (reloading == true && shootTimer >= (reloadSpeed * playerShooting.reloadSpeedMultiplier))
            {
                currentAmmo = maxAmmo + playerShooting.ammoBoost;
                reloading = false;
                shootTimer = 0f;
            }

            if (shootTimer >= (timeBetweenBullets * playerShooting.attackSpeedMultiplier) * effectsDisplayTime)
            {
                DisableEffects();
            }
        }

        public void Shoot()
        {
            if (shootTimer >= (timeBetweenBullets * playerShooting.attackSpeedMultiplier) && Time.timeScale != 0)
            {
                if (currentAmmo > 0)
                {
                    for (var p = 0; p < shotFired; p++)
                    {
                        FireWeapon(Random.Range(-spreadAngle, spreadAngle), p);
                    }
                    currentAmmo--;
                }
                else
                {
                    reloading = true;
                }
            }
        }


        public void DisableEffects()
        {
            gunLight.enabled = false;

            foreach (var line in gunLines)
            {
                var lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.enabled = false;
            }
        }

        protected virtual void FireWeapon(int angle, int shotIndex)
        {
            shootTimer = 0f;

            gunAudio.Play();

            gunLight.enabled = true;

            gunParticles.Stop();
            gunParticles.Play();

            var gunLineObject = gunLines[shotIndex];
            var gunLine = gunLineObject.GetComponent<LineRenderer>();
            gunLine.SetPosition(0, transform.position);
            gunLine.enabled = true;

            shootRay.origin = transform.position;
            shootRay.direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            for (var p = 0; p <= (enemiesPierced + playerShooting.extraEnemiesPierced); p++)
            {
                if (Physics.Raycast(shootRay, out shootHit, range * playerShooting.rangeMultiplier, playerShooting.shootableMask))
                {
                    EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(Mathf.RoundToInt(damagePerShot * playerShooting.damageMultiplier), shootHit.point, playerLevel);
                    }
                    gunLine.SetPosition(1, shootHit.point);
                    shootRay.origin = shootHit.point;
                }
                else
                {
                    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range * playerShooting.rangeMultiplier);
                    break;
                }
            }
        }

        GameObject createLine()
        {
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

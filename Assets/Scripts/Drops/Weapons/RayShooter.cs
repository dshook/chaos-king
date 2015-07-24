using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using Player;

namespace Weapons
{
    /// <summary>
    /// Base class for all weapons that use raycasts to shoot
    /// </summary>
    public class RayShooter : MonoBehaviour
    {
        public int damagePerShot = 1;
        public float timeBetweenBullets = 0.5f;
        public float range = 100f;
        public int enemiesPierced = 0;
        public int shotFired = 1;
        public int spreadAngle = 0;
        public int maxAmmo = 10;
        public int currentAmmo = 0;
        public float reloadSpeed = 1.5f;

        public Material gunLineMaterial;
        public PlayerShooting playerShooting;

        bool reloading = false;

        float shootTimer;
        Ray shootRay;
        RaycastHit shootHit;
        ParticleSystem gunParticles;
        PlayerLevel playerLevel;
        AudioSource gunAudio;
        Light gunLight;
        float effectsDisplayTime = 0.2f;

        GameObject[] gunLines;


        void Awake()
        {
        }

        protected void Start()
        {
            gunLines = new GameObject[shotFired];
            gunParticles = GetComponent<ParticleSystem>();
            gunAudio = GetComponent<AudioSource>();
            gunLight = GetComponent<Light>();
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
            playerShooting.ammoSlider.maxValue = maxAmmo;
            currentAmmo = maxAmmo;
            UpdateAmmoUI();
            enabled = true;
        }

        void Update()
        {
            shootTimer += Time.deltaTime;

            if (reloading == true && shootTimer >= reloadSpeed)
            {
                currentAmmo = maxAmmo;
                UpdateAmmoUI();
                reloading = false;
                shootTimer = 0f;
            }

            if (shootTimer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects();
            }
        }

        public void Shoot()
        {
            if (shootTimer >= timeBetweenBullets && Time.timeScale != 0)
            {
                if (currentAmmo > 0)
                {
                    for (var p = 0; p < shotFired; p++)
                    {
                        Shoot(Random.Range(-spreadAngle, spreadAngle), p);
                    }
                    currentAmmo--;
                    UpdateAmmoUI();
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


        void Shoot(int angle, int shotIndex)
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

            for (var p = 0; p <= enemiesPierced; p++)
            {
                if (Physics.Raycast(shootRay, out shootHit, range, playerShooting.shootableMask))
                {
                    EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damagePerShot, shootHit.point, playerLevel);
                    }
                    gunLine.SetPosition(1, shootHit.point);
                    shootRay.origin = shootHit.point;
                }
                else
                {
                    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
                    break;
                }
            }
        }

        void UpdateAmmoUI()
        {
            playerShooting.ammoSlider.value = currentAmmo;
            playerShooting.ammoText.text = string.Format("{0}/{1}", currentAmmo, maxAmmo);
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

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
        [SyncVar] public int maxAmmo = 10;
        [SyncVar] public int currentAmmo = 10;

        public GameObject gunLinePrefab;
        public PlayerShooting playerShooting;

        protected bool reloading = false;

        protected float shootTimer;
        protected Ray shootRay;
        protected RaycastHit shootHit;
        protected ParticleSystem gunParticles;
        protected PlayerLevel playerLevel;
        protected AudioSource gunAudio;
        protected Light gunLight;
        protected float effectsDisplayTime = 0.2f;
        protected Transform gunTip;

        private bool effectsShowing = false;
        GameObject[] gunLines;


        void Awake()
        {
        }

        protected void Start()
        {
            gunLines = new GameObject[shotFired];
            gunParticles = GetComponentInChildren<ParticleSystem>();
            gunAudio = GetComponentInChildren<AudioSource>();
            gunLight = GetComponentInChildren<Light>();
            playerLevel = GetComponentInParent<PlayerLevel>();
            gunTip = transform.FindChild("GunTip").transform;
            currentAmmo = maxAmmo;

            if (isServer)
            {
                for (int i = 0; i < shotFired; i++)
                {
                    gunLines[i] = createLine();
                }
                RpcDisableEffects(gunLines);
            }
        }

        public virtual void Enable(PlayerShooting ps)
        {
            playerShooting = ps;
            currentAmmo = maxAmmo;
            enabled = true;
        }

        public virtual void Disable()
        {

        }

        void Update()
        {
            if (!isServer) return;

            shootTimer += Time.deltaTime;

            if (reloading == true && shootTimer >= (reloadSpeed * playerShooting.reloadSpeedMultiplier))
            {
                currentAmmo = maxAmmo + playerShooting.ammoBoost;
                reloading = false;
                shootTimer = 0f;
            }

            if (effectsShowing && shootTimer >= (timeBetweenBullets * playerShooting.attackSpeedMultiplier) * effectsDisplayTime)
            {
                RpcDisableEffects(gunLines);
            }
        }

        public virtual void Shoot()
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

                    RpcPlayEffects(gunLines);
                }
                else
                {
                    reloading = true;
                }
            }
        }

        protected virtual void FireWeapon(int angle, int shotIndex)
        {
            shootTimer = 0f;

            var gunLineObject = gunLines[shotIndex];
            var gunLine = gunLineObject.GetComponent<SyncLinePosition>();
            gunLine.SetStart(gunTip.position);

            shootRay.origin = gunTip.position;
            shootRay.direction = (Quaternion.Euler(0, angle, 0) * gunTip.forward).normalized;
            var remainingRange = range * playerShooting.rangeMultiplier;

            for (var p = 0; p <= (enemiesPierced + playerShooting.extraEnemiesPierced); p++)
            {
                if (Physics.Raycast(shootRay, out shootHit, remainingRange, playerShooting.shootableMask))
                {
                    EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(Mathf.RoundToInt(damagePerShot * playerShooting.damageMultiplier), shootHit.point, playerLevel);
                    }
                    remainingRange -= Vector3.Distance(shootRay.origin, shootHit.point);

                    gunLine.SetEnd(shootHit.point);
                    shootRay.origin = shootHit.point;
                }
                else
                {
                    gunLine.SetEnd(shootRay.origin + (shootRay.direction * remainingRange));
                    break;
                }
            }
        }

        [ClientRpc]
        protected void RpcPlayEffects(GameObject[] gunLines)
        {
            gunAudio.Play();

            gunLight.enabled = true;

            gunParticles.Stop();
            gunParticles.Play();

            foreach (var gunLineObject in gunLines)
            {
                var gunLine = gunLineObject.GetComponent<LineRenderer>();
                gunLine.enabled = true;
            }
            effectsShowing = true;
        }

        [ClientRpc]
        public void RpcDisableEffects(GameObject[] gunLines)
        {
            if (gunLight != null)
            {
                gunLight.enabled = false;
            }

            foreach (var line in gunLines)
            {
                var lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.enabled = false;
            }
            effectsShowing = false;
        }


        GameObject createLine()
        {
            var newGunLine = (GameObject)Instantiate(gunLinePrefab, Util.Vector.hiddenSpawn, Quaternion.identity);
            newGunLine.transform.parent = gunTip.transform;
            var renderer = newGunLine.GetComponent<LineRenderer>();
            renderer.enabled = false;
            NetworkServer.Spawn(newGunLine);

            return newGunLine;
        }

        public int GetCurrentAmmo()
        {
            return currentAmmo;
        }

        public int GetMaxAmmo()
        {
            return maxAmmo;
        }
    }
}

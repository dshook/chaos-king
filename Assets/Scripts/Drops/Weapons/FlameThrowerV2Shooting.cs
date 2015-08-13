using UnityEngine;
using UnityEngine.Networking;
using Player;

namespace Weapons
{
    public class FlameThrowerV2Shooting : RayShooter, IShoot
    {
        private int maxFlames = 10;
        private GameObject[] flameArray;
        private Object flamePrefab = null;

        private int flameRotator = 0;

        public void Awake() {
            flamePrefab = Resources.Load("Projectiles/Flame");
        }

        public override void Enable(PlayerShooting ps)
        {
            base.Enable(ps);
            flameArray = new GameObject[maxFlames];

            if (isServer)
            {
                var hiddenSpawn = new Vector3(0, -100, 0);
                for (int i = 0; i < maxFlames; i++)
                {
                    var flame = Instantiate(flamePrefab, hiddenSpawn, Quaternion.identity) as GameObject;
                    flame.GetComponent<FlameProjectile>().enabled = false;
                    flameArray[i] = flame;
                    NetworkServer.Spawn(flame);
                }
            }
        }

        public override void Disable()
        {
            base.Disable();
            if (isServer)
            {
                foreach (var flame in flameArray)
                {
                    Destroy(flame);
                }
            }
            flameArray = null;
        }

        public override void Shoot()
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

        protected override void FireWeapon(int angle, int shotIndex)
        {
            shootTimer = 0f;

            //play when we have a flamethrower sound, gun shots sound wrong on this.
            //gunAudio.Play();
            //gunLight.enabled = true;

            if (isServer)
            {
                CreateFlame();
            }
        }

        void CreateFlame()
        {
            var projectile = flameArray[flameRotator];

            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;
            var flame = projectile.GetComponent<FlameProjectile>();
            flame.damage = (Mathf.RoundToInt(damagePerShot * playerShooting.damageMultiplier));
            flame.range = range * playerShooting.rangeMultiplier;
            flame.canPierce = enemiesPierced + playerShooting.extraEnemiesPierced;
            flame.playerLevel = playerLevel;
            flame.Enable();

            flameRotator++;
            if (flameRotator >= maxFlames) flameRotator = 0;
        }
    }
}

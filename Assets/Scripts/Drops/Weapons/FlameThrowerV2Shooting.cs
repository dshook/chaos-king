using UnityEngine;
using UnityEngine.Networking;
using Player;

namespace Weapons
{
    public class FlameThrowerV2Shooting : RayShooter, IShoot
    {

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
            GameObject projectile = Instantiate(Resources.Load("Projectiles/Flame"), transform.position, transform.rotation) as GameObject;
            var flame = projectile.GetComponent<FlameProjectile>();
            flame.damage = (Mathf.RoundToInt(damagePerShot * playerShooting.damageMultiplier));
            flame.range = range * playerShooting.rangeMultiplier;
            flame.canPierce = enemiesPierced + playerShooting.extraEnemiesPierced;
            flame.playerLevel = playerLevel;
            NetworkServer.Spawn(projectile);
        }
    }
}

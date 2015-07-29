using UnityEngine;
using Player;

namespace Weapons
{
    public class FlameThrowerV2Shooting : RayShooter, IShoot
    {
        protected override void CmdFireWeapon(int angle, int shotIndex)
        {
            shootTimer = 0f;

            //play when we have a flamethrower sound, gun shots sound wrong on this.
            //gunAudio.Play();
            //gunLight.enabled = true;

            CreateFlame();
        }

        void CreateFlame()
        {
            GameObject projectile = Instantiate(Resources.Load("Projectiles/Flame"), transform.position, GameObject.FindGameObjectWithTag("Player").transform.rotation) as GameObject;
            projectile.GetComponent<FlameProjectile>().damage = (Mathf.RoundToInt(damagePerShot * playerShooting.damageMultiplier));
            projectile.GetComponent<FlameProjectile>().range = range * playerShooting.rangeMultiplier;
            projectile.GetComponent<FlameProjectile>().canPierce = enemiesPierced + playerShooting.extraEnemiesPierced;
        }
    }
}

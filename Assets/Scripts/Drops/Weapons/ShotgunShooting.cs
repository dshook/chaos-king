using Player;

namespace Weapons
{
    public class ShotgunShooting : RayShooter, IShoot
    {
        new void Start()
        {
            base.Start();

            damagePerShot = 20;
            timeBetweenBullets = 0.15f;
            range = 100f;
            enemiesPierced = 5;
            shotFired = 5;
            spreadAngle = 20;
            maxAmmo = 8;
            currentAmmo = 8;
            reloadSpeed = 0.5f;
        }
    }
}

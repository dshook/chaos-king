using Player;

namespace Weapons
{
    public class MinigunShooting : RayShooter, IShoot
    {
        new void Start()
        {
            base.Start();

            damagePerShot = 8;
            timeBetweenBullets = 0.05f;
            range = 60f;
            enemiesPierced = 2;
            shotFired = 1;
            spreadAngle = 0;
            maxAmmo = 100;
            currentAmmo = 0;
            reloadSpeed = 3f;

            currentAmmo = maxAmmo;
        }
    }
}

using Player;

namespace Weapons
{
    public class MinigunShooting : RayShooter, IShoot
    {
        new void Start()
        {
            base.Start();

            damagePerShot = 5;
            timeBetweenBullets = 0.05f;
            range = 100f;
            enemiesPierced = 1;
            shotFired = 1;
            spreadAngle = 0;
            maxAmmo = 100;
            currentAmmo = 0;
            reloadSpeed = 5f;

            currentAmmo = maxAmmo;
        }
    }
}

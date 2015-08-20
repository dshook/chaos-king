using Player;

namespace Weapons
{
    public class AssaultRifleShooting : RayShooter, IShoot
    {
        new void Start()
        {
            base.Start();

            damagePerShot = 10;
            timeBetweenBullets = 0.15f;
            range = 80f;
            enemiesPierced = 0;
            shotFired = 1;
            spreadAngle = 2;
            maxAmmo = 20;
            currentAmmo = 0;
            reloadSpeed = 0.6f;
        }
    }
}

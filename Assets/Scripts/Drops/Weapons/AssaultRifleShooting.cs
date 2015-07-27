﻿using Player;

namespace Weapons
{
    public class AssaultRifleShooting : RayShooter, IShoot
    {
        new void Start()
        {
            base.Start();

            damagePerShot = 10;
            timeBetweenBullets = 0.15f;
            range = 100f;
            enemiesPierced = 0;
            shotFired = 1;
            spreadAngle = 5;
            maxAmmo = 20;
            currentAmmo = 0;
            reloadSpeed = 0.6f;
        }
    }
}
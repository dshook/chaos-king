using UnityEngine;
using System.Collections.Generic;
using Player;

namespace Weapons
{
    //may need to redo weapon to shoot out collision boxes instead, would prolly function better
    public class FlameThrowerShooting : RayShooter, IShoot
    {
        List<GameObject> enemies;

        new void Start()
        {
            base.Start();

            enemies = new List<GameObject>();
            
            damagePerShot = 10;
            timeBetweenBullets = 0.05f;
            range = 100f;
            enemiesPierced = 0;
            shotFired = 1;
            spreadAngle = 0;
            maxAmmo = 40;
            currentAmmo = 0;
            reloadSpeed = 0.10f;
        }
        
        public override void FireWeapon(int angle, int shotIndex)
        {
            shootTimer = 0f;

            //loop backwards so we can remove dead enemies from the list
            for (int i = enemies.Count-1; i >= 0; i--)
            {
                EnemyHealth enemyHealth = enemies[i].GetComponent<EnemyHealth>();
                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage(Mathf.RoundToInt(damagePerShot * playerShooting.damageMultiplier), shootHit.point, playerLevel);
                    if(enemyHealth.currentHealth <= 0)
                    {
                        enemies.Remove(enemies[i]);
                    }
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Enemy")
            {
                enemies.Add(other.gameObject);
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                enemies.Remove(other.gameObject);
            }
        }
    }
}

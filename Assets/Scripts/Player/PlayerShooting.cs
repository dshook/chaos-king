using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace Player
{
    public class PlayerShooting : NetworkBehaviour
    {
        public float damageMultiplier = 1;
        public float attackSpeedMultiplier = 1;
        public float rangeMultiplier = 1;
        public float reloadSpeedMultiplier = 1;
        public int extraEnemiesPierced = 0;
        public int ammoBoost = 0;

        public int maxAmmo = 10;
        public int currentAmmo = 0;

        public int shootableMask;

        IShoot gun;

        void Awake()
        {
        }

        void Start()
        {
            shootableMask = LayerMask.GetMask("Shootable");
            gun = GetComponentInChildren<IShoot>();

            gun.Enable(this);
        }


        void Update()
        {
            if (!isLocalPlayer) return;

            if (CrossPlatformInputManager.GetButton("Fire1"))
            {
                if (gun != null)
                {
                    gun.Shoot();
                }
            }
        }

        public void SetGun(IShoot gun)
        {
            this.gun = gun;
        }
    }
}

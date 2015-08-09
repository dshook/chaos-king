using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace Player
{
    public class PlayerShooting : NetworkBehaviour
    {
        [SyncVar] public float damageMultiplier = 1;
        [SyncVar] public float attackSpeedMultiplier = 1;
        [SyncVar] public float rangeMultiplier = 1;
        [SyncVar] public float reloadSpeedMultiplier = 1;
        [SyncVar] public int extraEnemiesPierced = 0;
        [SyncVar] public int ammoBoost = 0;

        [SyncVar] public int maxAmmo = 10;
        [SyncVar] public int currentAmmo = 0;

        public int shootableMask;

        IShoot gun;

        [SyncVar]
        private bool isShooting = false;

        void Awake()
        {
        }

        void Start()
        {
            shootableMask = LayerMask.GetMask("Shootable");
            gun = GetComponentInChildren<IShoot>();

            if (gun != null)
            {
                gun.Enable(this);
            }
        }

        void Update()
        {
            if (isLocalPlayer)
            {
                if (CrossPlatformInputManager.GetButton("Fire1"))
                {
                    CmdStartShooting();
                }
                else
                {
                    CmdStopShooting();
                }
            }

            if (isServer)
            {
                if (gun != null && isShooting)
                {
                    gun.Shoot();
                }
            }
        }

        public void SetGun(IShoot gun)
        {
            this.gun = gun;
        }

        [Command]
        public void CmdStartShooting()
        {
            isShooting = true;
        }

        [Command]
        public void CmdStopShooting()
        {
            isShooting = false;
        }
    }
}

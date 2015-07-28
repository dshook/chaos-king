using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace Player
{
    public class PlayerShooting : MonoBehaviour
    {
        public float damageMultiplier = 1;
        public float attackSpeedMultiplier = 1;
        public float rangeMultiplier = 1;
        public float reloadSpeedMultiplier = 1;
        public int extraEnemiesPierced = 0;
        public int ammoBoost = 0;

        public Slider ammoSlider;
        public Text ammoText;

        public int shootableMask;

        IShoot gun;

        void Awake()
        {
        }

        void Start()
        {
            shootableMask = LayerMask.GetMask("Shootable");
            gun = GetComponentInChildren<IShoot>();

            var ammoUI = GameObject.Find("AmmoUI");
            ammoSlider = ammoUI.GetComponentInChildren<Slider>();
            ammoText = ammoUI.GetComponentInChildren<Text>();

            gun.Enable(this);
        }


        void Update()
        {
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

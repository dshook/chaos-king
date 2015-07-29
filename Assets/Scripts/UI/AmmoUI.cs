using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UI
{
    public class AmmoUI : MonoBehaviour
    {
        GameObject _player;
        public GameObject player
        {
            get { return _player; }
            set
            {
                _player = value;
                playerShooting = _player.GetComponent<PlayerShooting>();
            }
        }

        Text text;
        Slider slider;
        PlayerShooting playerShooting;

        // Use this for initialization
        void Start()
        {
            text = GetComponentInChildren<Text>();
            slider = GetComponentInChildren<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerShooting != null)
            {
                //TODO: cleanup usages into properties
                slider.maxValue = playerShooting.maxAmmo + playerShooting.ammoBoost;
                slider.value = playerShooting.currentAmmo;
                text.text = string.Format("{0}/{1}", playerShooting.currentAmmo, playerShooting.maxAmmo + playerShooting.ammoBoost);
            }
        }
    }
}

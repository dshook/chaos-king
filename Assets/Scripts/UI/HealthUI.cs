using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UI
{
    public class HealthUI : MonoBehaviour
    {
        GameObject _player;
        public GameObject player
        {
            get { return _player; }
            set
            {
                _player = value;
                if (_player != null)
                {
                    playerHealth = _player.GetComponent<PlayerHealth>();
                }
                else
                {
                    playerHealth = null;
                }
            }
        }

        Text text;
        Slider slider;
        PlayerHealth playerHealth;
        Image damageImage;
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

        void Start()
        {
            text = GetComponentInChildren<Text>();
            slider = GetComponentInChildren<Slider>();
            damageImage = transform.FindChild("DamageImage").GetComponent<Image>();
        }

        void Update()
        {
            if (playerHealth != null)
            {
                if (playerHealth.damaged)
                {
                    damageImage.color = flashColour;
                }
                else
                {
                    damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
                }

                slider.maxValue = playerHealth.maxHealth;
                slider.value = playerHealth.currentHealth;
                text.text = string.Format("{0}/{1}", playerHealth.currentHealth, playerHealth.maxHealth);
            }
        }
    }
}

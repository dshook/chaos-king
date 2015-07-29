using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        public int maxHealth = 10;

        [SyncVar]
        public int currentHealth;
        public AudioClip deathClip;
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


        Slider healthSlider;
        Image damageImage;
        Animator anim;
        AudioSource playerAudio;
        PlayerMovement playerMovement;
        GameObject playerWeapon;
        bool isDead;
        bool damaged;


        void Awake ()
        {
            anim = GetComponent <Animator> ();
            playerAudio = GetComponent <AudioSource> ();
            playerMovement = GetComponent <PlayerMovement> ();
            playerWeapon = transform.FindChild("Weapon").gameObject;
            currentHealth = maxHealth;

            var healthUI = GameObject.Find("HealthUI");
            healthSlider = healthUI.GetComponentInChildren<Slider>();
            damageImage = GameObject.Find("DamageImage").GetComponent<Image>();
            
            UpdateHealthSlider();
        }


        void Update ()
        {
            if(damaged)
            {
                damageImage.color = flashColour;
            }
            else
            {
                damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
            damaged = false;
        }


        public void TakeDamage (int amount)
        {
            if (!isServer) return;

            damaged = true;

            currentHealth -= amount;

            UpdateHealthSlider();

            FloatingTextManager.PlayerDamage(amount, transform.position);

            playerAudio.Play ();

            if(currentHealth <= 0 && !isDead)
            {
                Death ();
            }
        }

        public void IncreaseHealth(int amount)
        {
            maxHealth += amount;
            currentHealth += amount;
            UpdateHealthSlider();
        }

        void UpdateHealthSlider()
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }


        void Death ()
        {
            isDead = true;

            //playerShooting.DisableEffects ();

            anim.SetTrigger ("Die");

            playerAudio.clip = deathClip;
            playerAudio.Play ();

			playerMovement.enabled = false;
			playerWeapon = transform.FindChild("Weapon").gameObject;
			playerWeapon.SetActive (false);
        }


        public void RestartLevel ()
        {
            Application.LoadLevel (Application.loadedLevel);
        }
    }
}

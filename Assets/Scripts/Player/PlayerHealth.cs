using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        public int startingHealth = 10;
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
        PlayerShooting playerShooting;
        bool isDead;
        bool damaged;


        void Awake()
        {
            anim = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
            playerMovement = GetComponent<PlayerMovement>();
            playerShooting = GetComponent<PlayerShooting>();

            var healthUI = GameObject.Find("HealthUI");
            healthSlider = healthUI.GetComponentInChildren<Slider>();
            damageImage = GameObject.Find("DamageImage").GetComponent<Image>();

            ResetHealth();
        }


        void Update()
        {
            if (damaged)
            {
                damageImage.color = flashColour;
            }
            else
            {
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
            damaged = false;
        }

        public void TakeDamage(int amount)
        {
            if (!isServer) return;

            currentHealth -= amount;

            RpcTakeDamage(amount);

            if (currentHealth <= 0 && !isDead)
            {
                Death();
            }
        }

        [ClientRpc]
        private void RpcTakeDamage(int amount)
        {
            damaged = true;
            UpdateHealthSlider();
            FloatingTextManager.PlayerDamage(amount, transform.position);

            playerAudio.Play();
        }

        public void IncreaseHealth(int amount)
        {
            maxHealth += amount;
            currentHealth += amount;
            UpdateHealthSlider();
        }

        public void ResetHealth()
        {
            maxHealth = startingHealth;
            currentHealth = maxHealth;
            UpdateHealthSlider();
        }

        void UpdateHealthSlider()
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }


        void Death()
        {
            isDead = true;

            RpcDeath();

            playerMovement.enabled = false;
            playerShooting.Disable();
        }

        [ClientRpc]
        void RpcDeath()
        {
            anim.SetTrigger("Die");

            playerAudio.clip = deathClip;
            playerAudio.Play();

            playerMovement.enabled = false;
            playerShooting.Disable();
        }

        //opposite of death of course
        public void Live()
        {
            anim.SetTrigger("Live");
            ResetHealth();

            playerMovement.enabled = true;
            playerShooting.Enable();
            
            isDead = false;
        }
    }
}

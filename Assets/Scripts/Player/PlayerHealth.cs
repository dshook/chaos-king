﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 10;
        public int currentHealth;
        public Slider healthSlider;
        public Image damageImage;
        public AudioClip deathClip;
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


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
            damaged = true;

            currentHealth -= amount;

            healthSlider.value = currentHealth;
            healthSlider.maxValue = maxHealth;

            playerAudio.Play ();

            if(currentHealth <= 0 && !isDead)
            {
                Death ();
            }
        }


        void Death ()
        {
            isDead = true;

            //playerShooting.DisableEffects ();

            anim.SetTrigger ("Die");

            playerAudio.clip = deathClip;
            playerAudio.Play ();

            playerMovement.enabled = false;
			playerWeapon.SetActive (false);
        }


        public void RestartLevel ()
        {
            Application.LoadLevel (Application.loadedLevel);
        }
    }
}

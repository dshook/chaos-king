using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Util;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        public int startingHealth = 10;
        public int maxHealth = 10;

        [SyncVar]
        public int currentHealth;
        public AudioClip deathClip;

        Animator anim;
        AudioSource playerAudio;
        PlayerMovement playerMovement;
        PlayerShooting playerShooting;
        bool isDead;
        public bool damaged;


        void Awake()
        {
            anim = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
            playerMovement = GetComponent<PlayerMovement>();
            playerShooting = GetComponent<PlayerShooting>();

            ResetHealth();
        }


        void Update()
        {
            //reset damaged to be false, note any scripts relying on this need to execute first
            //changing to a damage event is probably a good idea
            damaged = false;
        }

        public void TakeDamage(int amount)
        {
            if (!isServer) return;

            currentHealth -= amount;

            var playerDmgMsg = new PlayerDamageMessage()
            {
                player = this.gameObject,
                amount = amount
            };
            NetworkServer.SendToClient(connectionToClient.connectionId, MessageTypes.PlayerDamage, playerDmgMsg);

            if (currentHealth <= 0 && !isDead)
            {
                Death();
            }
        }

        public static void OnTakeDamage(NetworkMessage netMsg)
        {

            var msg = netMsg.ReadMessage<PlayerDamageMessage>();
            var playerHealth = msg.player.GetComponent<PlayerHealth>();

            playerHealth.ClientTakeDamage(msg.amount);
        }

        class PlayerDamageMessage : MessageBase
        {
            public GameObject player;
            public int amount;
        }

        public void ClientTakeDamage(int amount)
        {
            damaged = true;
            FloatingTextManager.PlayerDamage(amount, transform.position);

            playerAudio.Play();
        }

        public void IncreaseHealth(int amount)
        {
            maxHealth += amount;
            currentHealth += amount;
        }

        public void ResetHealth()
        {
            maxHealth = startingHealth;
            currentHealth = maxHealth;
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

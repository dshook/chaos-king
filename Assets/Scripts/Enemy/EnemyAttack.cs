using UnityEngine;
using UnityEngine.Networking;
using Player;

public class EnemyAttack : NetworkBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;

    bool playerDead = false;

    void Awake ()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerHealth = null;
            playerInRange = false;
        }
    }


    void Update ()
    {
        if (!isServer) return;

        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        if(playerHealth != null && playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
            playerDead = true;
        }

        if (playerDead && playerHealth == null)
        {
            playerDead = false;
            anim.SetTrigger("PlayerLives");
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}

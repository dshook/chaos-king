using UnityEngine;
using UnityEngine.Networking;
using Player;

public class EnemyAttack : NetworkBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;

    void Awake ()
    {
        enemyHealth = GetComponent<EnemyHealth>();
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

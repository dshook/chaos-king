using UnityEngine;
using UnityEngine.Networking;
using Player;

public class EnemyHealth : NetworkBehaviour
{
    public int startingHealth = 100;
    [SyncVar]
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public AudioClip deathClip;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    KillExperience killExperience;
    DropManager dropManager;
    bool isDead;
    bool isSinking;


    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        killExperience = GetComponent<KillExperience>();
        dropManager = FindObjectOfType<DropManager>();

        currentHealth = startingHealth;
    }


    void Update()
    {
        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage(int amount, Vector3 hitPoint, PlayerLevel player)
    {
        if (isDead)
            return;

        currentHealth -= amount;

        RpcTakeDamage(amount, hitPoint);

        if (currentHealth <= 0)
        {
            Death(player);
        }
    }

    [ClientRpc]
    private void RpcTakeDamage(int amount, Vector3 point)
    {
        FloatingTextManager.EnemyDamage(amount, point);
        enemyAudio.Play();

        hitParticles.transform.position = point;
        hitParticles.Play();
    }


    void Death(PlayerLevel player)
    {
        isDead = true;

        RpcDeath();
        capsuleCollider.isTrigger = true;

        killExperience.GiveExperience(player);
        dropManager.SpawnDrop(this.transform);
    }

    [ClientRpc]
    void RpcDeath()
    {
        anim.SetTrigger("Dead");
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
    }


    public void StartSinking()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;

        Destroy(gameObject, 2f);
    }
}

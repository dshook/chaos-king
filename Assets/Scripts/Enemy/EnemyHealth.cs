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


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        killExperience = GetComponent <KillExperience> ();
		dropManager = GameObject.FindObjectOfType<DropManager> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint, PlayerLevel player)
    {
        if(isDead)
            return;

        FloatingTextManager.EnemyDamage(amount, hitPoint);
        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death (player);
        }
    }


    void Death (PlayerLevel player)
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
        killExperience.GiveExperience(player);
		dropManager.SpawnDrop (this.transform);
    }


    public void StartSinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;

        Destroy (gameObject, 2f);
    }
}

using UnityEngine;
using UnityEngine.Networking;
using Player;

public class EnemyMovement : NetworkBehaviour
{
    public float aggroTime = 3f;

    Transform playerTransform;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    Animator anim;
    NavMeshAgent nav;

    GameObject[] players;
    float playerTimer = 0f;

    void OnUpdatePlayer() {
        FindClosestPlayer();
    }

    void Awake ()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        CustomNetManager.OnPlayerJoined += OnUpdatePlayer;
        CustomNetManager.OnPlayerLeft += OnUpdatePlayer;
    }


    void Update ()
    {
        if (!isServer) return;

        playerTimer += Time.deltaTime;
        if (playerTransform == null || playerTimer > aggroTime)
        {
            playerTimer = 0;
            FindClosestPlayer();
        }

        if (enemyHealth.currentHealth > 0 && playerHealth != null && playerHealth.currentHealth > 0)
        {
            nav.SetDestination(playerTransform.position);
        }
    }

    [Server]
    void FindClosestPlayer() {
        players = GameObject.FindGameObjectsWithTag("Player");

        float minRange = float.MaxValue;
        foreach (var p in players)
        {
            var tmpPlayerHealth = p.GetComponent<PlayerHealth>();
            if (tmpPlayerHealth.currentHealth <= 0) continue;

            var dist = Vector3.Distance(p.transform.position, transform.position);
            if (dist < minRange)
            {
                minRange = dist;
                playerTransform = p.transform;
            }
        }
        if (playerTransform != null)
        {
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }
}

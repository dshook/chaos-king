using UnityEngine;
using UnityEngine.Networking;
using Player;

public class EnemyMovement : NetworkBehaviour
{
    public float aggroTime = 10f;

    Transform playerTransform;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
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

        CustomNetManager.OnPlayerJoined += OnUpdatePlayer;
        CustomNetManager.OnPlayerJoined += OnUpdatePlayer;
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

    void FindClosestPlayer() {
        players = GameObject.FindGameObjectsWithTag("Player");

        float minRange = float.MaxValue;
        foreach (var p in players)
        {
            var dist = Vector3.Distance(p.transform.position, transform.position);
            if (dist < minRange)
            {
                minRange = dist;
                playerTransform = p.transform;
            }
        }
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
    }
}

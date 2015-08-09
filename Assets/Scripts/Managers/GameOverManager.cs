using UnityEngine;
using UnityEngine.Networking;
using Player;

public class GameOverManager : NetworkBehaviour
{
    //both of these set via script when the player spawns
    public Animator anim;
    public CustomNetManager netManager;

    public float restartDelay = 5f;

    PlayerHealth playerHealth;
    float restartTimer;
    bool dead = false;


    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }


    void Update()
    {
        if (!isServer) return;

        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            if (!dead)
            {
                RpcAnimate();
                dead = true;
            }

            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                playerHealth.Live();
                dead = false;
                var spawnPoint = netManager.spawnPoints[Random.Range(0, netManager.spawnPoints.Length)];
                SetPlayerPosition(spawnPoint.position);
                RpcRespawn(spawnPoint.position);
            }
        }
    }

    [ClientRpc]
    void RpcAnimate()
    {
        //for now anim will be null on clients who don't control this player
        //so they should not see the game over fade in text
        if (anim != null)
        {
            anim.SetTrigger("GameOver");
        }
    }

    [ClientRpc]
    void RpcRespawn(Vector3 newPosition)
    {
        SetPlayerPosition(newPosition);
        playerHealth.Live();
        if (anim != null)
        {
            anim.SetTrigger("Respawn");
        }
    }

    void SetPlayerPosition(Vector3 newPosition)
    {
        playerHealth.transform.position = newPosition;

    }
}

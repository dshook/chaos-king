using UnityEngine;
using UnityEngine.Networking;
using Player;

public class PlayerRespawn : NetworkBehaviour
{
    //both of these set via script when the player spawns
    public Animator anim;
    public GameSetup gameSetup;

    public float restartDelay = 5f;

    PlayerHealth playerHealth;
    PlayerLevel playerLevel;
    PlayerPerks playerPerks;
    PlayerShooting playerShooting;
    PlayerMovement playerMovement;
    float restartTimer;
    bool dead = false;


    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
        playerPerks = GetComponent<PlayerPerks>();
        playerShooting = GetComponent<PlayerShooting>();
        playerMovement = GetComponent<PlayerMovement>();
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
                playerLevel.Respawn();
                playerPerks.ResetPerks();
                playerShooting.ResetMultipliers();
                playerMovement.ResetSpeed();
                playerHealth.Live();
                dead = false;
                var spawnPoint = gameSetup.GetNextSpawnpoint();
                SetPlayerPosition(spawnPoint);
                RpcRespawn(spawnPoint);
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

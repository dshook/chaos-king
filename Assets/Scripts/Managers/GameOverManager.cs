using UnityEngine;
using UnityEngine.Networking;
using Player;

public class GameOverManager : NetworkBehaviour
{
    public PlayerHealth playerHealth;
    public float restartDelay = 5f;

    Animator anim;
    float restartTimer;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");

            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                RpcRespawn();
            }
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {

    }
}

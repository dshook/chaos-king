using UnityEngine;
using UnityEngine.Networking;
using Player;
using System.Collections;

public class FlameProjectile : NetworkBehaviour
{

    public int damage = 0;
    public float speed = 10f;
    public float range = 1f;
    public PlayerLevel playerLevel;

    float distanceTraveled = 0f;
    public int canPierce = 10;
    int enemiesPierced = 0;

    [SyncVar(hook ="OnEnableChanged")]
    bool isEnabled = false;

    void Update()
    {
        if (!isServer) return;

        if (enemiesPierced >= canPierce || distanceTraveled >= range && isEnabled)
        {
            Disable();
        }
        else
        {
            Vector3 oldPosition = transform.position;
            transform.position += transform.forward * (speed * Time.deltaTime);
            distanceTraveled += Vector3.Distance(oldPosition, transform.position);
        }
    }

    public void Disable()
    {
        enabled = false;
        isEnabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Light>().enabled = false;
        GetComponent<ParticleSystem>().Stop();
        GetComponent<ParticleSystem>().Clear();

    }

    public void Enable()
    {
        enabled = true;
        isEnabled = true;
        distanceTraveled = 0;
        enemiesPierced = 0;
        StartCoroutine(DelayedPlay());
    }

    IEnumerator DelayedPlay()
    {
        yield return new WaitForFixedUpdate();

        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Light>().enabled = true;
        GetComponent<ParticleSystem>().Play();
    }

    [Client]
    void OnEnableChanged(bool newEnabled)
    {
        if (newEnabled)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;

        if (other.gameObject.tag == "Enemy")
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, transform.position, playerLevel);
            }
            enemiesPierced++;
        }
    }
}

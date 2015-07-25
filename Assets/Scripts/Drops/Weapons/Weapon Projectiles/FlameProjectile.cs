using UnityEngine;
using System.Collections;
using Player;

public class FlameProjectile : MonoBehaviour {

    public int damage = 0;
    public float speed = 10f;
    public float range = 1f;
    float distanceTraveled = 0f;
    public int canPierce = 10;
    int enemiesPierced = 0;
    GameObject player;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	void Update () {
        if(enemiesPierced >= canPierce || distanceTraveled >= range)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 oldPosition = transform.position;
            transform.position += transform.forward * (speed * Time.deltaTime);
            distanceTraveled += Vector3.Distance(oldPosition, transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, transform.position, player.GetComponent<PlayerLevel>());
            }
            enemiesPierced++;
        }
    }
}

using UnityEngine;
using Player;
using UnityEngine.Networking;

public class EnemyManager : NetworkBehaviour
{
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;


    public override void OnStartServer ()
    {
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        var newEnemy = (GameObject)Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        newEnemy.transform.parent = gameObject.transform;

        NetworkServer.Spawn(newEnemy);
    }
}

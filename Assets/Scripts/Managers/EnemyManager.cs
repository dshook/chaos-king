using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class EnemyManager : NetworkBehaviour
{
    public GameObject enemy;
    public float spawnTime = 3f;
    public int maxEnemies = 15;
    public Transform[] spawnPoints;

    List<GameObject> spawnedEnemies = new List<GameObject>();

    public override void OnStartServer ()
    {
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        spawnedEnemies = spawnedEnemies.Where(x => x != null).ToList();
        if (spawnedEnemies.Count >= maxEnemies) return;

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        var newEnemy = (GameObject)Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        newEnemy.transform.parent = gameObject.transform;

        spawnedEnemies.Add(newEnemy);

        NetworkServer.Spawn(newEnemy);
    }
}

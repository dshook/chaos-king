using UnityEngine;
using UnityEngine.Networking;

public class DropManager : NetworkBehaviour
{
    public GameObject[] dropList;
    public float dropChance = 0.2f;

    public void SpawnDrop(Transform spawnPoint)
    {
        if (dropList.Length == 0)
        {
            return;
        }

        if (Random.Range(0, 1f) < dropChance)
        {
            int dropIndex = Random.Range(0, dropList.Length);

            var obj = (GameObject)Instantiate(dropList[dropIndex], spawnPoint.position, spawnPoint.rotation);
            NetworkServer.Spawn(obj);
        }
    }
}

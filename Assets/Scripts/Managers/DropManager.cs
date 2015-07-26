using UnityEngine;
using Player;

public class DropManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject[] dropList;
    public float dropChance = 0.2f;

    public void SpawnDrop(Transform spawnPoint)
    {
        if (playerHealth.currentHealth <= 0f || dropList.Length == 0)
        {
            return;
        }

        if (Random.Range(0, 1f) < dropChance)
        {
            int dropIndex = Random.Range(0, dropList.Length);

            Instantiate(dropList[dropIndex], spawnPoint.position, spawnPoint.rotation);
        }
    }
}

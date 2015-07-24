using UnityEngine;
using Player;

public class DropManager : MonoBehaviour
{
	public PlayerHealth playerHealth;
	public GameObject[] dropList;

	public void SpawnDrop (Transform spawnPoint)
	{
		if(playerHealth.currentHealth <= 0f || dropList.Length == 0)
		{
			return;
		}
		
		int dropIndex = Random.Range (0, dropList.Length);
		
		Instantiate (dropList[dropIndex], spawnPoint.position, spawnPoint.rotation);
	}
}

using UnityEngine;
using System.Collections;
using Player;

public class KillTimer : MonoBehaviour
{
    public float killTime = 10f;
    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > killTime)
        {
            Destroy(gameObject);
        }
    }
}

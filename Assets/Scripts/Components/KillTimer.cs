using UnityEngine;
using System.Collections;
using Player;

public class KillTimer : MonoBehaviour
{
    public float killTime = 10f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, killTime);
    }
}

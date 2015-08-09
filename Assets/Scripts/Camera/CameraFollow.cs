using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour
{
    public float smoothing = 5f;

    Transform cameraTransform;
    Vector3 offset = new Vector3(1, 15, -22);

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        Vector3 targetCamPos = transform.position + offset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

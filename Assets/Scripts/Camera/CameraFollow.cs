using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour
{
    public float smoothing = 5f;

    Vector3 offset;
    Transform cameraTransform;

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        offset = cameraTransform.position - transform.position;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        Vector3 targetCamPos = transform.position + offset;
        cameraTransform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

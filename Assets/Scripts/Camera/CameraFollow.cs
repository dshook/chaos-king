using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour
{
    public float smoothing = 5f;

    Vector3 offset;
    Transform cameraTransform;
    Vector3 cameraOffset = new Vector3(1, 15, -22);

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        offset = cameraOffset - transform.position;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        Vector3 targetCamPos = transform.position + offset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

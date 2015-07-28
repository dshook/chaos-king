using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class Zoom : NetworkBehaviour
{
    public float zoomSpeed = 10f;
    public float minDistance = 2.5f;
    public float maxDistance = 6f;

    Camera cam;
    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer) return;

        float scroll = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            var newSize = cam.orthographicSize - (scroll * zoomSpeed);

            newSize = Mathf.Clamp(newSize, minDistance, maxDistance);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newSize, zoomSpeed * Time.deltaTime);

        }

    }
}

using UnityEngine;
using UnityEngine.Networking;

public class SyncLinePosition : NetworkBehaviour
{
    [SyncVar(hook = "OnStartSynced")]
    private Vector3 start;

    [SyncVar(hook = "OnEndSynced")]
    private Vector3 end;

    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetStart(Vector3 newStart)
    {
        start = newStart;
        lineRenderer.SetPosition(0, start);
    }

    public void SetEnd(Vector3 newEnd)
    {
        end = newEnd;
        lineRenderer.SetPosition(1, end);
    }

    [Client]
    void OnStartSynced(Vector3 v)
    {
        lineRenderer.SetPosition(0, v);
    }

    [Client]
    void OnEndSynced(Vector3 v)
    {
        lineRenderer.SetPosition(1, v);
    }
}

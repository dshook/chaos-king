using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class SyncTransform : NetworkBehaviour
{
    [SyncVar(hook = "OnRotSynced")]
    private float syncPlayerRotation;

    [SyncVar(hook = "SyncPositionValues")]
    private Vector3 syncPos;

    [SerializeField]
    public Transform myTransform;

    [SerializeField]
    private bool useHistoricalLerping = false;
    [SerializeField]
    private bool syncRotation = false;

    private float lerpRate;
    private float normalLerpRate = 16;
    private float fasterLerpRate = 27;

    private Vector3 lastPos;
    private float lastRot;
    private float threshold = 0.5f;
    private float rotationThreshold = 0.001f;

    private List<Vector3> syncPosList = new List<Vector3>();
    private List<float> syncPlayerRotList = new List<float>();
    private float closeEnough = 0.11f;

    void Start()
    {
        lerpRate = normalLerpRate;
    }

    void Update()
    {
        LerpPosition();
        if (syncRotation)
        {
            LerpRotations();
        }
    }

    void FixedUpdate()
    {
        TransmitPosition();
        if (syncRotation)
        {
            TransmitRotation();
        }
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            if (useHistoricalLerping)
            {
                HistoricalLerping();
            }
            else
            {
                OrdinaryLerping();
            }
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
        {
            CmdProvidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
    }

    [Client]
    void SyncPositionValues(Vector3 latestPos)
    {
        syncPos = latestPos;
        syncPosList.Add(syncPos);
    }

    void OrdinaryLerping()
    {
        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
    }

    void HistoricalLerping()
    {
        if (syncPosList.Count > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);

            if (Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
            {
                syncPosList.RemoveAt(0);
            }

            if (syncPosList.Count > 10)
            {
                lerpRate = fasterLerpRate;
            }
            else
            {
                lerpRate = normalLerpRate;
            }
        }
    }


    void LerpRotations()
    {
        if (!isLocalPlayer)
        {
            if (useHistoricalLerping)
            {
                HistoricalRotationInterpolation();
            }
            else
            {
                OrdinaryRotationLerping();
            }
        }
    }

    void HistoricalRotationInterpolation()
    {
        if (syncPlayerRotList.Count > 0)
        {
            LerpRotation(syncPlayerRotList[0]);

            if (Mathf.Abs(myTransform.localEulerAngles.y - syncPlayerRotList[0]) < closeEnough)
            {
                syncPlayerRotList.RemoveAt(0);
            }

        }
    }

    void OrdinaryRotationLerping()
    {
        LerpRotation(syncPlayerRotation);
    }

    void LerpRotation(float rotAngle)
    {
        Vector3 playerNewRot = new Vector3(0, rotAngle, 0);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(playerNewRot), lerpRate * Time.deltaTime);
    }

    [Command]
    void CmdProvideRotationToServer(float playerRot)
    {
        syncPlayerRotation = playerRot;
    }

    [Client]
    void TransmitRotation()
    {
        if (isClient)
        {
            if (CheckIfBeyondThreshold(myTransform.localEulerAngles.y, lastRot))
            {
                lastRot = myTransform.localEulerAngles.y;
                CmdProvideRotationToServer(lastRot);
            }
        }
    }

    bool CheckIfBeyondThreshold(float rot1, float rot2)
    {
        if (Mathf.Abs(rot1 - rot2) > rotationThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [Client]
    void OnRotSynced(float latestPlayerRot)
    {
        syncPlayerRotation = latestPlayerRot;
        syncPlayerRotList.Add(syncPlayerRotation);
    }
}

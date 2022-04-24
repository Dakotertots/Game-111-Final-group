using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrackerYToggle : MonoBehaviour
{
    private Transform localTrans,parentTrans;
    private bool bTrackY;

    private void Awake()
    {
        localTrans = transform;
        parentTrans = localTrans.parent.transform;
        localTrans.parent = null;
        CuboidMaster.DelResetGame += ResetObject;
    }
    private void OnEnable()
    {
        CuboidMaster.DelEnablePlayerMovement += RoundY;
        CuboidMaster.DelDisablePlayerMovement += TrackY;
    }
    private void OnDisable()
    {
        CuboidMaster.DelEnablePlayerMovement -= RoundY;
        CuboidMaster.DelDisablePlayerMovement -= TrackY;
        CuboidMaster.DelResetGame -= ResetObject;
    }
    private void ResetObject()
    {
        bTrackY = false;
    }
    private void TrackY() { bTrackY = true; }
    private void RoundY() { bTrackY = false; }
    private void LateUpdate()
    {
        if (bTrackY)
            transform.position = parentTrans.position;
        else
            transform.position = new Vector3(parentTrans.position.x,
                                             Mathf.RoundToInt(parentTrans.position.y),
                                             parentTrans.position.z);
    }
}

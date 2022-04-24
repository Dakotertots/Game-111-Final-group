using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGame : MonoBehaviour
{
    public Transform cubeTracker,virtualCamera,mainCamera;

    private void Start()
    {
        cubeTracker.parent = null;
        virtualCamera.transform.parent = null;
        virtualCamera.transform.localRotation = Quaternion.Euler(45, 0, 0);
        mainCamera.transform.parent = null;
    }
}

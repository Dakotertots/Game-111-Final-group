using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCubeGA : GameActions
{
    public bool bStay;
    public Rigidbody rBody;
    public MeshRenderer mRenderer;
    private Vector3 origPosition;
    private Quaternion origRotation;

    private void Awake()
    {
        origPosition = rBody.transform.position;
        origRotation = rBody.transform.rotation;
        CuboidMaster.DelResetGame += ResetObject;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetObject;
    }
    private void ResetObject()
    {
        rBody.transform.position = origPosition;
        rBody.transform.rotation = origRotation;
        mRenderer.enabled = true;
    }
    public override void DeAction()
    {
        rBody.isKinematic = false;
        rBody.useGravity = true;
        if (bStay) return;
        StartCoroutine(nameof(Fall));
    }
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(2);
        rBody.isKinematic = true;
        rBody.useGravity = false;
        mRenderer.enabled = false;
    }
}

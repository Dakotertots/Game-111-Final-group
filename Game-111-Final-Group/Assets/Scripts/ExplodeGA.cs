using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeGA : GameActions
{
    public MeshRenderer origMesh;
    public List<Rigidbody> pieces;
    public bool bCheckpoint,bPlayer;
    private Transform parentTrans;
    private Vector3 parentOrigPos;

    private void Awake()
    {        
        CubeMovement.ReturnToCheckpointComplete += CheckpointComplete;
        CuboidMaster.DelReturnToCheckpoint += ResetObject;
        CuboidMaster.DelResetGame += ResetObject;
        parentTrans = transform.parent;
        parentOrigPos = parentTrans.position;
    }
    private void OnDisable()
    {
        CuboidMaster.DelReturnToCheckpoint -= ResetObject;
        CuboidMaster.DelResetGame -= ResetObject;
        CubeMovement.ReturnToCheckpointComplete -= CheckpointComplete;
    }   
    public override void Action()
    {       
        origMesh.enabled = false;
        transform.parent = null;
        foreach(Rigidbody item in pieces)
        {
            item.gameObject.SetActive(true);
            item.isKinematic = false;
            item.AddExplosionForce(3, transform.position, 1.5f,2,ForceMode.Impulse);
        }
        if (bPlayer)
        {
            CuboidMaster.DisablePlayerMovement();
            StartCoroutine(nameof(ResetToCheckpoint));
        }
    }
    private void ResetObject()
    {
        origMesh.enabled = true;
        transform.parent = parentTrans;
        transform.localPosition = Vector3.zero;
        foreach(Rigidbody item in pieces)
        {
            item.gameObject.SetActive(false);
            item.isKinematic = false;
            item.transform.localPosition = Vector3.zero;
        }
        if (!bPlayer)
            parentTrans.position = parentOrigPos;
    }
    private void CheckpointComplete()
    {
        bCheckpoint = true;
    }
    IEnumerator ResetToCheckpoint()
    {
        yield return new WaitForSeconds(1);
        ResetObject();
        CuboidMaster.ReturnToCheckpoint();
        while (!bCheckpoint)
            yield return new WaitForEndOfFrame();
        CuboidMaster.EnablePlayerMovement();
        bCheckpoint = false;
    }
}

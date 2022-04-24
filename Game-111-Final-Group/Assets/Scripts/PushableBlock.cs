using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    public float speed = 1;
    public AnimationCurve animCurve;
    public AudioSource aSource;
    public LayerMask mask;
    public LockPushable pushableLock;
    private Collider localCollider;
    private Vector3 direction;
    private bool bMove;   
    private Vector3 desiredPos, startPos, movingPosition, origPosition,checkpointPosition;
    private float rate;

    private void Awake()
    {
        localCollider = GetComponent<Collider>();
        origPosition = transform.position;
        checkpointPosition = transform.position;
        CuboidMaster.DelResetGame += ResetObject;
        CuboidMaster.DelCheckpoint += Checkpoint;
        CuboidMaster.DelReturnToCheckpoint += ReturnToCheckpoint;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetObject;
        CuboidMaster.DelCheckpoint -= Checkpoint;
        CuboidMaster.DelReturnToCheckpoint -= ReturnToCheckpoint;
    }
    private void Checkpoint(Vector3 value)
    {
        checkpointPosition = transform.position;
        pushableLock.UnlockLock();
    }
    private void ReturnToCheckpoint()
    {
        StopAllCoroutines();
        transform.position = checkpointPosition;
        bMove = false;
        pushableLock.UnlockLock();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            direction = Vector3.Normalize(new Vector3(transform.position.x, 0, transform.position.z) -
                                          new Vector3(other.transform.position.x, 0, other.transform.position.z));
            if (IsValid(direction))
            {
                if (bMove) return;
                StartCoroutine(nameof(MoveCube));
            }
        }
        else if (other.CompareTag("Trigger"))
        {            
            pushableLock.LockBlock();
            StartCoroutine(nameof(TemporaryLock));
        }
    }
    private bool IsValid(Vector3 rayDirection)
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(transform.position, rayDirection,out hitinfo, 0.75f, mask))
        {
            return false;
        }
        else if (Physics.Raycast(transform.position + rayDirection, -Vector3.up,out hitinfo, 0.75f))
        {            
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool MoveInDirection(Vector3 desiredDirection)
    {
        if (pushableLock.IsLocked) return false;
        if (IsValid(desiredDirection))
        {
            direction = desiredDirection;
            StartCoroutine(nameof(MoveCube));
            return true;
        }
        return false;
    }
    private void SetRotation()
    {
        //input direction
        if (direction.y > 0.5f && IsValid(Vector3.forward))
            direction = Vector3.forward;
        else if (direction.y < -0.5f && IsValid(-Vector3.forward))
            direction = -Vector3.forward;
        else if (direction.x > 0.5f && IsValid(Vector3.right))
            direction = Vector3.right;
        else if (direction.x < -0.5f && IsValid(-Vector3.right))
            direction = -Vector3.right;
    }
    private void ResetObject()
    {
        //aStopAllCoroutines();
        transform.position = origPosition;
        bMove = false;
        Debug.Log("Erwar");
        pushableLock.UnlockLock();
    }
    IEnumerator MoveCube()
    {
        SetRotation();
        bMove = true;
        aSource.PlayOneShot(aSource.clip);
        startPos = transform.position;
        desiredPos = transform.position + direction;       
        rate = 0;

        while (bMove)
        {
            yield return new WaitForEndOfFrame();
            rate += Time.deltaTime * speed;
            movingPosition = Vector3.Lerp(startPos, desiredPos, rate);
            transform.position = movingPosition;
            if (rate >= 1)
            {
                transform.position = desiredPos;
                rate = 0;
                bMove = false;
            }
        }
        pushableLock.MovementCheck();
    }
    IEnumerator TemporaryLock()
    {
        yield return new WaitForSeconds(0.25f);
        pushableLock.UnlockLock();
    }
}

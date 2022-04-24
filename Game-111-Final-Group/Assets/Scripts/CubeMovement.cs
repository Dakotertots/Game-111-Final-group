using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CubeMovement : MonoBehaviour
{    
    public float speed =  1;
    public bool bMove,bMoving;
    public AnimationCurve animCurve;
    public AudioSource aSource;
    public bool bInputRef;
    public LayerMask mask;
    private PlayerInput pInput;
    private Quaternion desiredRot,startingRot;
    private Vector3 desiredPos, startPos,movingPosition,rotationDirection,moveDirection;
    private float rate,movementDelay;
    private Vector2 controlInput;
    private Vector3 origPosition,checkpoint;
    private Quaternion origRotation;
    public static Action<Transform> SendTransform = delegate { };
    private Rigidbody rBody;
    private RaycastHit hitInfo;
    private static bool bInput;
    private PushableBlock pBlock;
    private bool bMoveDelay;

    public static Action ReturnToCheckpointComplete = delegate { };
    private void Awake()
    {
        bInput = true;
        bInputRef = bInput;
        pInput = new PlayerInput();
        pInput.Enable();       
        MoveToGA.PlayerTransform += SendPlayerTransform;
        checkpoint = transform.position;
        origPosition = transform.position;
        origRotation = transform.rotation;
        rBody = GetComponent<Rigidbody>();
        CuboidMaster.DelEnablePlayerMovement += EnableInput;
        CuboidMaster.DelDisablePlayerMovement += DisableInput;
        CuboidMaster.DelResetGame += ResetPlayer;
        CuboidMaster.DelCheckpoint += Checkpoint;
        CuboidMaster.DelReturnToCheckpoint += ReturnToCheckpoint;
    }
    private void OnDisable()
    {
        CuboidMaster.DelEnablePlayerMovement -= EnableInput;
        CuboidMaster.DelDisablePlayerMovement -= DisableInput;
        MoveToGA.PlayerTransform -= SendPlayerTransform;
        CuboidMaster.DelResetGame -= ResetPlayer;
        CuboidMaster.DelCheckpoint -= Checkpoint;
        CuboidMaster.DelReturnToCheckpoint -= ReturnToCheckpoint;
    }
    private void Update()
    {
        if (!bMove && !bMoving && bInput)
            GetInput();

        if (!bInput)
        {
            if (bMoving)
                Moving();
            return;
        }
        if (bMove)
        {
            aSource.PlayOneShot(aSource.clip);
            startingRot = Quaternion.Euler(Vector3.zero);
            desiredRot = startingRot * Quaternion.Euler(rotationDirection);

            startPos = transform.position;
            desiredPos = transform.position + moveDirection;

            bMove = false;
            rate = 0;
            bMoving = true;            
        }

        if (bMoving)
            Moving();
    }
    private void GetInput()
    {
        if (bMoveDelay)
        {            
            movementDelay += Time.deltaTime;
            if (movementDelay >= 0.025f)
                bMoveDelay = false;
            return;
        }
        controlInput = pInput.Player.Movement.ReadValue<Vector2>();
        if (controlInput.magnitude == 0) return;

        //input direction
        if (controlInput.y > 0.5f && IsValid(Vector3.forward))
        {
            if (pBlock)
            {
                if (!pBlock.MoveInDirection(Vector3.forward))
                    return;
            }
            moveDirection = Vector3.forward;
            rotationDirection = new Vector3(90, 0, 0);
            bMove = true;
        }
        else if (controlInput.y < -0.5f && IsValid(-Vector3.forward))
        {
            if (pBlock)
            {
                if (!pBlock.MoveInDirection(-Vector3.forward))
                    return;
            }
            moveDirection = -Vector3.forward;
            rotationDirection = new Vector3(-90, 0, 0);
            bMove = true;
        }
        else if (controlInput.x > 0.5f && IsValid(Vector3.right))
        {
            if (pBlock)
            {
                if (!pBlock.MoveInDirection(Vector3.right))
                    return;
            }
            moveDirection = Vector3.right;
            rotationDirection = new Vector3(0, 0, -90);
            bMove = true;
        }
        else if (controlInput.x < -0.5f && IsValid(-Vector3.right))
        {
            if (pBlock)
            {
                if (!pBlock.MoveInDirection(-Vector3.right))
                    return;
            }
            moveDirection = -Vector3.right;
            rotationDirection = new Vector3(0, 0, 90);
            bMove = true;
        }
    }
    private void Moving()
    {
        rate += Time.deltaTime * speed;
        transform.rotation = Quaternion.Lerp(startingRot, desiredRot, rate);
        movingPosition = Vector3.Lerp(startPos, desiredPos, rate);
        movingPosition.y = startPos.y + animCurve.Evaluate(rate);
        transform.position = movingPosition;
        if (rate >= 1)
        {
            transform.position = desiredPos;
            bMoving = false;
            movementDelay = 0;
            bMoveDelay = true;
        }
    }
    private void FloorCheck()
    {
        if(Physics.Raycast(transform.position,-Vector3.up,0.51f))
        {
            rBody.isKinematic = true;
            rBody.useGravity = false;
        }
        else
        {
            rBody.isKinematic = false;
            rBody.useGravity = true;
        }
    }
    private bool IsValid(Vector3 rayDirection)
    {
        pBlock = null;
        if (Physics.Raycast(transform.position, rayDirection, out hitInfo, 0.75f, mask))
        {
            if (hitInfo.transform.TryGetComponent(out PushableBlock value))
            {
                pBlock = value;
                return true;
            }
           /* if (hitInfo.transform.CompareTag("Trigger"))
                return true;
            else*/
                return false;
        }
        else if (Physics.Raycast(transform.position + rayDirection, -Vector3.up, out hitInfo, 0.75f))
        {
            if(hitInfo.transform.TryGetComponent(out PushableBlock value))
                pBlock = value;
            return true;
        }
        else
            return false;
    }
    private void EnableInput()
    {
        controlInput = Vector2.zero;
        bInput = true;
        bInputRef = bInput;
    }
    private void DisableInput()
    {
        bInput = false;
        bInputRef = bInput;
    }
    public static bool GetState{ get{ return bInput; } }   
    private void SendPlayerTransform()
    {
        SendTransform(transform);
    }
    private void ResetPlayer()
    {
        transform.position = origPosition;
        transform.rotation = origRotation;
        bInput = true;
    }
    private void Checkpoint(Vector3 value)
    {
        checkpoint = value;
    }
    private void ReturnToCheckpoint()
    {
        transform.position = checkpoint;
        ReturnToCheckpointComplete();
    }
}
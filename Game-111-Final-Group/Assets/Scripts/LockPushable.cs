using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockPushable : MonoBehaviour
{
    public LayerMask mask;
    public PushableBlock pBlock;
    public GameObject trigger;
    private Vector3 direction;
    private PlayerInput pInput;
    private bool bLock;
    
    private void OnEnable()
    {        
        CuboidMaster.DelResetGame += ResetObject;
        CuboidMaster.DelCheckpoint += ResetObject;
        pInput = new PlayerInput();
        pInput.Enable();
    }
    private void OnDisable()
    {
        pInput.Disable();
        CuboidMaster.DelResetGame -= ResetObject;
        CuboidMaster.DelCheckpoint -= ResetObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            pInput.Player.Movement.performed += ChangeState;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            pInput.Player.Movement.performed -= ChangeState;
    }
    private void ChangeState(InputAction.CallbackContext c)
    {
        ValidateMove();
    }
    private void ValidateMove()
    {
        if (bLock) return;
        direction = new Vector3(pInput.Player.Movement.ReadValue<Vector2>().x, 0,
                                pInput.Player.Movement.ReadValue<Vector2>().y);
        if (Physics.Raycast(transform.position,direction , 0.75f, mask))
            trigger.layer = 0;
        else if (Physics.Raycast(transform.position + direction, -Vector3.up, 0.75f))
            trigger.layer = 7;
        else
            trigger.layer = 0;
    }
    public void MovementCheck()
    {
        ValidateMove();
    }
    public void LockBlock()
    {
        bLock = true;
        trigger.layer = 0;
    }
    public void UnlockLock()
    {
        bLock = false;
        trigger.layer = 7;
    }
    private void ResetObject()
    {
        trigger.layer = 7;
        bLock = false;
    }
    private void ResetObject(Vector3 value)
    {
        trigger.layer = 7;
    }
    public bool IsLocked { get { return bLock; } }
}
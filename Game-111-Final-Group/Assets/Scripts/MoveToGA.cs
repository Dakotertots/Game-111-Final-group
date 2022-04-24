using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveToGA : GameActions
{
    public float speed = 1;
    public Transform localTransform,positionA,positionB;
    public PlayerInputToggleGA playerInput;
    private bool bMoving, bMoveTo;
    public Transform objectTrans;
    public static Action PlayerTransform = delegate { };
    private Vector3 origPosition;

    private void Awake()
    {
        CuboidMaster.DelResetGame += ResetObject;
        bMoveTo = true;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        objectTrans = other.transform;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            CuboidMaster.EnablePlayerMovement();
        StopAllCoroutines();
        objectTrans = null;
    }
    private void ResetObject()
    {
        StopAllCoroutines();
        bMoveTo = true;
        bMoving = false;
        localTransform.position = positionA.position;
    }
    public override void Action()
    {
        if (bMoving) return;
        if(objectTrans)
            StartCoroutine(nameof(Move));
    }
    public override void DeAction()
    {
        if (bMoving) return;
        if (objectTrans)
            StartCoroutine(nameof(Move));
    }
    IEnumerator Move()
    {
        objectTrans.parent = localTransform;

        bMoving = true;
        float rate = 0;
        Vector3 startPos,destination;

        if(bMoveTo)
        {
            startPos = positionA.position;
            destination = positionB.position;
            bMoveTo = false;
        }
        else
        {
            startPos = positionB.position;
            destination = positionA.position;
            bMoveTo = true;
        }
        while (rate < 1)
        {
            rate += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
            localTransform.position = Vector3.Lerp(startPos, destination, rate);
        }
        CuboidMaster.EnablePlayerMovement();

        if(objectTrans)
            objectTrans.parent = null;
        bMoving = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualBlock : MonoBehaviour
{
    public float speed = 1;
    public float pauseTime = 2;
    public PlayerInputToggleGA playerInput;
    public LockAndParentGA lockParent;
    public Collider trigger;

    private List<Transform> wayPoints;
    private float rate, distance,internalTimer;
    private int startIndex, targetIndex;
    private bool bHold;
    private Collider localCollider;

    private void Awake()
    {
        wayPoints = new List<Transform>();
        for (int x = 0; x < transform.childCount; x++)
        {
            if(!transform.GetChild(x).GetComponent<Collider>())
                wayPoints.Add(transform.GetChild(x));
        }

        if(wayPoints.Count >= 2)
        {
            startIndex = 0;
            targetIndex = 1;
            distance = Vector3.Distance(wayPoints[startIndex].position, wayPoints[targetIndex].position);
        }

        //unparent wapoints
        foreach (Transform item in wayPoints)
            item.parent = null;

        localCollider = GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        if(bHold)
        {            
            if (lockParent.HasPlayer)
            {
                CuboidMaster.EnablePlayerMovement();
                lockParent.Unlock();
            }
            else
            {
                trigger.enabled = true;
                localCollider.enabled = true;
            }
            internalTimer += Time.deltaTime;
            if (internalTimer >= pauseTime)
            {
                bHold = false;
                lockParent.Lock();
            }
            return;
        }
        if (wayPoints.Count > 1)
        {
            if (lockParent.HasPlayer)
            {
                CuboidMaster.DisablePlayerMovement();
                lockParent.Lock();
            }
            else
            {
                trigger.enabled = false;
                localCollider.enabled = false;
            }
            rate = Mathf.Clamp(rate + (Time.deltaTime *speed) * distance,0,1);
            transform.position = Vector3.Lerp(wayPoints[startIndex].position, wayPoints[targetIndex].position,rate);
            
            if(rate == 1)
            {
                bHold = true;
                internalTimer = 0;
                rate = 0;
                targetIndex++;
                startIndex++;

                if (targetIndex >= wayPoints.Count)
                {
                    targetIndex = 0;
                    startIndex = wayPoints.Count - 1;
                }
                else if (targetIndex == 1)
                    startIndex = 0;
            }
        }
    }
}

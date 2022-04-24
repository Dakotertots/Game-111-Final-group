using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBlockGA : GameActions
{
    public TeleporterBlockGA destination;
    public GameActions playSound;
    private bool bActive,bPlayer,bTeleporting;
    private Transform objectToTeleport;
    
    public override void Action()
    {       
        if (bActive || bTeleporting) return;
        if (destination.IsOccupied) return;
        bActive = true;
        playSound.Action();
        StartCoroutine(nameof(TeleportSequence));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (bTeleporting) return;
        if (other.CompareTag("Player"))
            bPlayer = true;
        objectToTeleport = other.transform;       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            bPlayer = false;
        bActive = false;
    }
    public Vector3 Position { get { return transform.position + new Vector3(0, 1, 0); } }
    public void MakeActive() { bActive = true; }
    public bool IsOccupied { get{ return bActive; } }
    private void OnDrawGizmos()
    {
        if(destination)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, destination.Position);
        }
    }
    IEnumerator TeleportSequence()
    {
        bTeleporting = true;
        if(bPlayer)
            CuboidMaster.DisablePlayerMovement();
        destination.MakeActive();
        yield return new WaitForSeconds(0.1f);
        float rate = 0;
        while(rate < 1)
        {
            rate = Mathf.Clamp(rate + Time.deltaTime * 2,0,1);
            objectToTeleport.localScale = Vector3.Lerp(new Vector3(1,1,1), Vector3.zero, rate);
            yield return new WaitForEndOfFrame();
        }
        objectToTeleport.position = destination.Position;
        rate = 0;
        while (rate < 1)
        {
            rate = Mathf.Clamp(rate + Time.deltaTime * 2, 0, 1);
            objectToTeleport.localScale = Vector3.Lerp(Vector3.zero,new Vector3(1, 1, 1), rate);
            yield return new WaitForEndOfFrame();
        }
        CuboidMaster.EnablePlayerMovement();
        bTeleporting =false;
    }
}

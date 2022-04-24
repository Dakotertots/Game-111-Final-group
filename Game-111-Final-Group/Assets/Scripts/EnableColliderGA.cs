using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnableColliderGA : GameActions
{
    public bool bInverse;
    private Collider localCollider;

    private void Awake()
    {
        localCollider = GetComponent<Collider>();
    }
    public override void Action()
    {
        if (bInverse)
            localCollider.enabled = false;
        else
            localCollider.enabled = true;
    }
    public override void DeAction()
    {
        if (bInverse)
            localCollider.enabled = true;
        else
            localCollider.enabled = false;
    }
}

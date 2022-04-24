using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderToggleGA : GameActions
{
    public bool bDisable;
    public Collider localCollider;

    private void OnEnable()
    {
        CuboidMaster.DelResetGame += ResetToDefaults;
        CuboidMaster.DelReturnToCheckpoint += ResetToDefaults;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetToDefaults;
        CuboidMaster.DelReturnToCheckpoint -= ResetToDefaults;
    }
    public override void Action()
    {
        if (bDisable)
            localCollider.enabled = false;
        else if (localCollider.enabled)
            localCollider.enabled = false;
        else
            localCollider.enabled = true;
    }
    public override void ResetToDefaults()
    {
        localCollider.enabled = true;
    }
}

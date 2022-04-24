using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointGA : GameActions
{
    public Transform checkpointPos;
    public override void Action()
    {
        CuboidMaster.Checkpoint(checkpointPos.position);
    }
}

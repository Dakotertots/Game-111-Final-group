using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WinGA : GameActions
{
    public static Action Goal = delegate { };
    public override void Action()
    {
        Goal();
    }
}

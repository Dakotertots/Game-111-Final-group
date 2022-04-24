using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    public float delay;
    public virtual void Action() { }
    public virtual void DeAction() { }
    public virtual void ResetToDefaults() { }
}

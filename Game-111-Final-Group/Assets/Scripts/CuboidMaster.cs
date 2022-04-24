using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CuboidMaster
{
    public static Action DelFadeIn = delegate { };
    public static Action DelFadeOut = delegate { };
    public static Action DelResetGame = delegate { };
    public static Action DelDisablePlayerMovement = delegate { };
    public static Action DelEnablePlayerMovement = delegate { };
    public static Action<Vector3> DelCheckpoint = delegate { };
    public static Action DelReturnToCheckpoint = delegate { };
    public static void FadeIn() { DelFadeIn(); }
    public static void FadeOut() { DelFadeOut(); }
    public static void ResetGame() { DelResetGame(); }
    public static void EnablePlayerMovement() { DelEnablePlayerMovement(); }
    public static void DisablePlayerMovement() { DelDisablePlayerMovement(); }
    public static void ReturnToCheckpoint() { DelReturnToCheckpoint(); }
    public static void Checkpoint(Vector3 value) { DelCheckpoint(value); }
}
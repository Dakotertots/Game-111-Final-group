using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSign : MonoBehaviour
{

    public List<GameActions> actions;

    private void OnEnable()
    {
        WinGA.Goal += PlayWin;
    }
    private void OnDisable()
    {
        WinGA.Goal -= PlayWin;
    }

    private void PlayWin()
    {
        foreach (GameActions item in actions)
            item.Action();
    }
}

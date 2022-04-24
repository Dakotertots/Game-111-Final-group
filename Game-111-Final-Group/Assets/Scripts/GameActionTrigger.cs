using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionTrigger : MonoBehaviour
{
    public bool bToggle,bTriggerOnce;
    public List<GameActions> action;
    public List<GameActions> deAction;
    private bool bActive,bDeActive,bTriggered;

    private void OnEnable()
    {
        CuboidMaster.DelResetGame += ResetObject;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (bActive) return;
        if (bTriggered && bTriggerOnce) return;
        StartCoroutine(nameof(GameActionSequence));
    }
    private void OnTriggerExit(Collider other)
    {
        if (bDeActive) return;
        if (bToggle)
            StartCoroutine(nameof(GameDeActionSequence));
    }
    private void ResetObject()
    {
        bActive = bDeActive = bTriggered = false;
        foreach (GameActions item in action)
            item.ResetToDefaults();
        foreach (GameActions item in deAction)
            item.ResetToDefaults();
    }
    IEnumerator GameActionSequence()
    {       
        bActive = true;
        if (bTriggerOnce)
            bTriggered = true;
        int count = 0;
        foreach(GameActions item in action)
        {
            count++;
            yield return new WaitForSeconds(item.delay);
            item.Action();
        }
        bActive = false;
    }
    IEnumerator GameDeActionSequence()
    {        
        bDeActive = true;
        foreach (GameActions item in deAction)
        {
            yield return new WaitForSeconds(item.delay);
            item.DeAction();
        }
        bDeActive = false;
    }
}

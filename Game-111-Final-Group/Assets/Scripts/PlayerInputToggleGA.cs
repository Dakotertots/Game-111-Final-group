using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputToggleGA : GameActions
{
    [Tooltip("Disables istead of enable input on Action")]
    public bool bInverse; 
    [Tooltip("Disables toggle")]
    public bool bDisableToggle;
    [Tooltip("Enable on Deaction")]
    public bool bEnableOnDeaction;
    /*public static Action DisablePlayerInput = delegate { };
    public static Action EnablePlayerInput = delegate {};*/
    private bool bPlayer;
    public override void Action()
    {
        if (!bPlayer) return;
        if(bDisableToggle)
        {
            if (bInverse)
                CuboidMaster.DisablePlayerMovement();//DisablePlayerInput();
            else
                CuboidMaster.EnablePlayerMovement();//EnablePlayerInput();
        }
        else
        {
            if (CubeMovement.GetState)
                CuboidMaster.DisablePlayerMovement();//DisablePlayerInput();
            else
                CuboidMaster.EnablePlayerMovement();//EnablePlayerInput();
        }        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayer = false;
        }
    }
    public override void DeAction()
    {
        if (bEnableOnDeaction)
            CuboidMaster.EnablePlayerMovement();//EnablePlayerInput();
        if (bDisableToggle)
        {
            if (bInverse)
                CuboidMaster.EnablePlayerMovement();//EnablePlayerInput();
            else
                CuboidMaster.DisablePlayerMovement(); //DisablePlayerInput();
        }
        else
        {
            if (CubeMovement.GetState)
                CuboidMaster.DisablePlayerMovement(); //DisablePlayerInput();
            else
                CuboidMaster.EnablePlayerMovement(); //EnablePlayerInput();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundGA : GameActions
{
    public bool bWinSound;
    public AudioClip actionClip, deActionClip;
    private AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        if(bWinSound)
            WinGA.Goal += Action;
    }
    private void OnDisable()
    {
        if (bWinSound)
            WinGA.Goal -= Action;
    }
    public override void Action()
    {
        aSource.clip = actionClip;
        aSource.Play();
    }
    public override void DeAction()
    {
        aSource.clip = deActionClip;
        aSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SwitchColorGA : GameActions
{
    public Color defaultColor, activeColor;
    private MeshRenderer mRenderer;

    private void Awake()
    {
        mRenderer = GetComponent<MeshRenderer>();
        CuboidMaster.DelResetGame -= ResetToDefaults;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetToDefaults;
    }
    public override void ResetToDefaults()
    {
        DeAction();
    }
    public override void Action()
    {
        mRenderer.material.SetColor("_BaseColor",activeColor);
    }
    public override void DeAction()
    {
        mRenderer.material.SetColor("_BaseColor", defaultColor);
    }
}

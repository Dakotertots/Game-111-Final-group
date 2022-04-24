using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationGA : GameActions
{
    public string actionParameter, deActionParameter;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        CuboidMaster.DelResetGame += ResetToDefaults;
    }
    private void OnDisable()
    {
        CuboidMaster.DelResetGame -= ResetToDefaults;
    }
    public override void ResetToDefaults()
    {
        anim.Rebind();
        anim.Update(0);
        //DeAction();
    }
    public override void Action()
    {
        anim.SetTrigger(actionParameter);
    }
    public override void DeAction()
    {
        if(deActionParameter != "")
            anim.SetTrigger(deActionParameter);
    }
}

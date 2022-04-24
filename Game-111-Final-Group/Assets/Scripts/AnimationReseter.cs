using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationReseter : MonoBehaviour
{
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
    public void ResetToDefaults()
    {
        anim.Rebind();
        anim.Update(0);
    }
}

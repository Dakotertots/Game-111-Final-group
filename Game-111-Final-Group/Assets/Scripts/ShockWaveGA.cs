using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveGA : GameActions
{
    public float maxHeight;
    public AnimationCurve animCurve;
    private List<Transform> blocks;
    private List<float> heights;
    private SphereCollider sCollider;
    private bool bActive;

    private void Awake()
    {
        sCollider = GetComponent<SphereCollider>();
        blocks = new List<Transform>();
        heights = new List<float>();
    }

    public override void Action()
    {
        if (bActive) return;
        StartCoroutine(nameof(ExecuteShockwave));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GenericBlock"))
        {
            blocks.Add(other.transform);
            heights.Add(maxHeight * (sCollider.radius/Vector3.Distance(other.transform.position, transform.position)));
        }
    }

    IEnumerator ExecuteShockwave()
    {
        sCollider.enabled = true;
        while (blocks.Count == 0)
            yield return new WaitForFixedUpdate();

        sCollider.enabled = false;
        bActive = true;
        float rate = 0;
        float yOffset = 0;
        List<Vector3> origPositions = new List<Vector3>();
        foreach (Transform item in blocks)
            origPositions.Add(item.position);
        while (rate < 1)
        {
            rate += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            for(int x = 0;x < blocks.Count;x++)
            {
                yOffset = origPositions[x].y + animCurve.Evaluate(rate);
                blocks[x].position = origPositions[x] + new Vector3(0, yOffset, 0);
            }
        }        
        for(int x = 0; x < origPositions.Count;x++)
        {
            blocks[x].position = origPositions[x];
        }
        bActive = false;
    }
}

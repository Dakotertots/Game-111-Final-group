using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBlock : MonoBehaviour
{
    public bool bChangeBlockType;
    public BlockType blockType;
    private int matchCount;
    public List<GenericBlock> blocks;

    private void Awake()
    {
        blocks = new List<GenericBlock>();
    }
    private void Update()
    {
        if(bChangeBlockType)
        {
            bChangeBlockType = false;
            for(int x = 0; x < blocks.Count;x++)
            {
                if (blocks[x].blockType == blockType)
                    matchCount++;
            }
            if (matchCount == 2)
                Debug.Log("string of three");
        }
    }
    private void Start()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position,transform.forward,out hitInfo,1.1f))
        {
            if (hitInfo.transform.TryGetComponent(out GenericBlock block))
                blocks.Add(block);
        }
        if (Physics.Raycast(transform.position, -transform.forward, out hitInfo, 1.1f))
        {
            if (hitInfo.transform.TryGetComponent(out GenericBlock block))
                blocks.Add(block);
        }
        if (Physics.Raycast(transform.position, transform.right, out hitInfo, 1.1f))
        {
            if (hitInfo.transform.TryGetComponent(out GenericBlock block))
                blocks.Add(block);
        }
        if (Physics.Raycast(transform.position, -transform.right, out hitInfo, 1.1f))
        {
            if (hitInfo.transform.TryGetComponent(out GenericBlock block))
                blocks.Add(block);
        }
    }
}
public enum BlockType { Red,Green,Blue,Yellow,White}

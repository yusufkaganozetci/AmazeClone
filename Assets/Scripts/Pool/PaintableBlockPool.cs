using System.Collections.Generic;
using UnityEngine;

public class PaintableBlockPool : MonoBehaviour
{
    [SerializeField] PaintableBlock paintableBlockPrefab;

    public List<PaintableBlock> availablePaintableBlocks = new List<PaintableBlock>();

    public void GeneratePool(int poolSize)
    {
        for(int i = 0; i < poolSize; i++)
        {
            PaintableBlock generatedBlock = Instantiate(paintableBlockPrefab, transform);
            generatedBlock.gameObject.SetActive(false);
            availablePaintableBlocks.Add(generatedBlock);
        }
    }

    public PaintableBlock GetBlock()
    {
        PaintableBlock paintableBlock;
        if(availablePaintableBlocks.Count > 0)
        {
            paintableBlock = availablePaintableBlocks[0];
            availablePaintableBlocks.RemoveAt(0);
            paintableBlock.gameObject.SetActive(true);
        }
        else
        {
            paintableBlock = Instantiate(paintableBlockPrefab, transform);
            paintableBlock.gameObject.SetActive(true);
        }
        return paintableBlock;
    }

    public void ReturnBlock(PaintableBlock block)
    {
        block.ResetBlock();
        block.gameObject.SetActive(false);
        block.transform.parent = transform;
        availablePaintableBlocks.Add(block);
    }

}
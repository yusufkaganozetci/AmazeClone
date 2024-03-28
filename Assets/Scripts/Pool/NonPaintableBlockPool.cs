using System.Collections.Generic;
using UnityEngine;

public class NonPaintableBlockPool : MonoBehaviour
{
    [SerializeField] NonPaintableBlock nonPaintableBlockPrefab;

    public List<NonPaintableBlock> availableNonPaintableBlocks = 
        new List<NonPaintableBlock>();

    public void GeneratePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            NonPaintableBlock generatedBlock = Instantiate(nonPaintableBlockPrefab, transform);
            generatedBlock.gameObject.SetActive(false);
            availableNonPaintableBlocks.Add(generatedBlock);
        }
    }

    public NonPaintableBlock GetBlock()
    {
        NonPaintableBlock paintableBlock;
        if (availableNonPaintableBlocks.Count > 0)
        {
            paintableBlock = availableNonPaintableBlocks[0];
            availableNonPaintableBlocks.RemoveAt(0);
            paintableBlock.gameObject.SetActive(true);
        }
        else
        {
            paintableBlock = Instantiate(nonPaintableBlockPrefab, transform);
            paintableBlock.gameObject.SetActive(true);
        }
        return paintableBlock;
    }

    public void ReturnBlock(NonPaintableBlock block)
    {
        block.gameObject.SetActive(false);
        block.transform.parent = transform;
        availableNonPaintableBlocks.Add(block);
    }

}
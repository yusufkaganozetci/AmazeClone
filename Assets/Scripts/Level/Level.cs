using System;
using UnityEngine;

[Serializable]
public enum BlockType
{
    Paintable,
    NonPaintable,
}

[Serializable]
public class BlockData
{
    public BlockType type;
}

[Serializable]
public class ArrayHolder<T>
{
    public T[] array;
}


[CreateAssetMenu(menuName = "Level")]
public class Level : ScriptableObject
{
    public int index;
    public Vector2 ballStartPosition;
    public int paintableBlockCount = 0;

    [Tooltip ("Blocks start from bottom left corner. " +
        "\nBlocks' length defines row count." +
        "\nBlocks' array's length defines column count")]
    public ArrayHolder<BlockData>[] blocks;

    [Header("Automatic Assign. Don't Change")]
    public int rowCount;
    public int columnCount;

    public void InitializeLevel()
    {
        paintableBlockCount = 0;
        columnCount = blocks[0].array.Length;
        rowCount = blocks.Length;
        CalculatePaintableBlockCount();
    }

    private void CalculatePaintableBlockCount()
    {
        for (int i = 0; i < rowCount; i++)
        {
            for(int j = 0; j < columnCount; j++)
            {
                if (blocks[i].array[j].type == BlockType.Paintable)
                {
                    paintableBlockCount++;
                }
            }
        }
    }

}
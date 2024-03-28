using UnityEngine;

namespace Managers
{
    public class BlockPoolManager : MonoBehaviour
    {
        [SerializeField] int blockCountInPools;
        [SerializeField] PaintableBlockPool paintableBlockPool;
        [SerializeField] NonPaintableBlockPool nonPaintableBlockPool;

        private void Awake()
        {
            GeneratePools();
        }

        private void GeneratePools()
        {
            paintableBlockPool.GeneratePool(blockCountInPools);
            nonPaintableBlockPool.GeneratePool(blockCountInPools);
        }

        private void Start()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventManager.Instance.SubscribeToEvent(EventType.GetBlockFromPool,
                GetBlockFromPool);
            EventManager.Instance.SubscribeToEvent(EventType.ReturnBlockToPool,
                ReturnBlockToPool);
        }

        public Block GetBlockFromPool(object blockTypeAsObj)
        {
            BlockType blockType = (BlockType)blockTypeAsObj;

            if (blockType == BlockType.Paintable)
            {
                return paintableBlockPool.GetBlock();
            }
            else if (blockType == BlockType.NonPaintable)
            {
                return nonPaintableBlockPool.GetBlock();
            }

            Debug.LogError("Undefined block type came. Pool didn't return block!");
            return null;
        }

        public void ReturnBlockToPool(object blockAsObj)
        {
            Block block = blockAsObj as Block;

            if (block.type == BlockType.Paintable)
            {
                paintableBlockPool.ReturnBlock(block as PaintableBlock);
            }
            else if (block.type == BlockType.NonPaintable)
            {
                nonPaintableBlockPool.ReturnBlock(block as NonPaintableBlock);
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.Instance.UnsubscribeFromEvent(EventType.GetBlockFromPool,
                GetBlockFromPool);
            EventManager.Instance.UnsubscribeFromEvent(EventType.ReturnBlockToPool,
                ReturnBlockToPool);
        }

    }

}
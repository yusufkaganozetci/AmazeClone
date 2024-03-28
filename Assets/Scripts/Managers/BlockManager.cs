using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Managers 
{ 
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private float blockDetectionThreshold = 0.49f;
        public List<Block> blocks;
        public int paintedBlockCount = 1;
        public bool areBlocksDestroyed = false;

        private Sequence clearBlocksSequence;

        private void Start()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventManager.Instance.SubscribeToEvent(EventType.DestinationBlocksRequested,
                GetDestinationAndNextBlock);
            EventManager.Instance.SubscribeToEvent(EventType.BlockPainted, OnBlockPainted);
            EventManager.Instance.SubscribeToEvent(EventType.ClosestBlockRequested, GetBlockInsideMinDistance);
            EventManager.Instance.SubscribeToEvent(EventType.LevelUnloadingStarted, ClearBlocks);
            EventManager.Instance.SubscribeToEvent(EventType.BlockGenerated, GenerateBlock);
            EventManager.Instance.SubscribeToEvent(EventType.CheckBlocksDestroyed, IsBlockDestroyingCompleted);
        }

        public void OnBlockPainted()
        {
            paintedBlockCount++;
            EventManager.Instance.TriggerActionEvent(EventType.CheckLevelFinished, paintedBlockCount);
        }
    
        private void GenerateBlock(object block)
        {
            blocks.Add(block as Block);
        }

        public Block GetBlockInsideMinDistance(object posAsObj)
        {
            Vector3 pos = (Vector3)posAsObj;

            foreach (var block in blocks)
            {
                if (Vector2.Distance(block.position, pos) <= blockDetectionThreshold)
                {
                    return block;
                }
            }

            return null;
        }

        public Block GetBlockFromExactPosition(Vector2 pos)
        {
            foreach (var block in blocks)
            {
                if (block.position == pos)
                {
                    return block;
                }
            }

            return null;
        }

        public object GetDestinationAndNextBlock(object ballPositionAsObject, object directionAsObject)
        {
            Vector3 ballPosition = (Vector3)ballPositionAsObject;
            Vector2 direction = (Vector2) directionAsObject;
            Vector2 currentBlockPosition = ballPosition;

            Block destinationBlock = null;
            Block afterDestinationBlock;
        
            while (true)
            {
                currentBlockPosition += direction;
                Block currentBlock = GetBlockFromExactPosition(currentBlockPosition);
                if (currentBlock == null || currentBlock.type == BlockType.NonPaintable)
                {
                    afterDestinationBlock = currentBlock;
                    break;
                }
                destinationBlock = currentBlock;
            }

            return (destinationBlock, afterDestinationBlock);
        }

        public void ClearBlocks()
        {
            paintedBlockCount = 1;
            clearBlocksSequence = DOTween.Sequence();
            float waitAmountBetweenGenerations = (float)EventManager.Instance.TriggerFuncEvent(EventType.BlockGenerationDelayRequested);
            float currentDelay = waitAmountBetweenGenerations;

            for (int i = 0; i < blocks.Count; i++)
            {
                clearBlocksSequence.Insert(currentDelay,
                    blocks[i].DestroyBlock());
                currentDelay += waitAmountBetweenGenerations;
            }

            clearBlocksSequence.OnComplete(() => OnBlockClearCompleted());
        }

        private void OnBlockClearCompleted()
        {
            blocks.Clear();
            areBlocksDestroyed = true;
        }

        private object IsBlockDestroyingCompleted()
        {
            if (!areBlocksDestroyed) return false;

            areBlocksDestroyed = false;
            return true;
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.Instance.UnsubscribeFromEvent(EventType.DestinationBlocksRequested,
                GetDestinationAndNextBlock);
            EventManager.Instance.UnsubscribeFromEvent(EventType.BlockPainted, OnBlockPainted);
            EventManager.Instance.UnsubscribeFromEvent(EventType.ClosestBlockRequested, GetBlockInsideMinDistance);
            EventManager.Instance.UnsubscribeFromEvent(EventType.LevelUnloadingStarted, ClearBlocks);
            EventManager.Instance.UnsubscribeFromEvent(EventType.BlockGenerated, GenerateBlock);
            EventManager.Instance.UnsubscribeFromEvent(EventType.CheckBlocksDestroyed, IsBlockDestroyingCompleted);
        }

    }
}
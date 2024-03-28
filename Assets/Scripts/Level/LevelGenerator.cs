using DG.Tweening;
using UnityEngine;
using Managers;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform levelParentTransform;
    [SerializeField] private float buildTime;
    [SerializeField] private float additionalDistanceForGeneration;

    public float timeBetweenBlockGenerations;

    private readonly float blockEdgeLength = 1f;

    private Sequence levelBuildSequence;
    private Vector2 blockStartPosition, blockCurrentPosition, ballStartPosition;
    private float distance, currentDelay;
    private bool isBallInThisBlock;
    
    void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        EventManager.Instance.SubscribeToEvent(EventType.LevelLoadingStarted, GenerateLevel);
        EventManager.Instance.SubscribeToEvent(EventType.BlockGenerationDelayRequested, GetBlockGenerationDelay);
    }

    public void GenerateLevel(object levelAsObj)
    {
        Level level = levelAsObj as Level;
        PrepareLevelGeneration(level);
        GenerateBlocks(level);
        levelBuildSequence.OnComplete(() =>
        OnLevelGenerationCompleted(ballStartPosition));
    }

    private void GenerateBlocks(Level level)
    {
        for (int i = 0; i < level.rowCount; i++)
        {
            for (int j = 0; j < level.columnCount; j++)
            {
                CreateBlock(level, i, j);
                blockCurrentPosition.x += blockEdgeLength;
                currentDelay += timeBetweenBlockGenerations;
            }
            blockCurrentPosition.y += blockEdgeLength;
            blockCurrentPosition.x = blockStartPosition.x;
        }
    }

    private void PrepareLevelGeneration(Level level)
    {
        levelBuildSequence = DOTween.Sequence();
        level.InitializeLevel();
        isBallInThisBlock = false;
        distance = level.columnCount + additionalDistanceForGeneration;
        timeBetweenBlockGenerations = buildTime / (level.columnCount * level.rowCount);
        currentDelay = timeBetweenBlockGenerations;
        blockStartPosition = new Vector2(CalculateStartPosition(level.columnCount) + distance,
            CalculateStartPosition(level.rowCount));
        blockCurrentPosition = blockStartPosition;
        ballStartPosition = Vector2.zero;
    }

    private void CreateBlock(Level level, int i, int j)
    {
        BlockData blockData = level.blocks[i].array[j];
        Block block = EventManager.Instance.TriggerFuncEvent(EventType.GetBlockFromPool, blockData.type) as Block;
        block.transform.parent = levelParentTransform;
        if (level.ballStartPosition == new Vector2(blockCurrentPosition.x - distance,
            blockCurrentPosition.y))
        {
            isBallInThisBlock = true;
            ballStartPosition = level.ballStartPosition;
        }
        levelBuildSequence.Insert(currentDelay, block.InitializeBlock(blockCurrentPosition, distance, isBallInThisBlock));
        isBallInThisBlock = false;
    }

    private void OnLevelGenerationCompleted(Vector2 ballStartPosition)
    {
        levelBuildSequence.Kill();
        EventManager.Instance.TriggerActionEvent(EventType.InitializeBall, ballStartPosition);
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void UnsubscribeToEvents()
    {
        EventManager.Instance.UnsubscribeFromEvent(EventType.LevelLoadingStarted, GenerateLevel);
        EventManager.Instance.UnsubscribeFromEvent(EventType.BlockGenerationDelayRequested, GetBlockGenerationDelay);
    }

    private float CalculateStartPosition(int blockCount)
    {
        if(blockCount % 2 == 0) return (-blockCount / 2) + blockEdgeLength / 2;
        return -blockCount / 2;
    }

    private object GetBlockGenerationDelay()
    {
        return timeBetweenBlockGenerations;
    }

}
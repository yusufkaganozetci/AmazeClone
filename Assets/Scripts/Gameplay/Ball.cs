using DG.Tweening;
using UnityEngine;
using Managers;

public class Ball : MonoBehaviour
{
    [SerializeField] private float movementTimeMultiplier = 3;
    [SerializeField] private float rotationAmount = 180;
    [SerializeField] private float ballStartZDistance = 1;
    [SerializeField] private float ballZPosition = 0;
    [SerializeField] private float initializeTweenDuration = 0.5f;
    [SerializeField] private Ease movementEase;
    [SerializeField] private Ease initializationEase;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem movingEffect;
    [SerializeField] private ParticleSystem movementFinishedEffect;

    private MovementInfo movementInfo;
    private Tween movementTween, rotationTween;
    private Vector3 currentPosition, destinationPosition;
    private bool canMove = false;

    private void Start()
    {
        SubscribeToEvents();
        gameObject.SetActive(false);
    }

    private void SubscribeToEvents()
    {
        EventManager.Instance.SubscribeToEvent(EventType.LevelUnloadingStarted, ResetBall);
        EventManager.Instance.SubscribeToEvent(EventType.InitializeBall, Initialize);
        EventManager.Instance.SubscribeToEvent(EventType.InitializeBallMovement, InitializeMovement);
    }

    public void Initialize(object positionAsObj)
    {
        gameObject.SetActive(true);
        Vector2 position = (Vector2) positionAsObj;
        currentPosition = new Vector3(position.x, position.y, transform.localPosition.z + ballStartZDistance);
        transform.localPosition = currentPosition;
        transform.DOLocalMoveZ(ballZPosition, initializeTweenDuration)
            .SetEase(initializationEase)
            .OnComplete(() => OnInitializationCompleted());
    }

    private void OnInitializationCompleted()
    {
        canMove = true;
        trailRenderer.enabled = true;
    }

    public void ResetBall()
    {
        canMove = false;
        movingEffect.Stop();
        KillTweens();
        ResetPosition();
        gameObject.SetActive(false);
    }

    public void InitializeMovement(object directionAsObj)
    {
        if (!canMove) return;

        canMove = false;
        Vector2 direction = (Vector2) directionAsObj;
        (Block, Block) destinationBlocks = ((Block, Block))EventManager.Instance.TriggerFuncEvent(EventType.DestinationBlocksRequested, currentPosition, direction);
        movementInfo = MovementInfo.GenerateNewMovement(destinationBlocks.Item1, destinationBlocks.Item2, currentPosition, direction);
        
        if(movementInfo == null)
        {
            canMove = true;
            return;
        }
        
        StartMovement();
    }

    private void StartMovement()
    {
        EventManager.Instance.TriggerActionEvent(EventType.BallMovementStarted);
        destinationPosition = new Vector3(movementInfo.destinationPosition.x, movementInfo.destinationPosition.y, 0);
        movementTween = transform.DOLocalMove(destinationPosition, movementInfo.movementTime)
            .SetEase(movementEase)
            .OnComplete(() => OnMovementCompleted());
        movingEffect.Play();
        RotateDuringMovement();
    }

    private void RotateDuringMovement()
    {
        ResetRotation();
        Vector3 rotationDirection = movementInfo.hitRotationDirection;
        rotationTween = transform.DOLocalRotate(transform.localRotation.eulerAngles + rotationDirection * rotationAmount, movementInfo.movementTime * movementTimeMultiplier);
    }

    private void ResetRotation()
    {
        if (rotationTween != null && rotationTween.active) rotationTween.Kill();
        transform.localEulerAngles = Vector3.zero;
    }

    private void OnMovementCompleted()
    {
        canMove = true;
        currentPosition = destinationPosition;
        movementFinishedEffect.Play();
        (movementInfo?.blockAfterTheDestination as NonPaintableBlock)?.OnBallHit();
        EventManager.Instance.TriggerActionEvent(EventType.BallMovementCompleted);
    }

    void Update()
    {
        UpdatePositionAndPaintBlock();
    }

    private void UpdatePositionAndPaintBlock()
    {
        UpdateCurrentPosition();
        PaintClosestBlock();
    }

    private void UpdateCurrentPosition()
    {
        currentPosition.x = transform.localPosition.x;
        currentPosition.y = transform.localPosition.y;
    }

    private void PaintClosestBlock()
    {
        PaintableBlock paintableBlock = EventManager.Instance.TriggerFuncEvent(EventType.ClosestBlockRequested, currentPosition) as PaintableBlock;
        if (paintableBlock != null) StartCoroutine(paintableBlock.PaintBlockCoroutine());
    }

    private void KillTweens()
    {
        movementTween?.Kill();
        rotationTween?.Kill();
    }

    private void ResetPosition()
    {
        currentPosition = Vector3.zero;
        transform.localPosition = currentPosition;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        EventManager.Instance.UnsubscribeFromEvent(EventType.LevelUnloadingStarted, ResetBall);
        EventManager.Instance.UnsubscribeFromEvent(EventType.InitializeBall, Initialize);
        EventManager.Instance.UnsubscribeFromEvent(EventType.InitializeBallMovement, InitializeMovement);
    }

}
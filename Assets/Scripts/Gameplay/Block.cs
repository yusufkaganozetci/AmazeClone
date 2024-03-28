using DG.Tweening;
using UnityEngine;
using Managers;

public abstract class Block : MonoBehaviour
{
    [SerializeField] private Ease movementEase;
    [SerializeField] private float movementTime = 0.3f;

    public Vector2 position;
    public BlockType type;

    private float distanceToDestination;
    
    public virtual Tween InitializeBlock(Vector3 currentPosition, float distanceBetweenDestination, bool isBallOnThisBlock)
    {
        currentPosition = AssignValues(currentPosition, distanceBetweenDestination);
        EventManager.Instance.TriggerActionEvent(EventType.BlockGenerated, this);
        return transform.DOLocalMove(currentPosition, movementTime)
            .SetEase(movementEase)
            .OnComplete(() => position = currentPosition);
    }

    private Vector3 AssignValues(Vector3 currentPosition, float distanceBetweenDestination)
    {
        currentPosition.z = transform.localPosition.z;
        transform.localPosition = currentPosition;
        transform.localEulerAngles = Vector3.zero;
        currentPosition.x -= distanceBetweenDestination;
        distanceToDestination = distanceBetweenDestination;
        return currentPosition;
    }

    public Tween DestroyBlock()
    {
        return transform.DOLocalMove(transform.localPosition - new Vector3(distanceToDestination, 0, 0), movementTime)
            .SetEase(movementEase)
            .OnComplete(() => EventManager.Instance.TriggerActionEvent(EventType.ReturnBlockToPool, this));
    }

}
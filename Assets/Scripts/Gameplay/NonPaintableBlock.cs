using DG.Tweening;
using UnityEngine;

public class NonPaintableBlock : Block
{
    [SerializeField] private float minRotationTime = 0.25f;
    [SerializeField] private float maxRotationTime = 0.4f;
    [SerializeField] private float rotationAmount = 90;
    [SerializeField] private Ease rotationEase;

    private readonly int minDistanceForRotation = 1;
    private readonly int maxDistanceForRotation = 8;

    private Tween rotationTween;

    public void OnBallHit()
    {
        MovementInfo movementInfo = MovementInfo.GetCurrentMovementInfo();
        if (movementInfo == null) return;
        rotationTween?.Kill();
        transform.localEulerAngles = Vector3.zero;
        Vector3 rotationDirection = movementInfo.hitRotationDirection;
        rotationTween = transform.DOLocalRotate(transform.localRotation.eulerAngles + rotationDirection * rotationAmount,
             CalculateRotationTime(movementInfo))
            .SetEase(rotationEase);
    }

    private float CalculateRotationTime(MovementInfo movementInfo)
    {
        float distance = movementInfo.distance;
        if(distance > maxDistanceForRotation) distance = maxDistanceForRotation;
        float rotationTime = maxRotationTime - (distance - minDistanceForRotation) * (maxRotationTime - minRotationTime) / (maxDistanceForRotation - minDistanceForRotation);
        return rotationTime;   
    }

}
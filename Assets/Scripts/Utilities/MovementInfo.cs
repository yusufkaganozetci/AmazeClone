using UnityEngine;

public class MovementInfo
{
    public static MovementInfo Instance;

    public Block destinationBlock, blockAfterTheDestination;

    public Vector3 hitRotationDirection;
    public Vector2 destinationPosition, direction;
    public float movementTime;
    public int distance;

    public readonly float movementTimePerBlock = 0.05f;

    private MovementInfo() { }

    public static MovementInfo GenerateNewMovement(Block destinationBlock, Block blockAfterTheDestination, Vector2 ballPosition, Vector2 direction)
    {
        Instance ??= new MovementInfo();
        if (destinationBlock == null) return null;
        Instance.destinationBlock = destinationBlock;
        Instance.blockAfterTheDestination = blockAfterTheDestination;
        Instance.destinationPosition = destinationBlock.position;
        Instance.direction = direction;
        Instance.distance = CalculateDistance(ballPosition, Instance.destinationPosition);
        Instance.movementTime = Instance.distance * Instance.movementTimePerBlock;
        Instance.hitRotationDirection = CalculateHitRotationDirection(direction);
        return Instance;
    }

    private static Vector2 CalculateHitRotationDirection(Vector2 direction)
    {
        Vector2 hitRotationDirection = Vector2.zero;

        if (direction == Vector2.up)
            hitRotationDirection = Vector2.right;
        
        else if (direction == Vector2.down)
            hitRotationDirection = Vector2.left;
        
        else if (direction == Vector2.right)
            hitRotationDirection = Vector2.down;
        
        else if (direction == Vector2.left)
            hitRotationDirection = Vector2.up;
        
        return hitRotationDirection;
    }

    public static MovementInfo GetCurrentMovementInfo()
    {
        Instance ??= new MovementInfo();
        if (Instance.destinationBlock == null) return null;
        return Instance;
    }

    private static int CalculateDistance(Vector2 ballPosition, Vector2 destinationPosition)
    {
        return (int)(Mathf.Abs(ballPosition.x - destinationPosition.x) + Mathf.Abs(ballPosition.y - destinationPosition.y));
    }

}
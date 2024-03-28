using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private float swipeThreshold;

        private Vector2 startTouchPosition, currentTouchPosition;

        void Update()
        {
            ManageInputs();
        }

        private void ManageInputs()
        {
            if (Application.isMobilePlatform)
                ManageMobileInput();
            else
                ManageComputerInput();
        }

        private void ManageMobileInput()
        {
            if (Input.touchCount <= 0) return;
            
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }

            else if (touch.phase == TouchPhase.Moved)
            {
                currentTouchPosition = touch.position;
                ManageSwipe();
                startTouchPosition = currentTouchPosition;
            }

        }

        private void ManageSwipe()
        {
            if (Vector2.Distance(startTouchPosition, currentTouchPosition) < swipeThreshold) return;

            Vector2 direction;
            if (Mathf.Abs(currentTouchPosition.x - startTouchPosition.x) > Mathf.Abs(currentTouchPosition.y - startTouchPosition.y))
            {
                // Horizontal swipe
                direction = (currentTouchPosition.x > startTouchPosition.x) ? Vector2.right : Vector2.left;
            }
            else
            {
                // Vertical swipe
                direction = (currentTouchPosition.y > startTouchPosition.y) ? Vector2.up : Vector2.down;
            }

            TriggerBallMovementEvent(direction);
        }

        private void ManageComputerInput()
        {
            Vector2 direction = Vector2.zero;

            if (Input.GetKeyDown(KeyCode.UpArrow))
                direction = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                direction = Vector2.down;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                direction = Vector2.right;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                direction = Vector2.left;

            if (direction != Vector2.zero)
                TriggerBallMovementEvent(direction);
        }

        private void TriggerBallMovementEvent(Vector2 direction)
        {
            EventManager.Instance.TriggerActionEvent(EventType.InitializeBallMovement,
                direction);
        }

    }
}
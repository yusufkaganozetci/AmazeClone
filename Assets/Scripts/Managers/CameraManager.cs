using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Position Settings")]
        [SerializeField] private float defaultZPosPerBlock;
        [SerializeField] private float movementTime = 1f;

        [Header("Camera Shake Settings")]
        [SerializeField] private float shakeDuration = 0.1f;
        [SerializeField] private float shakeStrength = 0.3f;

        private readonly float defaultAspectRatio = 1080f / 1920f;

        private Vector3 cameraPosition;
        private float deviceRatio;
        private Tween cameraShake, movementTween;

        void Start()
        {
            Initialize();
            SubscribeToEvents();
        }

        private void Initialize()
        {
            deviceRatio = defaultAspectRatio / Camera.main.aspect;
            cameraPosition = transform.position;
        }

        private void SubscribeToEvents()
        {
            EventManager.Instance.SubscribeToEvent(EventType.LevelUnloadingStarted, ResetCamera);
            EventManager.Instance.SubscribeToEvent(EventType.BallMovementCompleted, ShakeCamera);
            EventManager.Instance.SubscribeToEvent(EventType.LevelLoadingStarted, MoveCameraAccordingToBlockCount);
        }

        public void MoveCameraAccordingToBlockCount(object level)
        {
            int blockCount = (level as Level).columnCount;
            cameraPosition.z = defaultZPosPerBlock * blockCount * deviceRatio;
            movementTween = transform.DOMove(cameraPosition, movementTime);
        }

        public void ShakeCamera()
        {
            ResetCameraPosition();
            cameraShake = transform.DOShakePosition(shakeDuration, strength: shakeStrength);
        }

        public void ResetCamera()
        {
            ResetCameraPosition();
            KillAllTweens();
        }

        private void ResetCameraPosition()
        {
            transform.position = cameraPosition;
        }

        private void KillAllTweens()
        {
            movementTween?.Kill();
            cameraShake?.Kill();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.Instance.UnsubscribeFromEvent(EventType.LevelUnloadingStarted, ResetCamera);
            EventManager.Instance.UnsubscribeFromEvent(EventType.BallMovementCompleted, ShakeCamera);
            EventManager.Instance.UnsubscribeFromEvent(EventType.LevelLoadingStarted, MoveCameraAccordingToBlockCount);
        }

    }
}
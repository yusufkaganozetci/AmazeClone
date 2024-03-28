using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;

        [SerializeField] AudioClip[] levelCompletedSFXs;
        [Range(0,1)] 
        [SerializeField] float levelCompletedSFXVolume;

        [SerializeField] AudioClip ballRollingSFX;
        [Range(0, 1)]
        [SerializeField] float ballRollingSFXVolume;

        [SerializeField] AudioClip[] ballHitSFXs;
        [Range(0, 1)]
        [SerializeField] float ballHitSFXVolume;

        void Start()
        {
            EventManager.Instance.SubscribeToEvent(EventType.NextLevelLoadRequested, PlayLevelCompletedSFX);
            EventManager.Instance.SubscribeToEvent(EventType.BallMovementStarted, PlayBallRollingSFX);
            EventManager.Instance.SubscribeToEvent(EventType.BallMovementCompleted, PlayBallStopSFX);
        }

        private void PlayLevelCompletedSFX()
        {
            StopCurrentAudio();
            audioSource.PlayOneShot(levelCompletedSFXs[Random.Range(0, levelCompletedSFXs.Length)]
                ,levelCompletedSFXVolume);
        }

        private void PlayBallRollingSFX()
        {
            audioSource.PlayOneShot(ballRollingSFX, ballRollingSFXVolume);
        }

        private void PlayBallStopSFX()
        {
            StopCurrentAudio();
            audioSource.PlayOneShot(ballHitSFXs[Random.Range(0, ballHitSFXs.Length)], ballHitSFXVolume);
        }

        private void StopCurrentAudio()
        {
            audioSource.Stop();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.Instance.UnsubscribeFromEvent(EventType.NextLevelLoadRequested, PlayLevelCompletedSFX);
            EventManager.Instance.UnsubscribeFromEvent(EventType.BallMovementStarted, PlayBallRollingSFX);
            EventManager.Instance.UnsubscribeFromEvent(EventType.BallMovementCompleted, PlayBallStopSFX);
        }

    }
}
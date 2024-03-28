using Lofelt.NiceVibrations;
using Managers;
using UnityEngine;

public class VibrationController : MonoBehaviour
{

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        EventManager.Instance.SubscribeToEvent(EventType.BallMovementCompleted, BallHitVibration);
        EventManager.Instance.SubscribeToEvent(EventType.NextLevelLoadRequested, LevelCompletedVibration);
    }

    private void BallHitVibration()
    {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }

    private void LevelCompletedVibration()
    {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        EventManager.Instance.UnsubscribeFromEvent(EventType.BallMovementCompleted, BallHitVibration);
        EventManager.Instance.UnsubscribeFromEvent(EventType.NextLevelLoadRequested, LevelCompletedVibration);
    }
    
}
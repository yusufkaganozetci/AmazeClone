using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public Level currentLevel;

        private void Start()
        {
            //Application.targetFrameRate = (int) Screen.currentResolution.refreshRateRatio.value; //This line is used for mobile build.
            DOTween.SetTweensCapacity(300, 50);
            EventManager.Instance.SubscribeToEvent(EventType.LevelLoadingStarted, AssignCurrentLevel);
            EventManager.Instance.SubscribeToEvent(EventType.CheckLevelFinished, CheckIsLevelFinished);
        }

        public void AssignCurrentLevel(object level)
        {
            currentLevel = level as Level;
        }

        public void CheckIsLevelFinished(object paintedBlockCount)
        {
            if (currentLevel.paintableBlockCount == (int)paintedBlockCount)
            {
                EventManager.Instance.TriggerActionEvent(EventType.NextLevelLoadRequested);
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.Instance.SubscribeToEvent(EventType.LevelLoadingStarted, AssignCurrentLevel);
            EventManager.Instance.SubscribeToEvent(EventType.CheckLevelFinished, CheckIsLevelFinished);
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public List<Level> levels;
        public int currentLevelIndex = 0;

        void Start()
        {
            EventManager.Instance.SubscribeToEvent(EventType.NextLevelLoadRequested, LoadNextLevel);
            currentLevelIndex = PlayerPrefs.GetInt("Last Level Index", 0);
            StartCoroutine(LoadLevel(currentLevelIndex));
        }

        private void UnloadLevel()
        {
            EventManager.Instance.TriggerActionEvent(EventType.LevelUnloadingStarted);
        }

        public void LoadNextLevel()
        {
            currentLevelIndex++;
            if (currentLevelIndex >= levels.Count) currentLevelIndex = 0;
            PlayerPrefs.SetInt("Last Level Index", currentLevelIndex);
            StartCoroutine(LoadLevel(currentLevelIndex));
        }

        public IEnumerator LoadLevel(int index)
        {
            UnloadLevel();
            yield return new WaitUntil(() => (bool)EventManager.Instance.TriggerFuncEvent(EventType.CheckBlocksDestroyed));
            Level level = levels[index];
            EventManager.Instance.TriggerActionEvent(EventType.LevelTextUpdateRequested, index + 1);
            EventManager.Instance.TriggerActionEvent(EventType.LevelLoadingStarted, level);
        }

        private void OnDestroy()
        {
            EventManager.Instance.UnsubscribeFromEvent(EventType.NextLevelLoadRequested, LoadNextLevel);
        }

    }
}
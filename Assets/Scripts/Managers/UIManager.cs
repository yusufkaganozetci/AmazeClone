using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI currentLevelText;

        void Start()
        {
            EventManager.Instance.SubscribeToEvent(EventType.LevelTextUpdateRequested, ChangeCurrentLevelText);
        }

        private void ChangeCurrentLevelText(object levelIndex)
        {
            currentLevelText.text = "Level " + ((int)levelIndex).ToString();
        }

        private void OnDestroy()
        {
            EventManager.Instance.UnsubscribeFromEvent(EventType.LevelTextUpdateRequested, ChangeCurrentLevelText);
        }

    }
}
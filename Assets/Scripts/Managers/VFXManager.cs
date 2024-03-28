using System.Collections;
using UnityEngine;

namespace Managers
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] ParticleSystem[] levelCompletedVFXs;
        [SerializeField] private float delayBetweenLevelCompletedEffects;

        void Start()
        {
            EventManager.Instance.SubscribeToEvent(EventType.NextLevelLoadRequested,
                PlayLevelCompletedVFX);
        }

        private void PlayLevelCompletedVFX()
        {
            StartCoroutine(PlayLevelCompletedVFXCoroutine());
        }

        private IEnumerator PlayLevelCompletedVFXCoroutine()
        {
            for (int i = 0; i < levelCompletedVFXs.Length; i++)
            {
                levelCompletedVFXs[i].gameObject.SetActive(true);
                levelCompletedVFXs[i].Play();
                yield return new WaitForSeconds(delayBetweenLevelCompletedEffects);
            }
        }

        private void OnDestroy()
        {
            EventManager.Instance.UnsubscribeFromEvent(EventType.NextLevelLoadRequested,
                PlayLevelCompletedVFX);
        }

    }
}
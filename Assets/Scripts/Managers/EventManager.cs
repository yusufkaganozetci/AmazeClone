using System.Collections.Generic;
using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<EventType, Action> zeroParamActions = new Dictionary<EventType, Action>();
        private Dictionary<EventType, Action<object>> oneParamActions = new Dictionary<EventType, Action<object>>();

        private Dictionary<EventType, Func<object>> zeroParamFuncs = new Dictionary<EventType, Func<object>>();
        private Dictionary<EventType, Func<object, object>> oneParamFuncs = new Dictionary<EventType, Func<object, object>>();
        private Dictionary<EventType, Func<object, object, object>> twoParamFuncs = new Dictionary<EventType, Func<object, object, object>>();

        public static EventManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        #region Event Subscribtions

        public void SubscribeToEvent(EventType eventType, Action listener)
        {
            if (zeroParamActions.ContainsKey(eventType))
                zeroParamActions[eventType] += listener;

            else
                zeroParamActions[eventType] = listener;
        }

        public void SubscribeToEvent(EventType eventType, Action<object> listener)
        {
            if (oneParamActions.ContainsKey(eventType))
                oneParamActions[eventType] += listener;

            else
                oneParamActions[eventType] = listener;
        }

        public void SubscribeToEvent(EventType eventType, Func<object> listener)
        {
            if (zeroParamFuncs.ContainsKey(eventType))
                zeroParamFuncs[eventType] += listener;

            else
                zeroParamFuncs[eventType] = listener;
        }

        public void SubscribeToEvent(EventType eventType, Func<object, object> listener)
        {
            if (oneParamFuncs.ContainsKey(eventType))
                oneParamFuncs[eventType] += listener;

            else
                oneParamFuncs[eventType] = listener;
        }

        public void SubscribeToEvent(EventType eventType, Func<object, object, object> listener)
        {
            if (twoParamFuncs.ContainsKey(eventType))
                twoParamFuncs[eventType] += listener;

            else
                twoParamFuncs[eventType] = listener;
        }

        #endregion

        #region Event Unsubscribtions

        public void UnsubscribeFromEvent(EventType eventType, Action listener)
        {
            if (!zeroParamActions.ContainsKey(eventType)) return;

            zeroParamActions[eventType] -= listener;
            if (zeroParamActions[eventType] == null)
            {
                zeroParamActions.Remove(eventType);
            }
        }

        public void UnsubscribeFromEvent(EventType eventType, Action<object> listener)
        {
            if (!oneParamActions.ContainsKey(eventType)) return;
            Debug.Log("UNSUBSCRIBED");
            oneParamActions[eventType] -= listener;
            if (oneParamActions[eventType] == null)
            {
                oneParamActions.Remove(eventType);
            }
        }

        public void UnsubscribeFromEvent(EventType eventType, Func<object> listener)
        {
            if (!zeroParamFuncs.ContainsKey(eventType)) return;

            zeroParamFuncs[eventType] -= listener;
            if (zeroParamFuncs[eventType] == null)
            {
                zeroParamFuncs.Remove(eventType);
            }
        }

        public void UnsubscribeFromEvent(EventType eventType, Func<object, object> listener)
        {
            if (!oneParamFuncs.ContainsKey(eventType)) return;

            oneParamFuncs[eventType] -= listener;
            if (oneParamFuncs[eventType] == null)
            {
                oneParamFuncs.Remove(eventType);
            }
        }

        public void UnsubscribeFromEvent(EventType eventType, Func<object, object, object> listener)
        {
            if (!twoParamFuncs.ContainsKey(eventType)) return;

            twoParamFuncs[eventType] -= listener;
            if (twoParamFuncs[eventType] == null)
            {
                twoParamFuncs.Remove(eventType);
            }
        }

        #endregion

        #region Event Triggers

        public void TriggerActionEvent(EventType eventType)
        {
            if (zeroParamActions.TryGetValue(eventType, out Action delegateAction)) delegateAction.Invoke();
        }

        public void TriggerActionEvent(EventType eventType, object arg)
        {
            if (oneParamActions.TryGetValue(eventType, out Action<object> delegateAction)) delegateAction.Invoke(arg);
        }

        public object TriggerFuncEvent(EventType eventType)
        {
            return zeroParamFuncs.TryGetValue(eventType, out Func<object> delegateFunc) ? delegateFunc.Invoke() : null;
        }

        public object TriggerFuncEvent(EventType eventType, object arg1)
        {
            return oneParamFuncs.TryGetValue(eventType, out Func<object, object> delegateFunc) ? delegateFunc.Invoke(arg1) : null;
        }

        public object TriggerFuncEvent(EventType eventType, object arg1, object arg2)
        {
            return twoParamFuncs.TryGetValue(eventType, out Func<object, object, object> delegateFunc) ? delegateFunc.Invoke(arg1, arg2) : null;
        }

        #endregion

        private void OnDestroy()
        {
            zeroParamActions.Clear();
            oneParamActions.Clear();
            zeroParamFuncs.Clear();
            oneParamFuncs.Clear();
            twoParamFuncs.Clear();
        }

    }

}
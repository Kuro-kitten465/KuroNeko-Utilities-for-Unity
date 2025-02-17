using System;
using System.Collections.Generic;
using UnityEngine;

namespace KuroNeko.Utilities.DesignPattern
{
    public class EventBus
    {
        private readonly Dictionary<object, Action> _events = new();

        public void Register(object eventKey, Action callback)
        {
            if (_events.ContainsKey(eventKey))
            {
                _events[eventKey] = callback;
            }
            else
            {
                _events.Add(eventKey, callback);
            }
        }

        public void Unregister(object eventKey)
        {
            if (_events.TryGetValue(eventKey, out Action act))
            {
                var currentAct = Delegate.Remove(act, act);

                if (currentAct == null)
                {
                    _events.Remove(eventKey);
                }
                else
                {
                    Debug.LogWarning($"Event {eventKey} is not removed");
                }
            }
        }

        public void Invoke(object eventKey)
        {
            if (_events.TryGetValue(eventKey, out Action act))
            {
                act.Invoke();
            }
        }
    }
}
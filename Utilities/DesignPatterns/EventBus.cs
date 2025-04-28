using System;
using System.Collections.Generic;
using System.Linq;

namespace Kuro.UnityUtils.DesignPatterns
{
    public static class EventBus
    {
        public delegate void EventObject(object sender, params object[] args);
        public delegate T EventObject<T>(object sender, params object[] args);
        private static readonly Dictionary<string, List<EventObject>> _nonGenericEvents = new();
        private static readonly Dictionary<string, Delegate> _genericEvents = new();

        #region Subscribers
        public static void Subscribe(object eventName, EventObject callback)
        {
            var key = EventKey(eventName);

            if (!_nonGenericEvents.TryGetValue(key, out var handlers))
            {
                handlers = new List<EventObject>();
                _nonGenericEvents.Add(key, handlers);
            }

            handlers.Add(callback);
        }

        public static void Subscribe(object eventName, List<EventObject> callbacks)
        {
            var key = EventKey(eventName);

            if (!_nonGenericEvents.TryGetValue(key, out var handlers))
            {
                handlers = callbacks.ToList();
                _nonGenericEvents.Add(key, handlers);
            }

            handlers.AddRange(callbacks);
        }

        public static void Subscribe<T>(object eventName, EventObject<T> callback)
        {
            var key = EventKey(eventName);

            if (!_genericEvents.ContainsKey(key))
                _genericEvents.Add(key, callback);
        }

        #endregion
        #region Unsubscribers

        public static void Unsubscribe(object eventName, EventObject callback)
        {
            var key = EventKey(eventName);

            if (_nonGenericEvents.TryGetValue(key, out var handlers))
            {
                handlers.Remove(callback);

                if (handlers.Count == 0)
                    _nonGenericEvents.Remove(key);
            }
        }

        public static void Unsubscribe(object eventName, List<EventObject> callbacks)
        {
            var key = EventKey(eventName);

            if (_nonGenericEvents.TryGetValue(key, out var handlers))
            {
                foreach (var callback in callbacks)
                {
                    handlers.Remove(callback);
                }

                if (handlers.Count == 0)
                    _nonGenericEvents.Remove(key);
            }
        }

        public static void Unsubscribe<T>(object eventName, EventObject<T> callback)
        {
            var key = EventKey(eventName);

            if (_genericEvents.TryGetValue(key, out var handler))
            {
                handler = null;
                _genericEvents.Remove(key);
            }
        }

        public static void UnsubscribeAll(object eventName)
        {
            var key = EventKey(eventName);

            if (_genericEvents.TryGetValue(key, out var handler))
            {
                handler = null;
                _genericEvents.Remove(key);
            }

            if (_nonGenericEvents.TryGetValue(key, out var handlers))
            {
                handlers.Clear();
                _nonGenericEvents.Remove(key);
            }
        }

        public static void ClearEvents()
        {
            _nonGenericEvents.Clear();
            _genericEvents.Clear();
        }

        #endregion
        #region Publishers
        public static void Publish(object eventName, object sender, params object[] args) 
        {
            var key = EventKey(eventName);

            if (_nonGenericEvents.TryGetValue(key, out var handlers))
            {
                foreach (var handler in handlers)
                {
                    handler(sender, args);
                }
            }
        }

        public static T Publish<T>(object eventName, object sender, params object[] args) 
        {
            var key = EventKey(eventName);

            if (_genericEvents.TryGetValue(key, out var handler))
            {
                if (handler is EventObject<T> genericHandler)
                {
                    return genericHandler(sender, args);
                }
            }

            return default;
        }
        #endregion

        #region Extensions Features
        private static string EventKey(object name) => string.Intern(name.ToString());
        public enum EventType
        { 
            IsVoid, IsGeneric, IsBoth
        }

        public static bool HasEvent(object eventName, EventType eventType = EventType.IsBoth)
        {
            var key = EventKey(eventName);

            return eventType switch
            {
                EventType.IsVoid => _nonGenericEvents.ContainsKey(key),
                EventType.IsGeneric => _genericEvents.ContainsKey(key),
                EventType.IsBoth => _nonGenericEvents.ContainsKey(key) && _genericEvents.ContainsKey(key),
                _ => false
            };
        }

        public static EventType GetEventType(object eventName)
        {
            var key = EventKey(eventName);

            var hasVoidEvent = _nonGenericEvents.ContainsKey(key);
            var hasGenericEvent = _genericEvents.ContainsKey(key);

            if (hasVoidEvent && hasGenericEvent)
                return EventType.IsBoth;
            if (hasVoidEvent)
                return EventType.IsVoid;
            if (hasGenericEvent)
                return EventType.IsGeneric;

            throw new InvalidOperationException("Event type not found.");
        }

        #endregion
    }
}

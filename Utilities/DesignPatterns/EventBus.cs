using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kuro.UnityUtils.Tests;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kuro.UnityUtils.DesignPatterns
{
    public static class EventBus
    {
        public delegate void EventFunc(object sender, object arg);
        public delegate T EventFunc<T>(object sender, object arg);
        private static readonly ConcurrentDictionary<Enum, EventFunc> _nonGenericEvents = new();
        private static readonly ConcurrentDictionary<Enum, Delegate> _genericEvents = new();

        #region Subscribers
        public static void Subscribe(Enum key, EventFunc eventFunc)
        {
            if (!_nonGenericEvents.TryGetValue(key, out var handler))
            {
                _nonGenericEvents.TryAdd(key, eventFunc);
                return;
            }

            var oldHandler = handler;
            handler += eventFunc;
            var _ = _nonGenericEvents.TryUpdate(key, handler, oldHandler);
            Debug.LogAssertion(_);
        }

        public static void Subscribe(Enum key, List<EventFunc> eventFuncs)
        {
            if (!_nonGenericEvents.TryGetValue(key, out var handler))
            {
                eventFuncs.ForEach(e => handler += e);
                _nonGenericEvents.TryAdd(key, handler);
                return;
            }

            var oldHandler = handler;
            eventFuncs.ForEach(e => handler += e);
            _nonGenericEvents.TryUpdate(key, handler, oldHandler);
        }

        public static void Subscribe<T>(Enum key, EventFunc<T> eventFunc, bool updateExistEvent = false)
        {
            if (!_genericEvents.TryGetValue(key, out var handler))
                _genericEvents.TryAdd(key, eventFunc);

            if (updateExistEvent && handler != null)
                _genericEvents.TryUpdate(key, eventFunc, handler);
        }

        #endregion
        #region Unsubscribers

        public static void Unsubscribe(Enum key, EventFunc eventFunc)
        {
            if (_nonGenericEvents.TryGetValue(key, out var handler))
                handler -= eventFunc;

            if (handler != null && handler.GetInvocationList().Length == 0)
                _nonGenericEvents.Remove(key, out var _);
        }

        public static void Unsubscribe(Enum key, List<EventFunc> eventFuncs)
        {
            if (_nonGenericEvents.TryGetValue(key, out var handler))
                eventFuncs.ForEach(e => handler -= e);

            if (handler != null && handler.GetInvocationList().Length == 0)
                _nonGenericEvents.Remove(key, out var _);
        }

        public static void Unsubscribe<T>(Enum key, EventFunc<T> eventFunc)
        {
            if (_genericEvents.TryGetValue(key, out var handler))
            {
                if (handler is EventFunc<T> func)
                {
                    func -= eventFunc;
                }
            }

            if (handler != null && handler.GetInvocationList().Length == 0)
                _genericEvents.Remove(key, out var _);
        }
        public static void UnsubscribeAll(Enum key)
        {
            if (_genericEvents.TryGetValue(key, out var _))
                _genericEvents.Remove(key, out var _);

            if (_nonGenericEvents.TryGetValue(key, out var handlers))
                _nonGenericEvents.Remove(key, out var _);
        }

        public static void Clear()
        {
            _nonGenericEvents.Clear();
            _genericEvents.Clear();
        }

        #endregion
        #region Publishers
        public static void Publish(Enum key, object sender, object arg)
        {
            if (_nonGenericEvents.TryGetValue(key, out var handler))
                handler(sender, arg);
        }

        public static T Publish<T>(Enum key, object sender, object arg)
        {
            if (_genericEvents.TryGetValue(key, out var handler))
            {
                if (handler is EventFunc<T> genericHandler)
                    return genericHandler(sender, arg);
            }

            return default;
        }
        #endregion

        #region Extensions
        public enum EventType
        {
            IsVoid, IsGeneric, IsBoth
        }

        public static bool HasEvent(Enum key, EventType eventType = EventType.IsBoth)
        {
            return eventType switch
            {
                EventType.IsVoid => _nonGenericEvents.ContainsKey(key),
                EventType.IsGeneric => _genericEvents.ContainsKey(key),
                EventType.IsBoth => _nonGenericEvents.ContainsKey(key) && _genericEvents.ContainsKey(key),
                _ => false
            };
        }

        public static EventType GetEventType(Enum key)
        {
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

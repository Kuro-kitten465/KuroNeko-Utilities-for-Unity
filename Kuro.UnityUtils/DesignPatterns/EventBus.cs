using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    public interface IEvent { }

    internal interface IEventBinding<TEvent>
    {
        Action<TEvent> EventWithArgs { get; }
        Action EventWithoutArgs { get; }
        void Add(Action<TEvent> eventWithArgs);
        void Add(Action eventWithoutArgs);
        void Remove(Action<TEvent> eventWithArgs);
        void Remove(Action eventWithoutArgs);
        bool HasListeners { get; }
    }

    public class EventBinding<TEvent> : IEventBinding<TEvent> where TEvent : IEvent
    {
        private Action<TEvent> _eventWithArgs = _ => { };
        private Action _eventWithoutArgs = () => { };
        private int _listenerCount;

        public Action<TEvent> EventWithArgs => _eventWithArgs;
        public Action EventWithoutArgs => _eventWithoutArgs;
        public bool HasListeners => _listenerCount > 0;

        public EventBinding(Action<TEvent> eventWithArgs)
        {
            _eventWithArgs = eventWithArgs ?? throw new ArgumentNullException(nameof(eventWithArgs));
            _listenerCount++;
        }

        public EventBinding(Action eventWithoutArgs)
        {
            _eventWithoutArgs = eventWithoutArgs ?? throw new ArgumentNullException(nameof(eventWithoutArgs));
            _listenerCount++;
        }

        public void Add(Action<TEvent> eventWithArgs)
        {
            _eventWithArgs += eventWithArgs ?? throw new ArgumentNullException(nameof(eventWithArgs));
            _listenerCount++;
        }

        public void Remove(Action<TEvent> eventWithArgs)
        {
            if (eventWithArgs == null) return;
            _eventWithArgs -= eventWithArgs;
            _listenerCount = Math.Max(0, _listenerCount - 1);
        }

        public void Add(Action eventWithoutArgs)
        {
            _eventWithoutArgs += eventWithoutArgs ?? throw new ArgumentNullException(nameof(eventWithoutArgs));
            _listenerCount++;
        }

        public void Remove(Action eventWithoutArgs)
        {
            if (eventWithoutArgs == null) return;
            _eventWithoutArgs -= eventWithoutArgs;
            _listenerCount = Math.Max(0, _listenerCount - 1);
        }
    }

    public static class EventBus<TEvent> where TEvent : IEvent
    {
        private static readonly ConcurrentDictionary<Type, IEventBinding<TEvent>> _bindings = new();
        private static Type Key => typeof(TEvent);

        public static void Register(Action<TEvent> listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            _bindings.AddOrUpdate(
                Key,
                new EventBinding<TEvent>(listener),
                (_, existing) =>
                {
                    existing.Add(listener);
                    return existing;
                });
        }

        public static void Register(Action listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            _bindings.AddOrUpdate(
                Key,
                new EventBinding<TEvent>(listener),
                (_, existing) =>
                {
                    existing.Add(listener);
                    return existing;
                });
        }

        public static void Unregister(Action<TEvent> listener)
        {
            if (listener == null) return;
            if (_bindings.TryGetValue(Key, out var binding))
            {
                binding.Remove(listener);
                if (!binding.HasListeners)
                {
                    _bindings.TryRemove(Key, out _);
                }
            }
        }

        public static void Unregister(Action listener)
        {
            if (listener == null) return;
            if (_bindings.TryGetValue(Key, out var binding))
            {
                binding.Remove(listener);
                if (!binding.HasListeners)
                {
                    _bindings.TryRemove(Key, out _);
                }
            }
        }

        public static void Raise(TEvent evt)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));
            if (_bindings.TryGetValue(Key, out var binding))
            {
                try
                {
                    binding.EventWithArgs?.Invoke(evt);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error raising event {typeof(TEvent).Name}: {ex}");
                }
            }
        }

        public static void Raise()
        {
            if (_bindings.TryGetValue(Key, out var binding))
            {
                try
                {
                    binding.EventWithoutArgs?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error raising event {typeof(TEvent).Name}: {ex}");
                }
            }
        }

        public static void Clear() => _bindings.Clear();
        public static bool HasListeners => _bindings.ContainsKey(Key);
    }
}

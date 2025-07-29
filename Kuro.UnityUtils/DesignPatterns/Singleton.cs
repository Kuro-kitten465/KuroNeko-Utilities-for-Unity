using System;
using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{

    public abstract class LegacySingleton<T> where T : LegacySingleton<T>, IDisposable, new()
    {
        private static readonly object _lock = new();
        public static bool IsInitialized => _instance != null;
        public static Action<T> OnInstanceCreated;
        public static Action<T> OnInstanceDestroy;
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_lock)
                {
                    _instance = new T();
                    OnInstanceCreated?.Invoke(_instance);
                    return _instance;
                }
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                OnInstanceDestroy?.Invoke(_instance);
                _instance = null;
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LegacySingleton() => Dispose(false);
    }

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly object _lock = new();
        private static T _instance;
        public static bool IsInitialized => _instance != null;
        public static event Action<T> OnInstanceCreated;
        public static event Action<T> OnInstanceDestroy;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_lock)
                {
                    _instance = FindFirstObjectByType<T>();

                    if (_instance == null)
                    {
                        var go = new GameObject($"{typeof(T).Name} Instance");
                        _instance = go.AddComponent<T>();
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            OnInstanceCreated?.Invoke(_instance);
        }

        public T Persistent()
        {
            if (_instance != null && _instance == this) DontDestroyOnLoad(gameObject);
            return _instance;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                OnInstanceDestroy?.Invoke(_instance);
                OnInstanceCreated = null;
                OnInstanceDestroy = null;
                _instance = null;
            }
        }
    }
}


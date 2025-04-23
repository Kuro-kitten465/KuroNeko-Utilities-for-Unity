using UnityEngine;

namespace Kuro.UnityUtils.DesignPatterns
{
    public abstract class LegacySingleton<T> where T : LegacySingleton<T>, new()
    {
        private static readonly object _lock = new();
        public static bool IsInitialized => _instance is not null;
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance is not null) return _instance;

                throw new System.InvalidOperationException(
                    $"Singleton of type {typeof(T).Name} is not initialized. Call CreateInstance() first."
                );
            }
        }

        public static T CreateInstance()
        {
            if (_instance is not null) return _instance;

            lock (_lock)
            {
                _instance = new T();
                _instance.OnInstanceCreated();
                return _instance;
            }
        }

        public static void DestroyInstance()
        {
            lock (_lock)
            {
                if (_instance is null) return;

                _instance.OnInstanceDestroy();
                _instance = null;
            }
        }

        protected virtual void OnInstanceCreated() { }
        protected virtual void OnInstanceDestroy() { }
    }

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly object _lock = new();
        public static bool IsInitialized => _instance is not null;
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance is not null) return _instance;

                _instance = FindFirstObjectByType<T>();
                if (_instance is not null) return _instance;

                lock (_lock)
                {
                    var singletonObject = new GameObject($"{typeof(T).Name} Instance");
                    _instance = singletonObject.AddComponent<T>();
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance is not null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            OnInstanceCreated();
        }

        public static void DestroyInstance()
        {
            if (_instance is null) return;

            Destroy(_instance.gameObject);
        }

        protected virtual void OnInstanceCreated() { }
        protected virtual void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }

    public abstract class MonoSingletonPersistent<T> : MonoSingleton<T> where T : MonoBehaviour, new()
    {
        protected new void Awake()
        {
            if (_instance is not null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            OnInstanceCreated();
            DontDestroyOnLoad(gameObject);
        }
    }
}

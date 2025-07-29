using UnityEngine;

namespace Kuro.UnityUtils
{
    [AddComponentMenu("KuroUtils/Logger")]
    public class Logger : MonoBehaviour
    {
        [SerializeField] private bool _showLog = true;

        public void Log(object message, Object context = null)
        {
            if (_showLog)
                Debug.Log(message, context);
        }

        public void LogWarning(object message, Object context = null)
        {
            if (_showLog)
                Debug.LogWarning(message, context);
        }

        public void LogError(object message, Object context = null)
        {
            if (_showLog)
                Debug.LogError(message, context);
        }

        public void LogException(System.Exception exception, Object context = null)
        {
            if (_showLog)
                Debug.LogException(exception, context);
        }

        public void LogFormat(string format, params object[] args)
        {
            if (_showLog)
                Debug.LogFormat(format, args);
        }

        public void LogInfo(object message, Object context = null)
        {
            if (_showLog)
                Debug.Log(message, context);
        }
    }
}
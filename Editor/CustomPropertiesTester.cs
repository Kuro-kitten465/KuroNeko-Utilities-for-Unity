using Kuro.UnityUtils.Security.Primitives;
using UnityEngine;

namespace Kuro.UnityUtils.Editor
{
    public class CustomPropertiesTester : MonoBehaviour
    {
        public SecureBool _secureBool = false;
        [SerializeField] private SecureByte _secureByte = 0;
        [SerializeField] private SecureFloat _secureFloat = 0;
        [SerializeField] private SecureDouble _secureDouble = 0;
        //[SerializeField] private SecureDecimal _secureDecimal = 0;
        [SerializeField] private SecureInt16 _secureInt16 = 0;
        [SerializeField] private SecureInt32 _secureInt32 = 0;
        [SerializeField] private SecureInt64 _secureInt64 = 0;
        [SerializeField] private SecureSByte _secureSByte = 0;
        [SerializeField] private SecureUInt16 _secureUInt16 = 0;
        [SerializeField] private SecureUInt32 _secureUInt32 = 0;
        [SerializeField] private SecureUInt64 _secureUInt64 = 0;

        [SerializeField] private Logger _logger;

        private void OnGUI()
        {
            if (GUILayout.Button("Test"))
                TestValue();    
        }

        private void TestValue()
        {
            _logger.Log($"{nameof(_secureBool)}: {_secureBool}");
            _logger.Log($"{nameof(_secureByte)}: {_secureByte}");
            _logger.Log($"{nameof(_secureFloat)}: {_secureFloat}");
            _logger.Log($"{nameof(_secureInt16)}: {_secureInt16}");
            _logger.Log($"{nameof(_secureInt32)}: {_secureInt32}");
            _logger.Log($"{nameof(_secureInt64)}: {_secureInt64}");
            _logger.Log($"{nameof(_secureSByte)}: {_secureSByte}");
            _logger.Log($"{nameof(_secureUInt16)}: {_secureUInt16}");
            _logger.Log($"{nameof(_secureUInt32)}: {_secureUInt32}");
            _logger.Log($"{nameof(_secureUInt64)}: {_secureUInt64}");
        }
    }
}

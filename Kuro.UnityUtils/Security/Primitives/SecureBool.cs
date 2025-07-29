using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    
    [Serializable]
    public struct SecureBool : ISerializationCallbackReceiver, IComparable, IComparable<SecureBool>, IEquatable<SecureBool>
    {
        private const byte TRUE_PATTERN = 0b01010101;
        private const byte FALSE_PATTERN = 0b10101010;
        private static readonly System.Random _random = new();

        [SerializeField] private byte _obfuscated;
        [SerializeField] private byte _key;
        [SerializeField] private bool _modifier;

        private readonly bool Decrypted
        {
            get
            {
                byte decrypted = (byte)(_obfuscated ^ _key);
                return decrypted == TRUE_PATTERN;
            }
        }

        public SecureBool(bool value)
        {
            _key = (byte)_random.Next(byte.MinValue, byte.MaxValue);
            byte pattern = value ? TRUE_PATTERN : FALSE_PATTERN;
            _obfuscated = (byte)(pattern ^ _key);
            _modifier = value;
        }

        public SecureBool(string value)
        {
            bool parsed = ParseBoolString(value);
            _key = (byte)_random.Next(byte.MinValue, byte.MaxValue);
            byte pattern = parsed ? TRUE_PATTERN : FALSE_PATTERN;
            _obfuscated = (byte)(pattern ^ _key);
            _modifier = parsed;
        }

        private static bool ParseBoolString(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            return value.ToLowerInvariant() switch
            {
                "true" or "1" or "yes" or "y" or "on" or "enabled" => true,
                "false" or "0" or "no" or "n" or "off" or "disabled" => false,
                _ => bool.Parse(value)
            };
        }

        public bool Value
        {
            readonly get => Decrypted;
            set
            {
                _key = (byte)_random.Next(byte.MinValue, byte.MaxValue);
                byte pattern = value ? TRUE_PATTERN : FALSE_PATTERN;
                _obfuscated = (byte)(pattern ^ _key);
                _modifier = value;
            }
        }

        public static implicit operator bool(SecureBool val) => val.Value;
        public static implicit operator SecureBool(bool val) => new(val);
        public static implicit operator SecureBool(string val) => new(val);

        public void OnBeforeSerialize() => _modifier = Decrypted;
        public void OnAfterDeserialize()
        {
            _key = (byte)_random.Next(byte.MinValue, byte.MaxValue);
            byte pattern = _modifier ? TRUE_PATTERN : FALSE_PATTERN;
            _obfuscated = (byte)(pattern ^ _key);
        }

        public readonly int CompareTo(SecureBool other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureBool other) return CompareTo(other);
            if (obj is bool boolValue) return Value.CompareTo(boolValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureBool)} or {nameof(Boolean)}", nameof(obj));
        }

        public readonly bool Equals(SecureBool other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureBool secure) return Equals(secure);
            if (obj is bool boolValue) return Value == boolValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);

        public readonly byte ToPattern() => Value ? TRUE_PATTERN : FALSE_PATTERN;
        public static SecureBool FromPattern(byte pattern) => new(pattern == TRUE_PATTERN);
    }
}

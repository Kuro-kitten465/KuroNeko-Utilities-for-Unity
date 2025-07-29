using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureInt32 : ISerializationCallbackReceiver, IComparable, IComparable<SecureInt32>, IEquatable<SecureInt32>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private int _obfuscated;
        [SerializeField] private int _key;
        [SerializeField] private int _modifier;

        private readonly int Decrypted => _obfuscated ^ _key;

        public SecureInt32(int value)
        {
            _key = _random.Next(int.MinValue, int.MaxValue);
            _obfuscated = value ^ _key;
            _modifier = value;
        }

        public int Value
        {
            readonly get => Decrypted;
            set
            {
                _key = _random.Next(int.MinValue, int.MaxValue);
                _obfuscated = value ^ _key;
                _modifier = value;
            }
        }

        public const int MaxValue = int.MaxValue;
        public const int MinValue = int.MinValue;

        public static implicit operator int(SecureInt32 val) => val.Value;
        public static implicit operator SecureInt32(int val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            _key = _random.Next(int.MinValue, int.MaxValue);
            _obfuscated = _modifier ^ _key;
        }

        public readonly int CompareTo(SecureInt32 other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureInt32 other) return CompareTo(other);
            if (obj is int intValue) return Value.CompareTo(intValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureInt32)} or {nameof(Int32)}", nameof(obj));
        }

        public readonly bool Equals(SecureInt32 other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureInt32 secure) return Equals(secure);
            if (obj is int intValue) return Value == intValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

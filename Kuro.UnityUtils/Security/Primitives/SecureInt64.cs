using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureInt64 : ISerializationCallbackReceiver, IComparable, IComparable<SecureInt64>, IEquatable<SecureInt64>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private long _obfuscated;
        [SerializeField] private long _key;
        [SerializeField] private long _modifier;

        private readonly int Decrypted => (int)(_obfuscated ^ _key);

        public SecureInt64(long value)
        {
            var key = BitConverter.Int32BitsToSingle(_random.Next());
            _key = BitConverter.DoubleToInt64Bits(key);
            _obfuscated = value ^ _key;
            _modifier = value;
        }

        public long Value
        {
            readonly get => Decrypted;
            set
            {
                var key = BitConverter.Int32BitsToSingle(_random.Next());
                _key = BitConverter.DoubleToInt64Bits(key);
                _obfuscated = value ^ _key;
                _modifier = value;
            }
        }

        public const long MaxValue = long.MaxValue;
        public const long MinValue = long.MinValue;

        public static implicit operator long(SecureInt64 val) => val.Value;
        public static implicit operator SecureInt64(long val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            //_key = _random.Next(int.MinValue, int.MaxValue);
            var key = BitConverter.Int32BitsToSingle(_random.Next());
            _key = BitConverter.DoubleToInt64Bits(key);
            _obfuscated = _modifier ^ _key;
        }

        public readonly int CompareTo(SecureInt64 other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureInt64 other) return CompareTo(other);
            if (obj is long longValue) return Value.CompareTo(longValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureInt64)} or {nameof(Int64)}", nameof(obj));
        }

        public readonly bool Equals(SecureInt64 other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureInt64 secure) return Equals(secure);
            if (obj is long longValue) return Value == longValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

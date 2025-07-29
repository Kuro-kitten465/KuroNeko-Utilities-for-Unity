using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureUInt32 : ISerializationCallbackReceiver, IComparable, IComparable<SecureUInt32>, IEquatable<SecureUInt32>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private uint _obfuscated;
        [SerializeField] private uint _key;
        [SerializeField] private uint _modifier;

        private readonly uint Decrypted => _obfuscated ^ _key;

        public SecureUInt32(uint value)
        {
            uint randomKey = (uint)(_random.Next() & 0x7FFFFFFF) | ((uint)_random.Next(2) << 31);
            _key = randomKey;
            _obfuscated = value ^ _key;
            _modifier = value;
        }

        public uint Value
        {
            readonly get => Decrypted;
            set
            {
                uint randomKey = (uint)(_random.Next() & 0x7FFFFFFF) | ((uint)_random.Next(2) << 31);
                _key = randomKey;
                _obfuscated = value ^ _key;
                _modifier = value;
            }
        }

        public const uint MaxValue = uint.MaxValue;
        public const uint MinValue = uint.MinValue;

        public static implicit operator uint(SecureUInt32 val) => val.Value;
        public static implicit operator SecureUInt32(uint val) => new(val);
        public static explicit operator int(SecureUInt32 val) => (int)val.Value;
        public static explicit operator SecureUInt32(int val) => new((uint)val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            _key = (uint)_random.Next((int)uint.MinValue, int.MaxValue);
            _obfuscated = _modifier ^ _key;
        }

        public readonly int CompareTo(SecureUInt32 other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureUInt32 other) return CompareTo(other);
            if (obj is uint uintValue) return Value.CompareTo(uintValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureUInt32)} or {nameof(UInt32)}", nameof(obj));
        }

        public readonly bool Equals(SecureUInt32 other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureUInt32 secure) return Equals(secure);
            if (obj is uint uintValue) return Value == uintValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

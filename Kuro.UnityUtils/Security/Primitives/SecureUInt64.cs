using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureUInt64 : ISerializationCallbackReceiver, IComparable, IComparable<SecureUInt64>, IEquatable<SecureUInt64>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private ulong _obfuscated;
        [SerializeField] private ulong _key;
        [SerializeField] private ulong _modifier;

        private readonly ulong Decrypted => _obfuscated ^ _key;

        public SecureUInt64(ulong value)
        {
            ulong randomKey = ((ulong)_random.Next() << 32) | (uint)_random.Next();
            _key = randomKey;
            _obfuscated = value ^ _key;
            _modifier = value;
        }

        public ulong Value
        {
            readonly get => Decrypted;
            set
            {
                ulong randomKey = ((ulong)_random.Next() << 32) | (uint)_random.Next();
                _key = randomKey;
                _obfuscated = value ^ _key;
                _modifier = value;
            }
        }

        public const ulong MaxValue = ulong.MaxValue;
        public const ulong MinValue = ulong.MinValue;

        public static implicit operator ulong(SecureUInt64 val) => val.Value;
        public static implicit operator SecureUInt64(ulong val) => new(val);

        public static explicit operator long(SecureUInt64 val) => (long)val.Value;
        public static explicit operator SecureUInt64(long val) => new((ulong)val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            ulong randomKey = ((ulong)_random.Next() << 32) | (uint)_random.Next();
            _key = randomKey;
            _obfuscated = _modifier ^ _key;
        }

        public readonly int CompareTo(SecureUInt64 other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is SecureUInt64 other) return CompareTo(other);
            if (obj is ulong ulongValue) return Value.CompareTo(ulongValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureUInt64)} or {nameof(UInt64)}", nameof(obj));
        }

        public readonly bool Equals(SecureUInt64 other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureUInt64 secure) return Equals(secure);
            if (obj is ulong ulongValue) return Value == ulongValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureInt16 : ISerializationCallbackReceiver, IComparable, IComparable<SecureInt16>, IEquatable<SecureInt16>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private short _obfuscated;
        [SerializeField] private short _key;
        [SerializeField] private short _modifier;

        private readonly short Decrypted => (short)(_obfuscated ^ _key);

        public SecureInt16(short value)
        {
            _key = (short)_random.Next(short.MinValue, short.MaxValue);
            _obfuscated = (short)(value ^ _key);
            _modifier = value;
        }

        public short Value
        {
            readonly get => Decrypted;
            set
            {
                _key = (short)_random.Next(short.MinValue, short.MaxValue);
                _obfuscated = (short)(value ^ _key);
                _modifier = value;
            }
        }

        public const short MaxValue = short.MaxValue;
        public const short MinValue = short.MinValue;

        public static implicit operator short(SecureInt16 val) => val.Value;
        public static implicit operator SecureInt16(short val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            _key = (short)_random.Next(short.MinValue, short.MaxValue);
            _obfuscated = (short)(_modifier ^ _key);
        }

        public readonly int CompareTo(SecureInt16 other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureInt16 other) return CompareTo(other);
            if (obj is short shortValue) return Value.CompareTo(shortValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureInt16)} or {nameof(Int16)}", nameof(obj));
        }

        public readonly bool Equals(SecureInt16 other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureInt16 secure) return Equals(secure);
            if (obj is short shortValue) return Value == shortValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

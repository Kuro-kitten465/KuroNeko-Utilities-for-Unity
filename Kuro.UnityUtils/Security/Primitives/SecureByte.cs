using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureByte : ISerializationCallbackReceiver, IComparable, IComparable<SecureByte>, IEquatable<SecureByte>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private byte _obfuscated;
        [SerializeField] private byte _key;
        [SerializeField] private byte _modifier;

        private readonly byte Decrypted => (byte)(_obfuscated ^ _key);

        public SecureByte(byte value)
        {
            _key = (byte)_random.Next(byte.MinValue, byte.MaxValue);
            _obfuscated = (byte)(value ^ _key);
            _modifier = value;
        }

        public byte Value
        {
            readonly get => Decrypted;
            set
            {
                _key = (byte)_random.Next(byte.MinValue, byte.MaxValue);
                _obfuscated = (byte)(value ^ _key);
                _modifier = value;
            }
        }

        public const byte MaxValue = byte.MaxValue;
        public const byte MinValue = byte.MinValue;

        public static implicit operator byte(SecureByte val) => val.Value;
        public static implicit operator SecureByte(byte val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            _key = (byte)_random.Next(byte.MinValue, byte.MaxValue);
            _obfuscated = (byte)(_modifier ^ _key);
        }

        public readonly int CompareTo(SecureByte other) => (byte)Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureByte other) return CompareTo(other);
            if (obj is byte byteValue) return (byte)Value.CompareTo(byteValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureByte)} or {nameof(Byte)}", nameof(obj));
        }

        public readonly bool Equals(SecureByte other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureByte secure) return Equals(secure);
            if (obj is byte byteValue) return Value == byteValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

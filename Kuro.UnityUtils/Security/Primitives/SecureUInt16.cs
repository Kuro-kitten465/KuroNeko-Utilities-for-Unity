using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureUInt16 : ISerializationCallbackReceiver, IComparable, IComparable<SecureUInt16>, IEquatable<SecureUInt16>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private ushort _obfuscated;
        [SerializeField] private ushort _key;
        [SerializeField] private ushort _modifier;

        private readonly ushort Decrypted => (ushort)(_obfuscated ^ _key);

        public SecureUInt16(ushort value)
        {
            _key = (ushort)_random.Next(ushort.MinValue, ushort.MaxValue);
            _obfuscated = (ushort)(value ^ _key);
            _modifier = value;
        }

        public ushort Value
        {
            readonly get => Decrypted;
            set
            {
                _key = (ushort)_random.Next(ushort.MinValue, ushort.MaxValue);
                _obfuscated = (ushort)(value ^ _key);
                _modifier = value;
            }
        }

        public const ushort MaxValue = ushort.MaxValue;
        public const ushort MinValue = ushort.MinValue;

        public static implicit operator ushort(SecureUInt16 val) => val.Value;
        public static implicit operator SecureUInt16(ushort val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            _key = (ushort)_random.Next(ushort.MinValue, short.MaxValue);
            _obfuscated = (ushort)(_modifier ^ _key);
        }

        public readonly int CompareTo(SecureUInt16 other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureUInt16 other) return CompareTo(other);
            if (obj is ushort ushortValue) return Value.CompareTo(ushortValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureUInt16)} or {nameof(UInt16)}", nameof(obj));
        }

        public readonly bool Equals(SecureUInt16 other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureUInt16 secure) return Equals(secure);
            if (obj is ushort ushortValue) return Value == ushortValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

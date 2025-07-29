using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureSByte : ISerializationCallbackReceiver, IComparable, IComparable<SecureSByte>, IEquatable<SecureSByte>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private sbyte _obfuscated;
        [SerializeField] private sbyte _key;
        [SerializeField] private sbyte _modifier;

        private readonly sbyte Decrypted => (sbyte)(_obfuscated ^ _key);

        public SecureSByte(sbyte value)
        {
            _key = (sbyte)_random.Next(sbyte.MinValue, sbyte.MaxValue);
            _obfuscated = (sbyte)(value ^ _key);
            _modifier = value;
        }

        public sbyte Value
        {
            readonly get => Decrypted;
            set
            {
                _key = (sbyte)_random.Next(sbyte.MinValue, sbyte.MaxValue);
                _obfuscated = (sbyte)(value ^ _key);
                _modifier = value;
            }
        }

        public const sbyte MaxValue = sbyte.MaxValue;
        public const sbyte MinValue = sbyte.MinValue;

        public static implicit operator sbyte(SecureSByte val) => val.Value;
        public static implicit operator SecureSByte(sbyte val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            _key = (sbyte)_random.Next(sbyte.MinValue, sbyte.MaxValue);
            _obfuscated = (sbyte)(_modifier ^ _key);
        }

        public readonly int CompareTo(SecureSByte other) => (byte)Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureSByte other) return CompareTo(other);
            if (obj is sbyte sbyteValue) return (byte)Value.CompareTo(sbyteValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureSByte)} or {nameof(SByte)}", nameof(obj));
        }

        public readonly bool Equals(SecureSByte other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureSByte secure) return Equals(secure);
            if (obj is sbyte sbyteValue) return Value == sbyteValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureFloat : ISerializationCallbackReceiver, IComparable, IComparable<SecureFloat>, IEquatable<SecureFloat>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private float _obfuscated;
        [SerializeField] private float _key;
        [SerializeField] private float _modifier;

        private readonly float Decrypted
        {
            get
            {
                var obfuscatedBits = BitConverter.SingleToInt32Bits(_obfuscated);
                var keyBits = BitConverter.SingleToInt32Bits(_key);
                return BitConverter.Int32BitsToSingle(obfuscatedBits ^ keyBits);
            }
        }

        public SecureFloat(float value)
        {
            _key = BitConverter.Int32BitsToSingle(_random.Next());

            var valueBits = BitConverter.SingleToInt32Bits(value);
            var keyBits = BitConverter.SingleToInt32Bits(_key);
            _obfuscated = BitConverter.Int32BitsToSingle(valueBits ^ keyBits);

            _modifier = value;
        }

        public float Value
        {
            readonly get => Decrypted;
            set
            {
                _key = BitConverter.Int32BitsToSingle(_random.Next());
                var valueBits = BitConverter.SingleToInt32Bits(value);
                var keyBits = BitConverter.SingleToInt32Bits(_key);
                _obfuscated = BitConverter.Int32BitsToSingle(valueBits ^ keyBits);
                _modifier = value;
            }
        }

        public const float MaxValue = float.MaxValue;
        public const float MinValue = float.MinValue;
        public const float Epsilon = float.Epsilon;
        public const float NaN = float.NaN;
        public const float NegativeInfinity = float.NegativeInfinity;
        public const float PositiveInfinity = float.PositiveInfinity;

        public static implicit operator float(SecureFloat val) => val.Value;
        public static implicit operator SecureFloat(float val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            _key = BitConverter.Int32BitsToSingle(_random.Next());
            var modifierBits = BitConverter.SingleToInt32Bits(_modifier);
            var keyBits = BitConverter.SingleToInt32Bits(_key);
            _obfuscated = BitConverter.Int32BitsToSingle(modifierBits ^ keyBits);
        }

        public readonly int CompareTo(SecureFloat other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureFloat other) return CompareTo(other);
            if (obj is float floatValue) return Value.CompareTo(floatValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureFloat)} or {nameof(Single)}", nameof(obj));
        }

        public readonly bool Equals(SecureFloat other) => Math.Abs(Value - other.Value) < Epsilon;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureFloat secure) return Equals(secure);
            if (obj is float floatValue) return Math.Abs(Value - floatValue) < Epsilon;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

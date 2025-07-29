using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Serializable]
    public struct SecureDouble : ISerializationCallbackReceiver, IComparable, IComparable<SecureDouble>, IEquatable<SecureDouble>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private double _obfuscated;
        [SerializeField] private double _key;
        [SerializeField] private double _modifier;

        private readonly double Decrypted
        {
            get
            {
                var obfuscatedBits = BitConverter.DoubleToInt64Bits(_obfuscated);
                var keyBits = BitConverter.DoubleToInt64Bits(_key);
                return BitConverter.Int64BitsToDouble(obfuscatedBits ^ keyBits);
            }
        }

        public SecureDouble(double value)
        {
            _key = BitConverter.Int32BitsToSingle(_random.Next());

            var valueBits = BitConverter.DoubleToInt64Bits(value);
            var keyBits = BitConverter.DoubleToInt64Bits(_key);
            _obfuscated = BitConverter.Int64BitsToDouble(valueBits ^ keyBits);

            _modifier = value;
        }

        /*public SecureDouble(float value)
        {
            _key = BitConverter.Int32BitsToSingle(_random.Next());
            var doubleValue = (double)value;

            var valueBits = BitConverter.DoubleToInt64Bits(doubleValue);
            var keyBits = BitConverter.DoubleToInt64Bits(_key);
            _obfuscated = BitConverter.Int64BitsToDouble(valueBits ^ keyBits);

            _modifier = value;
        }*/

        public double Value
        {
            readonly get => Decrypted;
            set
            {
                _key = BitConverter.Int64BitsToDouble(_random.Next());
                var valueBits = BitConverter.DoubleToInt64Bits(value);
                var keyBits = BitConverter.DoubleToInt64Bits(_key);
                _obfuscated = BitConverter.Int64BitsToDouble(valueBits ^ keyBits);
                _modifier = value;
            }
        }

        public const double MaxValue = double.MaxValue;
        public const double MinValue = double.MinValue;
        public const double Epsilon = double.Epsilon;
        public const double NaN = double.NaN;
        public const double NegativeInfinity = double.NegativeInfinity;
        public const double PositiveInfinity = double.PositiveInfinity;

        public static implicit operator float(SecureDouble val) => (float)val.Value;
        public static implicit operator double(SecureDouble val) => val.Value;
        public static implicit operator decimal(SecureDouble val) => (decimal)val.Value;
        public static implicit operator SecureDouble(float val) => new(val);
        public static implicit operator SecureDouble(double val) => new(val);
        public static implicit operator SecureDouble(decimal val) => new(Convert.ToDouble(val));
        public static implicit operator SecureDouble(int val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            _key = BitConverter.Int64BitsToDouble(_random.Next());
            var modifierBits = BitConverter.DoubleToInt64Bits(_modifier);
            var keyBits = BitConverter.DoubleToInt64Bits(_key);
            _obfuscated = BitConverter.Int64BitsToDouble(modifierBits ^ keyBits);
        }

        public readonly int CompareTo(SecureDouble other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (obj is SecureDouble other) return CompareTo(other);
            if (obj is double doubleValue) return Value.CompareTo(doubleValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureDouble)} or {nameof(Double)}", nameof(obj));
        }

        public readonly bool Equals(SecureDouble other) => Math.Abs(Value - other.Value) < Epsilon;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureDouble secure) return Equals(secure);
            if (obj is double doubleValue) return Math.Abs(Value - doubleValue) < Epsilon;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

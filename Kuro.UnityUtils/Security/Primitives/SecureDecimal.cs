using System;
using UnityEngine;

namespace Kuro.UnityUtils.Security.Primitives
{
    [Obsolete("The core of SecureDecimal still not finish due to Unity can't serialized Decimal value.")]
    [Serializable]
    public struct SecureDecimal : ISerializationCallbackReceiver, IComparable, IComparable<SecureDecimal>, IEquatable<SecureDecimal>, IFormattable
    {
        private static readonly System.Random _random = new();

        [SerializeField] private decimal _obfuscated;
        [SerializeField] private decimal _key;
        [SerializeField] private decimal _modifier;

        private readonly decimal Decrypted
        {
            get
            {
                int[] valueInts = decimal.GetBits(_obfuscated);
                int[] keyInts = decimal.GetBits(_key);

                for (int i = 0; i < 4; i++)
                {
                    valueInts[i] ^= keyInts[i];
                }

                return new decimal(valueInts);
            }
        }

        public SecureDecimal(decimal value)
        {
            int[] keyInts = new int[4];
            for (int i = 0; i < 4; i++)
            {
                keyInts[i] = _random.Next();
            }
            _key = new decimal(keyInts);

            int[] valueInts = decimal.GetBits(value);

            for (int i = 0; i < 4; i++)
            {
                valueInts[i] ^= keyInts[i];
            }

            _obfuscated = new decimal(valueInts);
            _modifier = value;
        }

        public decimal Value
        {
            readonly get => Decrypted;
            set
            {
                int[] keyInts = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    keyInts[i] = _random.Next();
                }
                _key = new decimal(keyInts);

                int[] valueInts = decimal.GetBits(value);

                for (int i = 0; i < 4; i++)
                {
                    valueInts[i] ^= keyInts[i];
                }

                _obfuscated = new decimal(valueInts);
                _modifier = value;
            }
        }

        public const decimal MaxValue = decimal.MaxValue;
        public const decimal MinValue = decimal.MinValue;
        public const decimal One = decimal.One;
        public const decimal OneMinus = decimal.MinusOne;
        public const decimal Zero = decimal.Zero;

        public static implicit operator decimal(SecureDecimal val) => val.Value;
        public static explicit operator double(SecureDecimal val) => (double)val.Value;
        public static explicit operator float(SecureDecimal val) => (float)val.Value;
        public static implicit operator SecureDecimal(decimal val) => new(val);
        public static implicit operator SecureDecimal(int val) => new(val);

        public void OnBeforeSerialize() => _modifier = Value;
        public void OnAfterDeserialize()
        {
            int[] keyInts = new int[4];
            for (int i = 0; i < 4; i++)
            {
                keyInts[i] = _random.Next();
            }
            _key = new decimal(keyInts);

            int[] valueInts = decimal.GetBits(_modifier);

            for (int i = 0; i < 4; i++)
            {
                valueInts[i] ^= keyInts[i];
            }

            _obfuscated = new decimal(valueInts);
        }

        public readonly int CompareTo(SecureDecimal other) => Value.CompareTo(other.Value);
        public readonly int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is SecureDecimal other) return CompareTo(other);
            if (obj is decimal decimalValue) return Value.CompareTo(decimalValue);
            throw new ArgumentException($"Object must be of type {nameof(SecureDecimal)} or {nameof(Decimal)}", nameof(obj));
        }

        public readonly bool Equals(SecureDecimal other) => Value == other.Value;
        public override readonly bool Equals(object obj)
        {
            if (obj is SecureDecimal secure) return Equals(secure);
            if (obj is decimal decimalValue) return Value == decimalValue;
            return false;
        }

        public override readonly int GetHashCode() => Value.GetHashCode();
        public override readonly string ToString() => Value.ToString();
        public readonly string ToString(string format) => Value.ToString(format);
        public readonly string ToString(IFormatProvider provider) => Value.ToString(provider);
        public readonly string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}

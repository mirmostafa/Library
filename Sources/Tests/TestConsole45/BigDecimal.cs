using System;
using System.Numerics;

namespace TestConsole45
{
    /// <summary>
    ///     Arbitrary precision decimal.
    ///     All operations are exact, except for division. Division never determines more digits than the given precision.
    ///     Based on http://stackoverflow.com/a/4524254
    ///     Author: Jan Christoph Bernack (contact: jc.bernack at googlemail.com)
    /// </summary>
    public struct BigDecimal : IComparable, IComparable<BigDecimal>
    {
        /// <summary>
        ///     Specifies whether the significant digits should be truncated to the given precision after each operation.
        /// </summary>
        public static bool AlwaysTruncate = false;

        /// <summary>
        ///     Sets the maximum precision of division operations.
        ///     If AlwaysTruncate is set to true all operations are affected.
        /// </summary>
        public static int Precision = 50;

        public BigDecimal(BigInteger mantissa, int exponent)
            : this()
        {
            this.Mantissa = mantissa;
            this.Exponent = exponent;
            this.Normalize();
            if (AlwaysTruncate)
                this.Truncate();
        }

        public BigInteger Mantissa { get; set; }
        public int Exponent { get; set; }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is BigDecimal))
                throw new ArgumentException();
            return this.CompareTo((BigDecimal) obj);
        }

        public int CompareTo(BigDecimal other) => this < other ? -1 : (this > other ? 1 : 0);

        /// <summary>
        ///     Removes trailing zeros on the mantissa
        /// </summary>
        public void Normalize()
        {
            if (this.Mantissa.IsZero)
            {
                this.Exponent = 0;
            }
            else
            {
                BigInteger remainder = 0;
                while (remainder == 0)
                {
                    var shortened = BigInteger.DivRem(this.Mantissa, 10, out remainder);
                    if (remainder != 0)
                        continue;
                    this.Mantissa = shortened;
                    this.Exponent++;
                }
            }
        }

        /// <summary>
        ///     Truncate the number to the given precision by removing the least significant digits.
        /// </summary>
        /// <returns>The truncated number</returns>
        public BigDecimal Truncate(int precision)
        {
            // copy this instance (remember its a struct)
            var shortened = this;
            // save some time because the number of digits is not needed to remove trailing zeros
            shortened.Normalize();
            // remove the least significant digits, as long as the number of digits is higher than the given Precision
            while (NumberOfDigits(shortened.Mantissa) > precision)
            {
                shortened.Mantissa /= 10;
                shortened.Exponent++;
            }
            return shortened;
        }

        public BigDecimal Truncate() => this.Truncate(Precision);
        private static int NumberOfDigits(BigInteger value) => (value * value.Sign).ToString().Length;
        public override string ToString() => string.Concat(this.Mantissa.ToString(), "E", this.Exponent);
        public bool Equals(BigDecimal other) => other.Mantissa.Equals(this.Mantissa) && other.Exponent == this.Exponent;
        public override bool Equals(object obj) => !ReferenceEquals(null, obj) && obj is BigDecimal && this.Equals((BigDecimal) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Mantissa.GetHashCode() * 397) ^ this.Exponent;
            }
        }

        public static implicit operator BigDecimal(int value) => new BigDecimal(value, 0);

        public static implicit operator BigDecimal(double value)
        {
            var mantissa = (BigInteger) value;
            var exponent = 0;
            double scaleFactor = 1;
            while (Math.Abs(value * scaleFactor - (double) mantissa) > 0)
            {
                exponent -= 1;
                scaleFactor *= 10;
                mantissa = (BigInteger) (value * scaleFactor);
            }
            return new BigDecimal(mantissa, exponent);
        }

        public static implicit operator BigDecimal(decimal value)
        {
            var mantissa = (BigInteger) value;
            var exponent = 0;
            decimal scaleFactor = 1;
            while ((decimal) mantissa != value * scaleFactor)
            {
                exponent -= 1;
                scaleFactor *= 10;
                mantissa = (BigInteger) (value * scaleFactor);
            }
            return new BigDecimal(mantissa, exponent);
        }

        public static explicit operator double(BigDecimal value) => (double) value.Mantissa * Math.Pow(10, value.Exponent);

        public static explicit operator float(BigDecimal value) => Convert.ToSingle((double) value);

        public static explicit operator decimal(BigDecimal value) => (decimal) value.Mantissa * (decimal) Math.Pow(10, value.Exponent);

        public static explicit operator int(BigDecimal value) => (int) (value.Mantissa * BigInteger.Pow(10, value.Exponent));

        public static explicit operator uint(BigDecimal value) => (uint) (value.Mantissa * BigInteger.Pow(10, value.Exponent));

        public static BigDecimal operator +(BigDecimal value) => value;

        public static BigDecimal operator -(BigDecimal value)
        {
            value.Mantissa *= -1;
            return value;
        }

        public static BigDecimal operator ++(BigDecimal value) => value + 1;

        public static BigDecimal operator --(BigDecimal value) => value - 1;

        public static BigDecimal operator +(BigDecimal left, BigDecimal right) => Add(left, right);

        public static BigDecimal operator -(BigDecimal left, BigDecimal right) => Add(left, -right);

        private static BigDecimal Add(BigDecimal left, BigDecimal right)
        {
            return left.Exponent > right.Exponent
                ? new BigDecimal(AlignExponent(left, right) + right.Mantissa, right.Exponent)
                : new BigDecimal(AlignExponent(right, left) + left.Mantissa, left.Exponent);
        }

        public static BigDecimal operator *(BigDecimal left, BigDecimal right) => new BigDecimal(left.Mantissa * right.Mantissa, left.Exponent + right.Exponent);

        public static BigDecimal operator /(BigDecimal dividend, BigDecimal divisor)
        {
            var exponentChange = Precision - (NumberOfDigits(dividend.Mantissa) - NumberOfDigits(divisor.Mantissa));
            if (exponentChange < 0)
                exponentChange = 0;
            dividend.Mantissa *= BigInteger.Pow(10, exponentChange);
            return new BigDecimal(dividend.Mantissa / divisor.Mantissa, dividend.Exponent - divisor.Exponent - exponentChange);
        }

        public static bool operator ==(BigDecimal left, BigDecimal right) => left.Exponent == right.Exponent && left.Mantissa == right.Mantissa;

        public static bool operator !=(BigDecimal left, BigDecimal right) => left.Exponent != right.Exponent || left.Mantissa != right.Mantissa;

        public static bool operator <(BigDecimal left, BigDecimal right)
            => left.Exponent > right.Exponent ? AlignExponent(left, right) < right.Mantissa : left.Mantissa < AlignExponent(right, left);

        public static bool operator >(BigDecimal left, BigDecimal right)
            => left.Exponent > right.Exponent ? AlignExponent(left, right) > right.Mantissa : left.Mantissa > AlignExponent(right, left);

        public static bool operator <=(BigDecimal left, BigDecimal right)
            => left.Exponent > right.Exponent ? AlignExponent(left, right) <= right.Mantissa : left.Mantissa <= AlignExponent(right, left);

        public static bool operator >=(BigDecimal left, BigDecimal right)
            => left.Exponent > right.Exponent ? AlignExponent(left, right) >= right.Mantissa : left.Mantissa >= AlignExponent(right, left);

        /// <summary>
        ///     Returns the mantissa of value, aligned to the exponent of reference.
        ///     Assumes the exponent of value is larger than of reference.
        /// </summary>
        private static BigInteger AlignExponent(BigDecimal value, BigDecimal reference) => value.Mantissa * BigInteger.Pow(10, value.Exponent - reference.Exponent);

        public static BigDecimal Exp(double exponent)
        {
            var tmp = (BigDecimal) 1;
            while (Math.Abs(exponent) > 100)
            {
                var diff = exponent > 0 ? 100 : -100;
                tmp *= Math.Exp(diff);
                exponent -= diff;
            }
            return tmp * Math.Exp(exponent);
        }

        public static BigDecimal Pow(double basis, double exponent)
        {
            var tmp = (BigDecimal) 1;
            while (Math.Abs(exponent) > 100)
            {
                var diff = exponent > 0 ? 100 : -100;
                tmp *= Math.Pow(basis, diff);
                exponent -= diff;
            }
            return tmp * Math.Pow(basis, exponent);
        }
    }
}
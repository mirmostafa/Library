#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about numbers (int, float, double, ...)
    /// </summary>
    public static class NumericHelper
    {
        #region Fields

        private static readonly Type[] _NumericTypes =
        {
            typeof(short), typeof(int), typeof(long), typeof(double), typeof(float), typeof(decimal), typeof(ushort), typeof(uint), typeof(ulong), typeof(byte)
        };

        #endregion

        public static bool AllHaveValues(params int?[] numbers) => numbers.All(n => n.HasValue);
        public static long Between(this long value, long min, long max, long defaultValue) => value.IsBetween(min, max) ? value : defaultValue;
        public static int BinToDec(string binaryNumber) => ConvertBase(binaryNumber, 2, 10).ToIntNullable() ?? 0;
        public static string BinToHex(string binaryNumber) => ConvertBase(binaryNumber, 2, 16);

        public static string BytesToHex(byte[] bytes) => BytesToHex(bytes, false);

        public static string BytesToHex(byte[] bytes, bool removeDashes)
        {
            var str = BitConverter.ToString(bytes);
            if (removeDashes)
            {
                str = str.Replace("-", "");
            }

            return str;
        }

        public static string ConvertBase(string num, int sourceBase, int desctinationBase) =>
            Convert.ToString(Convert.ToInt32(num, sourceBase), desctinationBase);

        public static double FromSciToDouble(string obj) => double.Parse(obj, NumberStyles.Float);

        public static IEnumerable<int> GetPrimes(int min, int max) =>
            Enumerable.Range(min, max - min).Where(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));

        public static string HexToBin(string hex) => ConvertBase(hex, 16, 2);

        public static byte[] HexToBytes(string hex)
        {
            var str = hex.Replace("-", string.Empty);
            var numArray = new byte[str.Length / 2];
            var num1 = 4;
            var index = 0;
            foreach (var num2 in str)
            {
                var num3 = (num2 - 48) % 32;
                if (num3 > 9)
                {
                    num3 -= 7;
                }

                numArray[index] |= (byte)(num3 << num1);
                num1 ^= 4;
                if (num1 != 0)
                {
                    ++index;
                }
            }

            return numArray;
        }

        public static int HexToDec(string hexNumber) => Convert.ToInt32(hexNumber, 16);
        public static bool IsBetween(this int value, int min, int max) => value >= min && value <= max;
        public static bool IsBetween(this long value, long min, long max) => value >= min && value <= max;
        public static bool IsBiggerThan(this int value, int min) => value > min;
        public static bool IsBiggerThan(this long value, long max) => value > max;
        public static bool IsLessThan(this long value, long min) => value < min;
        public static bool IsLessThan(this int value, int max) => value < max;

        public static bool IsNumeric(this Type type)
        {
            if (type.Name == "Nullable`1")
            {
                type = type.GetGenericArguments()[0];
            }

            return _NumericTypes.Contains(type);
        }

        public static bool IsPositive(this int value) => value.IsBiggerThan(0);
        public static bool IsPrime(this int value) => IsPrime(value.ToLong());

        public static bool IsPrime(this long value)
        {
            if (value < 2)
            {
                return false;
            }

            if (value % 2 == 0)
            {
                return false;
            }

            for (var i = 3; i < Math.Sqrt(value); i = i + 2)
            {
                if (value % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static string Merge(char quat, char separator, params int[] array)
        {
            string[] result = {string.Empty};
            array.ForEach(num => result[0] += quat + num.ToString() + quat + separator + " ");
            result[0] = result[0].Trim();
            return result[0].Substring(0, result[0].Length - 1);
        }

        public static int SetFlag(int num, int index, bool set)
        {
            var byt = Convert.ToString(num, 2);
            var boolArray = byt.ToCharArray().Select(c => c.ToString().Equals("1")).ToArray();
            var bArray = new BitArray(boolArray);
            bArray.Set(index, set);
            var value = (int[])bArray.GetType().GetField("m_array", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(bArray);
            if (value != null)
            {
                return value[0];
            }

            throw new InvalidOperationException();
        }

        public static string ToBin(this int num) => Convert.ToString(num, 2);

        public static string ToCommaSeparate(this double num) => double.Parse(num.ToString(CultureInfo.InvariantCulture))
            .ToString("N",
                new NumberFormatInfo
                {
                    NumberDecimalDigits =
                        0
                });

        public static string ToCommaSeparate(this long num) => long.Parse(num.ToString(CultureInfo.InvariantCulture))
            .ToString("N",
                new NumberFormatInfo
                {
                    NumberDecimalDigits = 0
                });

        public static string ToCommaSeparate(this int num) => int.Parse(num.ToString(CultureInfo.InvariantCulture))
            .ToString("N",
                new NumberFormatInfo
                {
                    NumberDecimalDigits = 0
                });

        public static double? ToDouble(string str) => double.TryParse(str, out var num) ? num : default(double?);
        public static double ToDouble(string str, double defaultValue) => ToDouble(str) ?? defaultValue;
        public static string ToHex(this int num) => num.ToString("X");
        public static int ToInt(this object obj) => Convert.ToInt32(obj);

        public static int ToInt(this object obj, int defaultValue) =>
            obj == null ? defaultValue : obj.ToString().ToIntNullable() ?? defaultValue;

        public static int? ToIntNullable(this string str) => int.TryParse(str, out var result) ? result : default(int?);
        public static long ToLong(this object obj) => Convert.ToInt64(obj);

        public static long ToLong(this object obj, long defaultValue) =>
            obj == null ? defaultValue : obj.ToString().ToLongNullable() ?? defaultValue;

        public static long? ToLongNullable(this string str) => long.TryParse(str, out var result) ? result : default(long?);

        public static string ToMesuranceSystem(this int num)
        {
            var a = num.ToString().Length / 3 - 1;
            var measures = new[] {"", "K", "M", "G", "T"};
            return $"{Math.Round(num / Math.Pow(1024, a), 3)} {measures[a]}";
        }

        public static string ToMesuranceSystem(this ulong num)
        {
            if (num == 0)
            {
                return "0";
            }

            var a = num.ToString().Length / 3 - 1;
            var measures = new[] {"", "K", "M", "G", "T"};
            return $"{Math.Round(num / Math.Pow(1024, a), 3)} {measures[a]}";
        }

        public static string ToMesuranceSystem(this long num)
        {
            if (num == 0)
            {
                return "0";
            }

            var a = num.ToString().Length / 3 - 1;
            var mesures = new[] {"", "K", "M", "G", "T"};
            return $"{Math.Round(num / Math.Pow(1024, a), 3)} {mesures[a]}";
        }

        public static string ToOct(this int num) => ConvertBase(num.ToString(), 10, 8);
    }
}
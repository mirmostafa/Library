#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Library35.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about numbers (int, float, double, ...)
	/// </summary>
	public static class NumericHelper
	{
		public static long Between(this long value, long min, long max, long defaultValue)
		{
			return value.IsBetween(min, max) ? value : defaultValue;
		}

		public static int BinToDec(string binaryNumber)
		{
			return ConvertBase(binaryNumber, 2, 10).ToIntNullable().Value;
		}

		public static string BinToHex(string binaryNumber)
		{
			return ConvertBase(binaryNumber, 2, 16);
		}

		public static string ConvertBase(string num, int sourceBase, int desctinationBase)
		{
			return Convert.ToString(Convert.ToInt32(num, sourceBase), desctinationBase);
		}

		public static string HexToBin(string hex)
		{
			return ConvertBase(hex, 16, 2);
		}

		public static int HexToDec(string hexNumber)
		{
			return Convert.ToInt32(hexNumber, 16);
		}

		public static bool IsBetween(this int value, int min, int max)
		{
			return value >= min && value <= max;
		}

		public static bool IsBetween(this long value, long min, long max)
		{
			return value >= min && value <= max;
		}

		public static bool IsLessThan(this long value, long min)
		{
			return value < min;
		}

		public static bool IsBiggerThan(this int value, int min)
		{
			return value > min;
		}

		public static bool IsBiggerThan(this long value, long max)
		{
			return value > max;
		}

		public static bool IsLessThan(this int value, int max)
		{
			return value < max;
		}

		public static bool IsPercent(string str)
		{
			int currValue;
			return (TryParse(str, out currValue) ? ((currValue <= 100) && (currValue >= 0)) : false);
		}

		public static bool IsPrime(this int value)
		{
			if (value < 2)
				return false;
			if (value % 2 == 0)
				return false;
			for (var i = 3; i < Math.Sqrt(value); i = i + 2)
				if (value % i == 0)
					return false;
			return true;
		}

		public static int SetFlag(int num, int index, bool set)
		{
			var byt = Convert.ToString(num, 2);
			var boolArray = byt.ToCharArray().ForEach(c => c.ToString().Equals("1")).ToArray();
			var bArray = new BitArray(boolArray);
			bArray.Set(index, set);
			return ((int[])bArray.GetType().GetField("m_array", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(bArray))[0];
		}

		public static bool IsNumeric(this Type type)
		{
			var numericTypes = new[]
			                   {
				                   typeof (Int16), typeof (Int32), typeof (Int64), typeof (Double), typeof (Single), typeof (Decimal), typeof (UInt16), typeof (UInt32), typeof (UInt64),
				                   typeof (Byte)
			                   };
			if (type.Name == "Nullable`1")
				type = type.GetGenericArguments()[0];

			return numericTypes.Contains(type);
		}

		public static int ToInt(this object obj)
		{
			return Convert.ToInt32(obj);
		}

		public static long ToLong(this object obj)
		{
			return Convert.ToInt64(obj);
		}

		public static long ToLong(this object obj, long defaultValue)
		{
			if (obj == null)
				return defaultValue;
			return obj.ToString().ToLongNullable() ?? defaultValue;
		}

		public static int ToInt(this object obj, int defaultValue)
		{
			if (obj == null)
				return defaultValue;
			return obj.ToString().ToIntNullable() ?? defaultValue;
		}

		public static string ToMesuranceSystem(this int num)
		{
			var a = num.ToString().Length / 3 - 1;
			var mesures = new[]
			              {
				              "", "K", "M", "G", "T"
			              };
			return string.Format("{0} {1}", Math.Round(num / Math.Pow(1024, a), 3), mesures[a]);
		}

		public static string ToMesuranceSystem(this ulong num)
		{
			if (num == 0)
				return "0";
			var a = num.ToString().Length / 3 - 1;
			var mesures = new[]
			              {
				              "", "K", "M", "G", "T"
			              };
			return string.Format("{0} {1}", Math.Round(num / Math.Pow(1024, a), 3), mesures[a]);
		}

		public static string ToMesuranceSystem(this long num)
		{
			if (num == 0)
				return "0";
			var a = num.ToString().Length / 3 - 1;
			var mesures = new[]
			              {
				              "", "K", "M", "G", "T"
			              };
			return string.Format("{0} {1}", Math.Round(num / Math.Pow(1024, a), 3), mesures[a]);
		}

		public static bool TryParse(object obj, out int result)
		{
			ArgHelper.AssertNotNull(obj, "obj");
			return int.TryParse(obj.ToString(), out result);
		}

		public static bool TryParse(object obj, out double result)
		{
			ArgHelper.AssertNotNull(obj, "obj");
			return double.TryParse(obj.ToString(), out result);
		}

		public static bool IsPositive(this int value)
		{
			return value.IsBiggerThan(0);
		}

		public static string Merge(char quat, char separator, params int[] array)
		{
			var result = String.Empty;
			array.ForEach(num => result += quat + num.ToString() + quat + separator + " ");
			result = result.Trim();
			return result.Substring(0, result.Length - 1);
		}

		public static string Separate(this double num)
		{
			//return num.ToString("###,###0");
			var numberFormat = new NumberFormatInfo
			                   {
				                   NumberDecimalDigits = 0
			                   };
			//return Decimal.Parse(num.ToString(), NumberStyles.AllowThousands).ToString("N", numberFormat);
			return double.Parse(num.ToString()).ToString("N", numberFormat);
		}

		public static string ToBin(this int num)
		{
			return ConvertBase(num.ToString(), 10, 2);
		}

		public static double? ToDouble(string str)
		{
			double num;
			return double.TryParse(str, out num) ? num : default(double?);
		}

		public static double ToDouble(string str, double defaultValue)
		{
			return ToDouble(str) ?? defaultValue;
		}

		public static string ToHex(this int num)
		{
			return num.ToString("X");
		}

		public static int? ToIntNullable(this string str)
		{
			int result;
			return int.TryParse(str, out result) ? result : default(int?);
		}

		public static long? ToLongNullable(this string str)
		{
			long result;
			return long.TryParse(str, out result) ? result : default(long?);
		}
	}
}
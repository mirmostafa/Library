#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Globalization;
using Library35.Globalization.DataTypes;
using Library35.Helpers;

namespace Library35.Web.Globalization
{
	partial class FormatHelper
	{
		public static NumericFormatInfo NumericFormat
		{
			get { return Formatter.Info; }
		}

		public static string Format(this double number, FormatOption option)
		{
			switch (option)
			{
				case FormatOption.Number:
					return ToNumber(number);
				case FormatOption.Currency:
					return ToCurrency(number);
				case FormatOption.Percent:
					return ToPercent(number);
				case FormatOption.Cost:
					return ToCost(number);
				default:
					throw new ArgumentOutOfRangeException("option");
			}
		}

		private static string InnerFormat(double number, INumericFormat numericFormat)
		{
			//string result = number.ToString(numericFormat.FormatString, Formatter);
			var result = String.Format(Formatter, string.Concat("{0:", numericFormat.FormatString, "}"), number);

			//Removing 0s at the end of result
			var decimalSeparatorIndex = result.IndexOf(numericFormat.DecimalSeparator);
			var cursor = decimalSeparatorIndex + 1;
			while (result.Length > cursor && char.IsDigit(result[cursor]))
				cursor++;
			cursor--;
			while (char.IsDigit(result[cursor]) && result[cursor].ToString().ToInt() == 0)
			{
				result = result.Remove(cursor, 1);
				cursor--;
			}
			if (result.Length == decimalSeparatorIndex + 1)
				result = result.Remove(decimalSeparatorIndex, 1);
			else if (result.Length > decimalSeparatorIndex + 1 && !char.IsDigit(result[decimalSeparatorIndex + 1]))
				result = result.Remove(decimalSeparatorIndex, 1);

			return result;
		}

		public static string ToPercent(this double number)
		{
			return InnerFormat(number, Formatter.Info.Percent);
		}

		public static string ToCurrency(this double number)
		{
			return InnerFormat(number, NumericFormat.Currency);
		}

		public static string ToNumber(this double number)
		{
			return InnerFormat(number, NumericFormat.Number);
		}

		public static string ToCost(this double number)
		{
			return InnerFormat(number, NumericFormat.Cost);
		}

		public static double FromCost(this string formattedNumber)
		{
			return Formatter.ParseCost(formattedNumber);
		}

		public static double FromCurrency(this string formattedNumber)
		{
			return double.Parse(formattedNumber, NumberStyles.Any, Formatter.Engine);
		}

		public static double FromPercent(this string formattedNumber)
		{
			return double.Parse(formattedNumber, NumberStyles.Any, Formatter.Engine);
		}

		public static double FromNumber(this string formattedNumber)
		{
			return double.Parse(formattedNumber, NumberStyles.Any, Formatter.Engine);
		}

		internal static string ToCurrency(this decimal number)
		{
			return Convert.ToDouble(number).ToCurrency();
		}

		internal static string ToNumber(this decimal number)
		{
			return Convert.ToDouble(number).ToNumber();
		}

		internal static string ToCost(this decimal number)
		{
			return Convert.ToDouble(number).ToCost();
		}

		internal static string ToPercent(this decimal number)
		{
			return Convert.ToDouble(number).ToPercent();
		}
	}
}
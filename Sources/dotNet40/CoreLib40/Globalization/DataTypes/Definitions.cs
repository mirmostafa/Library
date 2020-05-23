#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Library40.Globalization.Attributes;
using Library40.Helpers;

namespace Library40.Globalization.DataTypes
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct PersianDateTimeData
	{
		internal int Year;
		internal int Month;
		internal int Day;
		internal int Hour;
		internal int Minute;
		internal int Second;

		internal bool HasDate;
		internal bool HasTime;

		internal void Init()
		{
			this.Year = this.Month = this.Day = this.Hour = this.Minute = this.Second = -1;
			this.HasDate = true;
			this.HasTime = true;
		}
	}

	public enum PersianMonth
	{
		[LocalizedDescription(cultureName: "fa-IR", description: "›——ÊœÌ‰")]
		[LocalizedDescription(cultureName: "en-US", description: "Farvardin")]
		Farvardin,
		[LocalizedDescription(cultureName: "fa-IR", description: "«—œÌ»Â‘ ")]
		[LocalizedDescription(cultureName: "en-US", description: "Ordibehesht")]
		Ordibehesht,
		[LocalizedDescription(cultureName: "fa-IR", description: "Œ—œ«œ")]
		[LocalizedDescription(cultureName: "en-US", description: "Khordad")]
		Khordad,
		[LocalizedDescription(cultureName: "fa-IR", description: " Ì—")]
		[LocalizedDescription(cultureName: "en-US", description: "Tir")]
		Tir,
		[LocalizedDescription(cultureName: "fa-IR", description: "„—œ«œ")]
		[LocalizedDescription(cultureName: "en-US", description: "Mordad")]
		Mordad,
		[LocalizedDescription(cultureName: "fa-IR", description: "‘Â—ÌÊ—")]
		[LocalizedDescription(cultureName: "en-US", description: "Shahrivar")]
		Sharivar,
		[LocalizedDescription(cultureName: "fa-IR", description: "„Â—")]
		[LocalizedDescription(cultureName: "en-US", description: "Mehr")]
		Mehr,
		[LocalizedDescription(cultureName: "fa-IR", description: "¬»«‰")]
		[LocalizedDescription(cultureName: "en-US", description: "Aban")]
		Aban,
		[LocalizedDescription(cultureName: "fa-IR", description: "¬–—")]
		[LocalizedDescription(cultureName: "en-US", description: "Azar")]
		Azar,
		[LocalizedDescription(cultureName: "fa-IR", description: "œÌ")]
		[LocalizedDescription(cultureName: "en-US", description: "Dey")]
		Dey,
		[LocalizedDescription(cultureName: "fa-IR", description: "»Â„‰")]
		[LocalizedDescription(cultureName: "en-US", description: "Bahman")]
		Bahman,
		[LocalizedDescription(cultureName: "fa-IR", description: "«”›‰œ")]
		[LocalizedDescription(cultureName: "en-US", description: "Esfand")]
		Esfand
	}

	public enum PersianDayOfWeek
	{
		[LocalizedDescription(cultureName: "fa-IR", description: "Ìò‘‰»Â")]
		[LocalizedDescription(cultureName: "en-US", description: "YekShanbeh")]
		YekShanbeh,
		[LocalizedDescription(cultureName: "fa-IR", description: "œÊ‘‰»Â")]
		[LocalizedDescription(cultureName: "en-US", description: "DoShanbeh")]
		DoShanbeh,
		[LocalizedDescription(cultureName: "fa-IR", description: "”Â˛‘‰»Â")]
		[LocalizedDescription(cultureName: "en-US", description: "SehShanbeh")]
		SeShanbeh,
		[LocalizedDescription(cultureName: "fa-IR", description: "çÂ«—‘‰»Â")]
		[LocalizedDescription(cultureName: "en-US", description: "ChaharShanbeh")]
		ChaharShanbeh,
		[LocalizedDescription(cultureName: "fa-IR", description: "Å‰Ã‘‰»Â")]
		[LocalizedDescription(cultureName: "en-US", description: "PanjShanbeh")]
		PanjShanbeh,
		[LocalizedDescription(cultureName: "fa-IR", description: "Ã„⁄Â")]
		[LocalizedDescription(cultureName: "en-US", description: "Jomeh")]
		Jomeh,
		[LocalizedDescription(cultureName: "fa-IR", description: "‘‰»Â")]
		[LocalizedDescription(cultureName: "en-US", description: "Shanbeh")]
		Shanbeh,
	}

	//public enum PercentNegativePattern
	//{
	//    /// <summary>
	//    ///   -n %
	//    /// </summary>
	//    [Description("-n %")]
	//    Value0 = 0,
	//    /// <summary>
	//    ///   -n%
	//    /// </summary>
	//    [Description("-n%")]
	//    Value1 = 1,
	//}

	//public enum CurrencyNegativePattern
	//{
	//    /// <summary>
	//    ///   $n
	//    /// </summary>
	//    [Description("($n)")]
	//    Value0 = 0,
	//    /// <summary>
	//    ///   -$n
	//    /// </summary>
	//    [Description("-$n")]
	//    Value1 = 1,
	//    /// <summary>
	//    ///   $-n
	//    /// </summary>
	//    [Description("$-n")]
	//    Value2 = 2,
	//    /// <summary>
	//    ///   $n-
	//    /// </summary>
	//    [Description("$n-")]
	//    Value3 = 3,
	//    /// <summary>
	//    ///   (n$)
	//    /// </summary>
	//    [Description("(n$)")]
	//    Value4 = 4,
	//    /// <summary>
	//    ///   -n$
	//    /// </summary>
	//    [Description("-n$")]
	//    Value5 = 5,
	//    /// <summary>
	//    ///   n-$
	//    /// </summary>
	//    [Description("n-$")]
	//    Value6 = 6,
	//    /// <summary>
	//    ///   n$-
	//    /// </summary>
	//    [Description("n$-")]
	//    Value7 = 7,
	//    /// <summary>
	//    ///   -n $
	//    /// </summary>
	//    [Description("-n $")]
	//    Value8 = 8,
	//    /// <summary>
	//    ///   -$ n
	//    /// </summary>
	//    [Description("-$ n")]
	//    Value9 = 9,
	//    /// <summary>
	//    ///   n $-
	//    /// </summary>
	//    [Description("n $-")]
	//    Value10 = 10,
	//    /// <summary>
	//    ///   $ n-
	//    /// </summary>
	//    [Description("$ n-")]
	//    Value11 = 11,
	//    /// <summary>
	//    ///   $ -n
	//    /// </summary>
	//    [Description("$ -n")]
	//    Value12 = 12,
	//    /// <summary>
	//    ///   n- $
	//    /// </summary>
	//    [Description("n- $")]
	//    Value13 = 13,
	//    /// <summary>
	//    ///   ($ n)
	//    /// </summary>
	//    [Description("($ n)")]
	//    Value14 = 14,
	//    /// <summary>
	//    ///   (n $)
	//    /// </summary>
	//    [Description("(n $)")]
	//    Value15 = 15,
	//}

	//public enum CurrencyPositivePattern
	//{
	//    /// <summary>
	//    ///   $n
	//    /// </summary>
	//    [Description("$n")]
	//    Value0 = 0,
	//    /// <summary>
	//    ///   n$
	//    /// </summary>
	//    [Description("n$")]
	//    Value1 = 1,
	//    /// <summary>
	//    ///   $ n
	//    /// </summary>
	//    [Description("$ n")]
	//    Value2 = 2,
	//    /// <summary>
	//    ///   n $
	//    /// </summary>
	//    [Description("n $")]
	//    Value3 = 3,
	//}

	//public enum NumberNegativePattern
	//{
	//    /// <summary>
	//    ///   (n)
	//    /// </summary>
	//    [Description("(n)")]
	//    Value0 = 0,
	//    /// <summary>
	//    ///   -n
	//    /// </summary>
	//    [Description("-n")]
	//    Value1 = 1,
	//    /// <summary>
	//    ///   - n
	//    /// </summary>
	//    [Description("- n")]
	//    Value2 = 2,
	//    /// <summary>
	//    ///   n-
	//    /// </summary>
	//    [Description("n-")]
	//    Value3 = 3,
	//    /// <summary>
	//    ///   n -
	//    /// </summary>
	//    [Description("n -")]
	//    Value4 = 4,
	//}

	//public enum PercentPositivePattern
	//{
	//    /// <summary>
	//    ///   n %
	//    /// </summary>
	//    [Description("n %")]
	//    Value0 = 0,
	//    /// <summary>
	//    ///   n%
	//    /// </summary>
	//    [Description("n%")]
	//    Value1 = 1,
	//    /// <summary>
	//    ///   %n
	//    /// </summary>
	//    [Description("%n")]
	//    Value2 = 2,
	//    /// <summary>
	//    ///   % n
	//    /// </summary>
	//    [Description("% n")]
	//    Value3 = 3,
	//}

	public enum FormatOption
	{
		Number,
		Currency,
		Percent,
		Cost
	}

	[Serializable]
	public sealed class NumericFormatInfo
	{
		public NumericFormatInfo(NumberFormat number, PercentFormat percent, CurrencyFormat currency, CostFormat cost)
		{
			this.Number = number;
			this.Percent = percent;
			this.Currency = currency;
			this.Cost = cost;
		}

		//public static NumericFormatInfo Instance
		//{
		//    get { return HttpContext.Current.Session["NumericFormat1"] as NumericFormatInfo; }
		//}

		public NumberFormat Number { get; private set; }
		public PercentFormat Percent { get; private set; }
		public CurrencyFormat Currency { get; private set; }
		public CostFormat Cost { get; set; }

		public static NumberFormatInfo MapToNumberFormatInfo(NumericFormatInfo numericFormatInfo)
		{
			var result = new NumberFormatInfo();
			MapToNumberFormatInfo(result, numericFormatInfo);
			return result;
		}

		public NumberFormatInfo MapToNumberFormatInfo()
		{
			var result = new NumberFormatInfo();
			MapToNumberFormatInfo(result, this);
			return result;
		}

		public void MapToNumberFormatInfo(NumberFormatInfo numberFormatInfo)
		{
			MapToNumberFormatInfo(numberFormatInfo, this);
		}

		public static void MapToNumberFormatInfo(NumberFormatInfo numberFormatInfo, NumericFormatInfo numericFormatInfo)
		{
			numberFormatInfo.CurrencyDecimalDigits = numericFormatInfo.Currency.DecimalDigits;
			numberFormatInfo.CurrencyGroupSeparator = numericFormatInfo.Currency.GroupSeparator.ToString();
			numberFormatInfo.CurrencySymbol = numericFormatInfo.Currency.Symbol;
			numberFormatInfo.CurrencyDecimalSeparator = numericFormatInfo.Currency.DecimalSeparator.ToString();
			numberFormatInfo.CurrencyNegativePattern = numericFormatInfo.Currency.NegativePattern;
			numberFormatInfo.NumberNegativePattern = numericFormatInfo.Number.NegativePattern;
			numberFormatInfo.NumberDecimalDigits = numericFormatInfo.Number.DecimalDigits;
			numberFormatInfo.NumberDecimalSeparator = numericFormatInfo.Number.DecimalSeparator.ToString();
			numberFormatInfo.NumberGroupSeparator = numericFormatInfo.Number.GroupSeparator.ToString();
			numberFormatInfo.PercentDecimalDigits = numericFormatInfo.Percent.DecimalDigits;
			numberFormatInfo.PercentDecimalSeparator = numericFormatInfo.Percent.DecimalSeparator.ToString();
			numberFormatInfo.PercentNegativePattern = numericFormatInfo.Percent.NegativePattern;
			numberFormatInfo.PercentSymbol = numericFormatInfo.Percent.Symbol;
			numberFormatInfo.PercentGroupSeparator = numericFormatInfo.Percent.GroupSeparator.ToString();
			numberFormatInfo.NegativeSign = numericFormatInfo.Number.NegativeSign;
			numberFormatInfo.PositiveSign = numericFormatInfo.Number.PositiveSign;
		}
	}

	public interface INumericFormat
	{
		int DecimalDigits { get; set; }
		char DecimalSeparator { get; set; }
		char GroupSeparator { get; set; }
		int NegativePattern { get; set; }
		string FormatString { get; }
	}

	[Serializable]
	public abstract class NumericFormatBase : INumericFormat
	{
		#region INumericFormat Members
		public int DecimalDigits { get; set; }
		public char DecimalSeparator { get; set; }
		public char GroupSeparator { get; set; }
		public int NegativePattern { get; set; }
		public abstract string FormatString { get; }
		#endregion
	}

	[Serializable]
	public sealed class NumberFormat : NumericFormatBase
	{
		public static readonly int DefaultDecimalDigits = 2;
		public static readonly char DefaultDecimalSeparator = '.';
		public static readonly char DefaultGroupSeparator = ',';
		public static readonly int DefaultNegativePattern = NumberNegativePattern.Value1.ToInt();
		public static readonly string DefaultNegativeSign = "-";
		public static readonly string DefaultPositiveSign = "+";

		public override string FormatString
		{
			get { return "N"; }
		}

		public string NegativeSign { get; set; }
		public string PositiveSign { get; set; }
	}

	[Serializable]
	public sealed class PercentFormat : NumericFormatBase
	{
		public static readonly string DefaultSymbol = "";
		public static readonly int DefaultDecimalDigits = 2;
		public static readonly char DefaultDecimalSeparator = '.';
		public static readonly int DefaultNegativePattern = PercentNegativePattern.Value1.ToInt();
		public static readonly int DefaultPositivePattern = PercentPositivePattern.Value1.ToInt();
		public static readonly char DefaultGroupSeparator = ',';

		public string Symbol { get; set; }
		public int PositivePattern { get; set; }

		public override string FormatString
		{
			get { return "P"; }
		}
	}

	[Serializable]
	public sealed class CurrencyFormat : NumericFormatBase
	{
		public static readonly string DefaultSymbol = "Rls";
		public static readonly int DefaultDecimalDigits = 2;
		public static readonly char DefaultDecimalSeparator = '.';
		public static readonly int DefaultNegativePattern = CurrencyNegativePattern.Value5.ToInt();
		public static readonly int DefaultPositivePattern = CurrencyPositivePattern.Value1.ToInt();
		public static readonly char DefaultGroupSeparator = ',';

		public string Symbol { get; set; }
		public int PositivePattern { get; set; }

		public override string FormatString
		{
			get { return "C"; }
		}
	}

	[Serializable]
	public sealed class CostFormat : NumericFormatBase
	{
		public static readonly string DefaultSymbol = "Rls";
		public static readonly int DefaultDecimalDigits = 8;
		public static readonly char DefaultDecimalSeparator = '.';
		public static readonly int DefaultNegativePattern = CurrencyNegativePattern.Value5.ToInt();
		public static readonly int DefaultPositivePattern = CurrencyPositivePattern.Value3.ToInt();
		public static readonly char DefaultGroupSeparator = ',';

		public string Symbol { get; set; }
		public int PositivePattern { get; set; }

		public override string FormatString
		{
			get { return "Co"; }
		}
	}

	public enum PercentNegativePattern
	{
		/// <summary>
		///     -n %
		/// </summary>
		[Description("-n %")]
		Value0 = 0,
		/// <summary>
		///     -n%
		/// </summary>
		[Description("-n%")]
		Value1 = 1,
	}

	public enum CurrencyNegativePattern
	{
		/// <summary>
		///     $n
		/// </summary>
		[Description("($n)")]
		Value0 = 0,
		/// <summary>
		///     -$n
		/// </summary>
		[Description("-$n")]
		Value1 = 1,
		/// <summary>
		///     $-n
		/// </summary>
		[Description("$-n")]
		Value2 = 2,
		/// <summary>
		///     $n-
		/// </summary>
		[Description("$n-")]
		Value3 = 3,
		/// <summary>
		///     (n$)
		/// </summary>
		[Description("(n$)")]
		Value4 = 4,
		/// <summary>
		///     -n$
		/// </summary>
		[Description("-n$")]
		Value5 = 5,
		/// <summary>
		///     n-$
		/// </summary>
		[Description("n-$")]
		Value6 = 6,
		/// <summary>
		///     n$-
		/// </summary>
		[Description("n$-")]
		Value7 = 7,
		/// <summary>
		///     -n $
		/// </summary>
		[Description("-n $")]
		Value8 = 8,
		/// <summary>
		///     -$ n
		/// </summary>
		[Description("-$ n")]
		Value9 = 9,
		/// <summary>
		///     n $-
		/// </summary>
		[Description("n $-")]
		Value10 = 10,
		/// <summary>
		///     $ n-
		/// </summary>
		[Description("$ n-")]
		Value11 = 11,
		/// <summary>
		///     $ -n
		/// </summary>
		[Description("$ -n")]
		Value12 = 12,
		/// <summary>
		///     n- $
		/// </summary>
		[Description("n- $")]
		Value13 = 13,
		/// <summary>
		///     ($ n)
		/// </summary>
		[Description("($ n)")]
		Value14 = 14,
		/// <summary>
		///     (n $)
		/// </summary>
		[Description("(n $)")]
		Value15 = 15,
	}

	public enum CurrencyPositivePattern
	{
		/// <summary>
		///     $n
		/// </summary>
		[Description("$n")]
		Value0 = 0,
		/// <summary>
		///     n$
		/// </summary>
		[Description("n$")]
		Value1 = 1,
		/// <summary>
		///     $ n
		/// </summary>
		[Description("$ n")]
		Value2 = 2,
		/// <summary>
		///     n $
		/// </summary>
		[Description("n $")]
		Value3 = 3,
	}

	public enum NumberNegativePattern
	{
		/// <summary>
		///     (n)
		/// </summary>
		[Description("(n)")]
		Value0 = 0,
		/// <summary>
		///     -n
		/// </summary>
		[Description("-n")]
		Value1 = 1,
		/// <summary>
		///     - n
		/// </summary>
		[Description("- n")]
		Value2 = 2,
		/// <summary>
		///     n-
		/// </summary>
		[Description("n-")]
		Value3 = 3,
		/// <summary>
		///     n -
		/// </summary>
		[Description("n -")]
		Value4 = 4,
	}

	public enum PercentPositivePattern
	{
		/// <summary>
		///     n %
		/// </summary>
		[Description("n %")]
		Value0 = 0,
		/// <summary>
		///     n%
		/// </summary>
		[Description("n%")]
		Value1 = 1,
		/// <summary>
		///     %n
		/// </summary>
		[Description("%n")]
		Value2 = 2,
		/// <summary>
		///     % n
		/// </summary>
		[Description("% n")]
		Value3 = 3,
	}
}
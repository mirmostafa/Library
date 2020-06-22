using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Mohammad.Globalization.Attributes;
using Mohammad.Helpers;

namespace Mohammad.Globalization.DataTypes
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
        [LocalizedDescription("fa-IR", "›——ÊœÌ‰")]
        [LocalizedDescription("en-US", "Farvardin")]
        Farvardin,

        [LocalizedDescription("fa-IR", "«—œÌ»Â‘ ")]
        [LocalizedDescription("en-US", "Ordibehesht")]
        Ordibehesht,

        [LocalizedDescription("fa-IR", "Œ—œ«œ")]
        [LocalizedDescription("en-US", "Khordad")]
        Khordad,

        [LocalizedDescription("fa-IR", " Ì—")]
        [LocalizedDescription("en-US", "Tir")]
        Tir,

        [LocalizedDescription("fa-IR", "„—œ«œ")]
        [LocalizedDescription("en-US", "Mordad")]
        Mordad,

        [LocalizedDescription("fa-IR", "‘Â—ÌÊ—")]
        [LocalizedDescription("en-US", "Shahrivar")]
        Sharivar,

        [LocalizedDescription("fa-IR", "„Â—")]
        [LocalizedDescription("en-US", "Mehr")]
        Mehr,

        [LocalizedDescription("fa-IR", "¬»«‰")]
        [LocalizedDescription("en-US", "Aban")]
        Aban,

        [LocalizedDescription("fa-IR", "¬–—")]
        [LocalizedDescription("en-US", "Azar")]
        Azar,

        [LocalizedDescription("fa-IR", "œÌ")]
        [LocalizedDescription("en-US", "Dey")]
        Dey,

        [LocalizedDescription("fa-IR", "»Â„‰")]
        [LocalizedDescription("en-US", "Bahman")]
        Bahman,

        [LocalizedDescription("fa-IR", "«”›‰œ")]
        [LocalizedDescription("en-US", "Esfand")]
        Esfand
    }

    public enum PersianDayOfWeek
    {
        [LocalizedDescription("fa-IR", "Ìò‘‰»Â")]
        [LocalizedDescription("en-US", "YekShanbeh")]
        YekShanbeh,

        [LocalizedDescription("fa-IR", "œÊ‘‰»Â")]
        [LocalizedDescription("en-US", "DoShanbeh")]
        DoShanbeh,

        [LocalizedDescription("fa-IR", "”Â˛‘‰»Â")]
        [LocalizedDescription("en-US", "SehShanbeh")]
        SeShanbeh,

        [LocalizedDescription("fa-IR", "çÂ«—‘‰»Â")]
        [LocalizedDescription("en-US", "ChaharShanbeh")]
        ChaharShanbeh,

        [LocalizedDescription("fa-IR", "Å‰Ã‘‰»Â")]
        [LocalizedDescription("en-US", "PanjShanbeh")]
        PanjShanbeh,

        [LocalizedDescription("fa-IR", "Ã„⁄Â")]
        [LocalizedDescription("en-US", "Jomeh")]
        Jomeh,

        [LocalizedDescription("fa-IR", "‘‰»Â")]
        [LocalizedDescription("en-US", "Shanbeh")]
        Shanbeh
    }

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
        public NumberFormat Number { get; }
        public PercentFormat Percent { get; }
        public CurrencyFormat Currency { get; }
        public CostFormat Cost { get; set; }

        public NumericFormatInfo(NumberFormat number, PercentFormat percent, CurrencyFormat currency, CostFormat cost)
        {
            this.Number = number;
            this.Percent = percent;
            this.Currency = currency;
            this.Cost = cost;
        }

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

        public void MapToNumberFormatInfo(NumberFormatInfo numberFormatInfo) { MapToNumberFormatInfo(numberFormatInfo, this); }

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
        public int DecimalDigits { get; set; }
        public char DecimalSeparator { get; set; }
        public char GroupSeparator { get; set; }
        public int NegativePattern { get; set; }
        public abstract string FormatString { get; }
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
        public override string FormatString { get { return "N"; } }
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
        public override string FormatString { get { return "P"; } }
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
        public override string FormatString { get { return "C"; } }
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
        public override string FormatString { get { return "Co"; } }
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
        Value1 = 1
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
        Value15 = 15
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
        Value3 = 3
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
        Value4 = 4
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
        Value3 = 3
    }
}
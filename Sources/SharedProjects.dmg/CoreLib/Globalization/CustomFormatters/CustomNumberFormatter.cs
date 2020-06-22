using System;
using System.Globalization;
using System.Threading;
using Mohammad.Globalization.DataTypes;

namespace Mohammad.Globalization.CustomFormatters
{
    [Serializable]
    public class CustomNumberFormatter : IFormatProvider, ICustomFormatter
    {
        public NumberFormatInfo Engine { get; }
        public NumericFormatInfo Info { get; }
        // Fields
        // Methods
        public CustomNumberFormatter(NumericFormatInfo numericFormatInfo)
            : this(Thread.CurrentThread.CurrentCulture.NumberFormat.Clone() as NumberFormatInfo, numericFormatInfo) { }

        public CustomNumberFormatter(NumberFormatInfo numberFormatInfo, NumericFormatInfo numericFormatInfo)
        {
            this.Engine = numberFormatInfo;
            this.Info = numericFormatInfo;
            this.Info.MapToNumberFormatInfo(this.Engine);
        }

        public double ParseCost(string formattedNumber)
        {
            var temp = this.Engine.Clone() as NumberFormatInfo;
            if (temp == null)
                return 0.0;
            temp.CurrencyDecimalDigits = this.Info.Cost.DecimalDigits;
            temp.CurrencyDecimalSeparator = this.Info.Cost.DecimalSeparator.ToString();
            temp.CurrencyGroupSeparator = this.Info.Cost.GroupSeparator.ToString();
            temp.CurrencyNegativePattern = this.Info.Cost.NegativePattern;
            temp.CurrencyPositivePattern = this.Info.Cost.PositivePattern;
            temp.CurrencySymbol = this.Info.Cost.Symbol;
            return double.Parse(formattedNumber, NumberStyles.Any, temp);
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var buffer = format.ToUpper();
            if (buffer != null && buffer == "CO")
            {
                var temp = this.Engine.Clone() as NumberFormatInfo;
                if (temp == null)
                    return null;
                temp.CurrencyDecimalDigits = this.Info.Cost.DecimalDigits;
                temp.CurrencyDecimalSeparator = this.Info.Cost.DecimalSeparator.ToString();
                temp.CurrencyGroupSeparator = this.Info.Cost.GroupSeparator.ToString();
                temp.CurrencyNegativePattern = this.Info.Cost.NegativePattern;
                temp.CurrencyPositivePattern = this.Info.Cost.PositivePattern;
                temp.CurrencySymbol = this.Info.Cost.Symbol;
                return string.Format(temp, "{0:C}", arg);
            }
            return string.Format(this.Engine, "{0:" + format + "}", arg);
        }

        public object GetFormat(Type formatType) { return formatType == typeof(ICustomFormatter) ? this : null; }
    }
}
#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Globalization;
using System.Threading;
using Library35.Globalization.DataTypes;

namespace Library35.Globalization.CustomFormatters
{
	[Serializable]
	public class CustomNumberFormatter : IFormatProvider, ICustomFormatter
	{
		// Fields
		private readonly NumberFormatInfo _Engine;

		// Methods
		public CustomNumberFormatter(NumericFormatInfo numericFormatInfo)
			: this(Thread.CurrentThread.CurrentCulture.NumberFormat.Clone() as NumberFormatInfo, numericFormatInfo)
		{
		}

		public CustomNumberFormatter(NumberFormatInfo numberFormatInfo, NumericFormatInfo numericFormatInfo)
		{
			this._Engine = numberFormatInfo;
			this.Info = numericFormatInfo;
			this.Info.MapToNumberFormatInfo(this.Engine);
		}

		public NumberFormatInfo Engine
		{
			get { return this._Engine; }
		}

		public NumericFormatInfo Info { get; private set; }

		#region ICustomFormatter Members
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			var buffer = format.ToUpper();
			if ((buffer != null) && (buffer == "CO"))
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
				return string.Format(temp,
					"{0:C}",
					new[]
					{
						arg
					});
			}
			return string.Format(this.Engine,
				"{0:" + format + "}",
				new[]
				{
					arg
				});
		}
		#endregion

		#region IFormatProvider Members
		public object GetFormat(Type formatType)
		{
			return ((formatType == typeof (ICustomFormatter)) ? this : null);
		}
		#endregion

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

		// Properties
	}
}
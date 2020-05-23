#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Web;
using Library35.Globalization.CustomFormatters;
using Library35.Globalization.DataTypes;

namespace Library35.Web.Globalization
{
	public static partial class FormatHelper
	{
		private static string _NumberFormatterKeyName = "CustomNumberFormatter";

		public static string NumberFormatterKeyName
		{
			get { return _NumberFormatterKeyName; }
			set { _NumberFormatterKeyName = value; }
		}
		public static CustomNumberFormatter Formatter
		{
			get { return HttpContext.Current.Application[NumberFormatterKeyName] as CustomNumberFormatter; }
			private set { HttpContext.Current.Application[NumberFormatterKeyName] = value; }
		}

		public static void Initialize(NumericFormatInfo numericFormat)
		{
			Formatter = new CustomNumberFormatter(numericFormat);
		}
	}
}
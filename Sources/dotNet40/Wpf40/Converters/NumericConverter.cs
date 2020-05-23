#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Globalization;
using System.Windows.Data;

namespace Library40.Wpf.Converters
{
	public class NumericConverter : IValueConverter
	{
		#region IValueConverter Members
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return 0;
			object result;
			if (Validate<int>(value, targetType, Int32.TryParse, out result))
				return result;
			if (Validate<long>(value, targetType, Int64.TryParse, out result))
				return result;
			if (Validate<short>(value, targetType, Int16.TryParse, out result))
				return result;
			if (Validate<double>(value, targetType, Double.TryParse, out result))
				return result;
			if (Validate<float>(value, targetType, Single.TryParse, out result))
				return result;
			return 0;
		}
		#endregion

		private static bool Validate<TNumericType>(object value, Type targetType, TryParse<TNumericType> tryParse, out object result) where TNumericType : struct
		{
			result = default(TNumericType);
			var res = default(TNumericType);
			if ((targetType == typeof (TNumericType)) || (targetType == typeof (TNumericType?)) && tryParse(value.ToString(), out res))
			{
				result = res;
				return true;
			}
			return false;
		}

		private delegate bool TryParse<TNumericType>(string s, out TNumericType result);
	}
}
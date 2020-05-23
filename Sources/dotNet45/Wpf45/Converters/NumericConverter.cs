using System;
using System.Globalization;
using System.Windows.Data;

namespace Mohammad.Wpf.Converters
{
    public class NumericConverter : IValueConverter
    {
        private static bool Validate<TNumericType>(object value, Type targetType, TryParse<TNumericType> tryParse, out object result) where TNumericType : struct
        {
            result = default(TNumericType);
            var res = default(TNumericType);
            if (targetType == typeof(TNumericType) || targetType == typeof(TNumericType?) && tryParse(value.ToString(), out res))
            {
                result = res;
                return true;
            }
            return false;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return value; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;
            object result;
            if (Validate<int>(value, targetType, int.TryParse, out result))
                return result;
            if (Validate<long>(value, targetType, long.TryParse, out result))
                return result;
            if (Validate<short>(value, targetType, short.TryParse, out result))
                return result;
            if (Validate<double>(value, targetType, double.TryParse, out result))
                return result;
            return Validate<float>(value, targetType, float.TryParse, out result) ? result : 0;
        }

        private delegate bool TryParse<TNumericType>(string s, out TNumericType result);
    }
}
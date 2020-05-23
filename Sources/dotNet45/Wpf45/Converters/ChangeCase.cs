using System;
using System.Globalization;
using System.Windows.Data;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var strValue = value.ToString();

                return strValue.ToUpperInvariant();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }

    [ValueConversion(typeof(string), typeof(string))]
    public class ToLowerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var strValue = value.ToString();

                return strValue.ToLowerInvariant();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }

    [ValueConversion(typeof(string), typeof(string))]
    public class SeparateCamelCase : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var strValue = value.ToString();

                return strValue.SeparateCamelCase();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}
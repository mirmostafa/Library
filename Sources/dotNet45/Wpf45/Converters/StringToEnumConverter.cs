using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Converters
{
    [ValueConversion(typeof(string), typeof(Enum))]
    public class StringToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetNames(targetType).FirstOrDefault(e => e == ObjectHelper.ToString(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return ObjectHelper.ToString(value); }
    }
}
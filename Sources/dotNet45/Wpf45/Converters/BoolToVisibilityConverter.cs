using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool) ? (object) null : (value.To<bool>() ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                return null;
            var visibility = value.To<Visibility>();
            switch (visibility)
            {
                case Visibility.Visible:
                    return true;
                case Visibility.Hidden:
                    return null;
                case Visibility.Collapsed:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class ValueEqualsParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return Equals(value, parameter); }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return Equals(value, parameter); }
    }

    public class ValueNotEqualsParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return !Equals(value, parameter); }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return !Equals(value, parameter); }
    }
}
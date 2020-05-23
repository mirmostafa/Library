using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Converters
{
    public class TextToBrushRequiredFieldValidator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StringHelper.IsNullOrEmpty(value) ? Brushes.Red : Brushes.RoyalBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }

    public class KeyEqualsValueValidator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return value == parameter; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}
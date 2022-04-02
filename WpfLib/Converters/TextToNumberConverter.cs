using System.Globalization;
using System.Windows.Data;

namespace Library.Wpf.Converters;

public sealed class TextToNumberConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value switch
        {
            null => null!,
            int or decimal or float => value!.ToString()!,
            _ => System.Convert.ToString(value, culture)!
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return null!;
        }

        if (value is string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null!;
            }
        }
        return System.Convert.ToDecimal(value, culture);
    }
}

using System.Globalization;
using System.Windows.Data;

namespace Library.Wpf.Converters;

public sealed class ObjectNullToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //! parameter says that value has the direct result, or must be reversed.
        => parameter switch
        {
            false or "false" => value switch
            {
                bool v => !v,
                not null => false,
                _ => !default(bool)
            },
            _ => value switch
            {
                bool v => v,
                not null => true,
                _ => default
            },
        };

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b ? b ? parameter : null : null;
}

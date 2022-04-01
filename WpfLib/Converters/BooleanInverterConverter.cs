using System.Globalization;
using System.Windows.Data;

namespace Library.Wpf.Converters;
public class BooleanInverterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return parameter switch
        {
            false or "false" => !invert(true),
            _ => invert(value),
        };
        static bool invert(object value) => value switch
        {
            false or "false" => true,
            true or "true" => false,
            null => true,
            _ => throw new NotImplementedException(),
        };
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
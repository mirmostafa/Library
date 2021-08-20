﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfLibrary.Converters;
public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => parameter switch
        {
            false or "false" => value switch
            {
                bool b => !b ? Visibility.Visible : Visibility.Collapsed,
                Visibility v => v == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible,
                _ => Visibility.Collapsed
            },
            _ => value switch
            {
                bool b => b ? Visibility.Visible : Visibility.Collapsed,
                Visibility v => v,
                _ => Visibility.Visible
            },
        };
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => (value ?? Visibility.Visible) is Visibility visibility ? visibility == Visibility.Visible : throw new NotSupportedException();
}

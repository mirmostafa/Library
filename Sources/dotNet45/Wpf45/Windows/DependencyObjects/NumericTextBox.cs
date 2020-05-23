using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Windows.DependencyObjects
{
    public class NumericTextBox : DependencyObject
    {
        private static readonly Dictionary<TextBox, Props> _Repository = new Dictionary<TextBox, Props>();

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.RegisterAttached("MinValue",
            typeof(int?),
            typeof(NumericTextBox),
            new PropertyMetadata(null,
                (sender, e) =>
                {
                    var textBox = (TextBox) sender;
                    if (textBox == null)
                        return;
                    SetProps(textBox, (int?) e.NewValue, int.MaxValue, null);
                }));

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.RegisterAttached("MaxValue",
            typeof(int?),
            typeof(NumericTextBox),
            new PropertyMetadata(null,
                (sender, e) =>
                {
                    var textBox = (TextBox) sender;
                    if (textBox == null)
                        return;
                    SetProps(textBox, int.MinValue, (int?) e.NewValue, null);
                }));

        public static readonly DependencyProperty IsNullableProperty = DependencyProperty.RegisterAttached("IsNullable",
            typeof(bool),
            typeof(NumericTextBox),
            new PropertyMetadata(default(bool),
                (sender, e) =>
                {
                    var textBox = (TextBox) sender;
                    if (textBox == null)
                        return;
                    SetProps(textBox, int.MinValue, int.MaxValue, (bool) e.NewValue);
                }));

        public static bool GetIsNullable(DependencyObject element) { return (bool) element.GetValue(IsNullableProperty); }
        public static void SetIsNullable(DependencyObject element, bool value) { element.SetValue(IsNullableProperty, value); }

        public static int? GetMaxValue(DependencyObject obj) { return (int?) obj.GetValue(MaxValueProperty); }
        public static void SetMaxValue(DependencyObject obj, int? value) { obj.SetValue(MaxValueProperty, value); }

        public static int? GetMinValue(DependencyObject element) { return (int?) element.GetValue(MinValueProperty); }
        public static void SetMinValue(DependencyObject element, int? value) { element.SetValue(MinValueProperty, value); }

        private static void SetProps(TextBox textBox, int? minValue, int? maxValue, bool? isNullable)
        {
            textBox.PreviewTextInput -= TargetTextbox_OnPreviewTextInput;
            textBox.PreviewKeyDown -= TargetTextbox_OnPreviewKeyDown;
            textBox.LostFocus -= TextBox_OnLostFocus;
            textBox.PreviewTextInput += TargetTextbox_OnPreviewTextInput;
            textBox.PreviewKeyDown += TargetTextbox_OnPreviewKeyDown;
            textBox.LostFocus += TextBox_OnLostFocus;

            Props props;
            if (_Repository.ContainsKey(textBox))
            {
                props = _Repository[textBox];
            }
            else
            {
                props = new Props();
                _Repository.Add(textBox, props);
            }
            if (minValue != int.MinValue)
                props.Min = minValue;
            if (maxValue != int.MaxValue)
                props.Max = maxValue;
            if (isNullable != null)
                props.IsNullable = isNullable;
        }

        private static void TargetTextbox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (!_Repository.ContainsKey(textBox))
                return;

            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.V)
                e.Handled = true;
        }

        private static void TargetTextbox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (!_Repository.ContainsKey(textBox))
                return;

            var newChar = e.Text[0];
            if (!char.IsNumber(newChar))
            {
                e.Handled = true;
                return;
            }
            var num = int.Parse(textBox.Text + newChar);
            if (num <= _Repository[textBox].Max && num >= _Repository[textBox].Min)
                return;
            if (num > _Repository[textBox].Max)
            {
                textBox.Text = _Repository[textBox].Max.ToString();
                textBox.SelectionStart = textBox.Text.Length;
                e.Handled = true;
            }
            if (!(num < _Repository[textBox].Min))
                return;
            textBox.Text = _Repository[textBox].Min.ToString();
            textBox.SelectionStart = textBox.Text.Length;
            e.Handled = true;
        }

        private static void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;

            if (!_Repository.ContainsKey(textBox))
                return;
            int num;
            var props = _Repository[textBox];
            if ((props.IsNullable ?? false) && textBox.Text.IsNullOrEmpty())
                return;
            if (!int.TryParse(textBox.Text, out num))
            {
                textBox.Text = props.Min.ToString();
                textBox.SelectionStart = textBox.Text.Length;
                e.Handled = true;
            }
            else if (num > props.Max)
            {
                textBox.Text = props.Max.ToString();
                textBox.SelectionStart = textBox.Text.Length;
                e.Handled = true;
            }
            else if (num < props.Min)
            {
                textBox.Text = props.Min.ToString();
                textBox.SelectionStart = textBox.Text.Length;
                e.Handled = true;
            }
            if (textBox.Text.Length == 1)
                textBox.Text = "0" + textBox.Text;
        }

        private class Props
        {
            public int? Min { get; set; }
            public int? Max { get; set; }
            public bool? IsNullable { get; set; }
        }
    }
}
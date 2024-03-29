﻿using System.Globalization;
using System.Windows.Input;

namespace Library.Wpf.Behaviors;

/// <summary>
///     WPF Maskable TextBox class. Just specify the TextBoxMaskBehavior.Mask attached property to a TextBox.
///     It protect your TextBox from unwanted non numeric symbols and make it easy to modify your numbers.
/// </summary>
public sealed class TextBoxMaskBehavior
{
    public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.RegisterAttached("MinimumValue",
        typeof(double),
        typeof(TextBoxMaskBehavior),
        new FrameworkPropertyMetadata(double.NaN, MinimumValueChangedCallback));

    public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.RegisterAttached("MaximumValue",
        typeof(double),
        typeof(TextBoxMaskBehavior),
        new FrameworkPropertyMetadata(double.NaN, MaximumValueChangedCallback));

    public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached("Mask",
        typeof(MaskType),
        typeof(TextBoxMaskBehavior),
        new FrameworkPropertyMetadata(MaskChangedCallback));

    public static MaskType GetMask(DependencyObject obj) => (MaskType)obj.GetValue(MaskProperty);

    public static void SetMask(DependencyObject obj, MaskType value) => obj.SetValue(MaskProperty, value);

    public static double GetMaximumValue(DependencyObject obj) => (double)obj.GetValue(MaximumValueProperty);

    public static void SetMaximumValue(DependencyObject obj, double value) => obj.SetValue(MaximumValueProperty, value);

    public static double GetMinimumValue(DependencyObject obj) => (double)obj.GetValue(MinimumValueProperty);

    public static void SetMinimumValue(DependencyObject obj, double value) => obj.SetValue(MinimumValueProperty, value);

    private static void MaskChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is TextBox _thisOld)
        {
            _thisOld.PreviewTextInput -= TextBox_OnPreviewTextInput;
            DataObject.RemovePastingHandler(e.OldValue as TextBox, TextBoxPastingEventHandler);
        }

        if (d is not TextBox _this)
        {
            return;
        }

        if ((MaskType)e.NewValue != MaskType.Any)
        {
            _this.PreviewTextInput += TextBox_OnPreviewTextInput;
            DataObject.AddPastingHandler(_this, TextBoxPastingEventHandler);
        }

        ValidateTextBox(_this);
    }

    private static void MaximumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var _this = d as TextBox;
        ValidateTextBox(_this);
    }

    private static void MinimumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var _this = d as TextBox;
        ValidateTextBox(_this);
    }

    private static void ValidateTextBox(TextBox _this)
    {
        if (GetMask(_this) != MaskType.Any)
        {
            _this.Text = ValidateValue(GetMask(_this), _this.Text);
        }
    }

    private static void TextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
    {
        var _this = sender as TextBox;
        var clipboard = e.DataObject.GetData(typeof(string)) as string;
        clipboard = ValidateValue(GetMask(_this), clipboard);
        if (!string.IsNullOrEmpty(clipboard))
        {
            _this.Text = clipboard;
        }

        e.CancelCommand();
        e.Handled = true;
    }

    private static void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var _this = sender as TextBox;
        var isValid = IsSymbolValid(GetMask(_this), e.Text);
        e.Handled = !isValid;
        if (isValid)
        {
            var caret = _this.CaretIndex;
            var text = _this.Text;
            var textInserted = false;
            var selectionLength = 0;

            if (_this.SelectionLength > 0)
            {
                text = text[.._this.SelectionStart] + text[(_this.SelectionStart + _this.SelectionLength)..];
                caret = _this.SelectionStart;
            }

            if (e.Text == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
            {
                while (true)
                {
                    var ind = text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                    if (ind == -1)
                    {
                        break;
                    }

                    text = text[..ind] + text[(ind + 1)..];
                    if (caret > ind)
                    {
                        caret--;
                    }
                }

                if (caret == 0)
                {
                    text = "0" + text;
                    caret++;
                }
                else if (caret == 1 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    text = NumberFormatInfo.CurrentInfo.NegativeSign + "0" + text[1..];
                    caret++;
                }

                if (caret == text.Length)
                {
                    selectionLength = 1;
                    textInserted = true;
                    text = text + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "0";
                    caret++;
                }
            }
            else if (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign)
            {
                textInserted = true;
                if (_this.Text.Contains(NumberFormatInfo.CurrentInfo.NegativeSign))
                {
                    text = text.Replace(NumberFormatInfo.CurrentInfo.NegativeSign, string.Empty);
                    if (caret != 0)
                    {
                        caret--;
                    }
                }
                else
                {
                    text = NumberFormatInfo.CurrentInfo.NegativeSign + _this.Text;
                    caret++;
                }
            }

            if (!textInserted)
            {
                text = text[..caret] + e.Text + (caret < _this.Text.Length ? text[caret..] : string.Empty);
                caret++;
            }

            try
            {
                var val = Convert.ToDouble(text);
                var newVal = ValidateLimits(GetMinimumValue(_this), GetMaximumValue(_this), val);
                if (val != newVal)
                {
                    text = newVal.ToString();
                }
                else if (val == 0)
                {
                    if (!text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                    {
                        text = "0";
                    }
                }
            }
            catch
            {
                text = "0";
            }

            while (text.Length > 1 && text[0] == '0' && string.Empty + text[1] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
            {
                text = text[1..];
                if (caret > 0)
                {
                    caret--;
                }
            }

            while (text.Length > 2 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign && text[1] == '0' &&
                   string.Empty + text[2] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
            {
                text = NumberFormatInfo.CurrentInfo.NegativeSign + text[2..];
                if (caret > 1)
                {
                    caret--;
                }
            }

            if (caret > text.Length)
            {
                caret = text.Length;
            }

            _this.Text = text;
            _this.CaretIndex = caret;
            _this.SelectionStart = caret;
            _this.SelectionLength = selectionLength;
            e.Handled = true;
        }
    }

    private static string ValidateValue(MaskType mask, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        value = value.Trim();
        switch (mask)
        {
            case MaskType.Integer:
                try
                {
                    _ = Convert.ToInt64(value);
                    return value;
                }
                catch
                {
                }

                return string.Empty;

            case MaskType.Decimal:
                try
                {
                    _ = Convert.ToDouble(value);

                    return value;
                }
                catch
                {
                }

                return string.Empty;
            case MaskType.Any:
                break;
        }

        return value;
    }

    private static double ValidateLimits(double min, double max, double value)
    {
        if (!min.Equals(double.NaN))
        {
            if (value < min)
            {
                return min;
            }
        }

        if (!max.Equals(double.NaN))
        {
            if (value > max)
            {
                return max;
            }
        }

        return value;
    }

    private static bool IsSymbolValid(MaskType mask, string str)
    {
        switch (mask)
        {
            case MaskType.Any:
                return true;

            case MaskType.Integer:
                if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    return true;
                }

                break;

            case MaskType.Decimal:
                if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator || str == NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    return true;
                }

                break;
                //case MaskType.Time:
                //	if (str == ":")
                //		return true;
                //	break;
        }

        if (mask.Equals(MaskType.Integer) || mask.Equals(MaskType.Decimal))
        {
            return str.All(char.IsDigit);
        }

        return false;
    }
}

public enum MaskType
{
    Any,
    Integer,

    Decimal
    //Time,
}

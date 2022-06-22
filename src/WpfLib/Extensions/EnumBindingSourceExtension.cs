#nullable disable
using System.ComponentModel;
using System.Windows.Markup;

namespace Library.Wpf.Extensions;

public class EnumBindingSourceExtension : MarkupExtension
{
    private Type _EnumType;
    public Type EnumType
    {
        get => this._EnumType;
        set
        {
            if (value != this._EnumType)
            {
                if (null != value)
                {
                    var enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum)
                    {
                        throw new ArgumentException("Type must be for an Enum.");
                    }
                }

                this._EnumType = value;
            }
        }
    }

    public EnumBindingSourceExtension() { }

    public EnumBindingSourceExtension(Type enumType) => this.EnumType = enumType;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (null == this._EnumType)
        {
            throw new InvalidOperationException("The EnumType must be specified.");
        }

        var actualEnumType = Nullable.GetUnderlyingType(this._EnumType) ?? this._EnumType;
        var enumValues = Enum.GetValues(actualEnumType);

        if (actualEnumType == this._EnumType)
        {
            return enumValues;
        }

        var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
        enumValues.CopyTo(tempArray, 1);
        return tempArray;
    }
}

public class EnumDescriptionTypeConverter : EnumConverter
{
    public EnumDescriptionTypeConverter(Type type)
        : base(type)
    {
    }
    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            if (value != null)
            {
                var fi = value.GetType().GetField(value.ToString());
                if (fi != null)
                {
                    var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Description))) ? attributes[0].Description : value.ToString();
                }
            }

            return string.Empty;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

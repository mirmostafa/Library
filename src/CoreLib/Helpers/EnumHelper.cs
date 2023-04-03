using System.Collections;
using System.ComponentModel;

using Library.Globalization.Attributes;
using Library.Helpers;
using Library.Validations;

namespace Library.Helpers;

/// <summary>
///     A utility to do some common tasks about enumerations
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// </summary>
    /// <param name="enumeration"> </param>
    /// <param name="item"> </param>
    /// <typeparam name="TEnum"> </typeparam>
    /// <returns> </returns>
    public static TEnum AddFlag<TEnum>(this TEnum enumeration, in TEnum item)
        where TEnum : Enum => (TEnum)Enum.ToObject(typeof(TEnum), enumeration.Cast().ToInt() | item.Cast().ToInt());

    public static bool Contains<TEnum>(this Enum enumeration, TEnum item)
        where TEnum : Enum => item.Cast().ToInt() is 0
        ? enumeration.Cast().ToInt() == 0
        : (enumeration.Cast().ToInt() | item.Cast().ToInt()) == enumeration.Cast().ToInt();

    /// <summary>
    /// </summary>
    /// <param name="enumValue"> </param>
    /// <typeparam name="TSourceEnum"> </typeparam>
    /// <typeparam name="TDestinationEnum"> </typeparam>
    /// <returns> </returns>
    public static TDestinationEnum Convert<TSourceEnum, TDestinationEnum>(TSourceEnum enumValue)
        where TDestinationEnum : Enum => (TDestinationEnum)Enum.Parse(typeof(TDestinationEnum), enumValue?.ToString() ?? string.Empty);

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <typeparam name="TEnum"> </typeparam>
    /// <returns> </returns>
    public static TEnum Convert<TEnum>(object value)
        where TEnum : Enum => Convert<object, TEnum>(value);

    public static IEnumerable<string?> GetDescriptions<TEnum>(IEnumerable<TEnum> items)
        where TEnum : Enum => items.Select(item => GetItemDescription(item));

    public static IEnumerable<string?> GetDescriptions<TEnum>(IEnumerable<TEnum> items, string cultureName)
        where TEnum : Enum => items.Select(item => GetItemDescription(item, cultureName: cultureName));

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <typeparam name="TAttribute"> </typeparam>
    /// <returns> </returns>
    public static TAttribute? GetItemAttribute<TAttribute>(Enum? value)
        where TAttribute : Attribute => GetItemAttributes<TAttribute>(value)?.FirstOrDefault();

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <typeparam name="TEnum"> </typeparam>
    /// <typeparam name="TAttribute"> </typeparam>
    /// <returns> </returns>
    public static object? GetItemAttribute<TEnum, TAttribute>([DisallowNull] TEnum value)
        where TAttribute : Attribute
        where TEnum : Enum
    {
        var attributes = (TAttribute[]?)value.ArgumentNotNull(nameof(value))
                                             .GetType()
                                             .GetField(value.ToString())?.GetCustomAttributes(typeof(TAttribute), false);
        return attributes?.Length > 0 ? attributes[0] : null;
    }

    public static IEnumerable<TAttribute>? GetItemAttributes<TAttribute>(Enum? value)
        where TAttribute : Attribute
    {
        if (value is null)
        {
            return Enumerable.Empty<TAttribute>();
        }

        var attributes = (TAttribute[]?)value.GetType().GetField(value.ToString())?.GetCustomAttributes(typeof(TAttribute), false);
        return attributes?.Length > 0 ? attributes.AsEnumerable() : Enumerable.Empty<TAttribute>();
    }

    public static string? GetItemDescription(Enum? value,
        bool localized = true,
        string cultureName = "en-US")
    {
        Check.IfArgumentNotNull(value);
        Check.If(!string.IsNullOrEmpty(cultureName), () => new ArgumentException($"'{nameof(cultureName)}' cannot be null or empty", nameof(cultureName)));

        if (localized)
        {
            var descriptions = GetItemAttributes<LocalizedDescriptionAttribute>(value)?.ToArray();
            if (descriptions?.Any() is false)
            {
                return value.ToString().SeparateCamelCase() ?? string.Empty;
            }

            var description = descriptions?.FirstOrDefault(desc => cultureName.EqualsTo(desc.CultureName));
            return description is null ? value.ToString().SeparateCamelCase() : description.Description;
        }

        var descriptionAttribute = GetItemAttribute<DescriptionAttribute>(value);
        return descriptionAttribute is null
            ? value.ToString().SeparateCamelCase()
            : descriptionAttribute.Description;
    }

    public static IDictionary<TEnum, string>? GetValues<TEnum>()
        where TEnum : Enum
    {
        var type = typeof(TEnum);

        if (!type.IsEnum)
        {
            throw new ArgumentException("type must be an enumeration type.");
        }

        var pairs = from TEnum t in Enum.GetValues(type)
                    select new
                    {
                        Value = (TEnum)Enum.ToObject(type, t),
                        Text = Enum.GetName(type, t)
                    };

        return pairs?.ToDictionary(t => t.Value, t => t.Text);
    }

    public static bool IsIn(this Enum value, params Enum[] range)
        => range.Contains(value);

    /// <summary>
    /// </summary>
    /// <param name="value"> </param>
    /// <typeparam name="TEnum"> </typeparam>
    /// <returns> </returns>
    public static bool IsMemberOf<TEnum>(object value)
        where TEnum : Enum
        => int.TryParse(value.ToString(), out var iValue)
            ? Enum.IsDefined(typeof(TEnum), iValue)
            : Enum.IsDefined(typeof(TEnum), Parse(value));

    public static TEnum Merge<TEnum>()
        where TEnum : Enum
    {
        var result = Enum.GetValues(typeof(TEnum)).Cast<int>().Aggregate(0, (current, item) => current | item);
        return (TEnum)Enum.Parse(typeof(TEnum), Enum.GetName(typeof(TEnum), result)!);
    }

    public static IEnumerable<TEnum> ParseIf<TEnum>(IEnumerable source)
        where TEnum : Enum
    {
        if (source is null)
        {
            yield break;
        }

        foreach (var item in source)
        {
            if (item is not null && TryParse(item, out TEnum? result))
            {
                yield return result;
            }
        }
    }

    /// <summary>
    ///     Removes the flag.
    /// </summary>
    /// <typeparam name="TEnum"> The type of the enum. </typeparam>
    /// <param name="enumeration"> The enumeration. </param>
    /// <param name="item"> The item. </param>
    /// <returns> </returns>
    public static TEnum RemoveFlag<TEnum>(TEnum enumeration, TEnum item)
        where TEnum : Enum => (TEnum)Enum.ToObject(typeof(TEnum), enumeration.Cast().ToInt() & ~item.Cast().ToInt());

    /// <summary>
    ///     Converts to enum.
    /// </summary>
    /// <typeparam name="TEnum"> The type of the enum. </typeparam>
    /// <param name="value"> The value. </param>
    /// <returns> </returns>
    public static TEnum ToEnum<TEnum>(string value)
        where TEnum : Enum => Enum.Parse(typeof(TEnum), value).Cast().To<TEnum>();

    /// <summary>
    ///     Converts to enum.
    /// </summary>
    /// <typeparam name="TEnum"> The type of the enum. </typeparam>
    /// <param name="value"> The value. </param>
    /// <returns> </returns>
    public static TEnum ToEnum<TEnum>(int value)
        => Enum.Parse(typeof(TEnum), Enum.GetName(typeof(TEnum), value)!).Cast().To<TEnum>();

    /// <summary>
    ///     Tries the parse.
    /// </summary>
    /// <typeparam name="TEnum"> The type of the enum. </typeparam>
    /// <param name="value"> The value. </param>
    /// <param name="result"> The result. </param>
    /// <returns> </returns>
    public static bool TryParse<TEnum>(object value, [NotNullWhen(true)] out TEnum? result)
        where TEnum : Enum
    {
        result = default;
        if (!IsMemberOf<TEnum>(value))
        {
            return false;
        }

        result = Convert<TEnum>(Parse(value));
        return true;
    }

    private static object Parse(object value) => value is string
        ? value.ToString()!.Contains('.')
            ? value.ToString()![(value.ToString()!.LastIndexOf(".") + 1)..]
            : value
        : value;

    public static IEnumerable<TEnum> GetItems<TEnum>()
        where TEnum : Enum =>
        Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
}
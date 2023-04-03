using System.Collections;

using Library.Validations;

namespace Library.Helpers;

public static class Cast
{
    /// <summary>
    /// Works just like <code>as</code> C# keyword
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static T? As<T>(this object? obj)
        where T : class => obj as T;

    public static T? Match<T>(object obj) => obj is T result ? result : default;

    public static IEnumerable<T> OfType<T>(IEnumerable items)
        => items.OfType<T>();

    public static T To<T>(this object? obj)
        => (T)obj;

    public static T To<T>(this object obj, [DisallowNull] Func<object, T> convert)
        => convert.ArgumentNotNull().Invoke(obj);

    /// <summary>
    /// Converts the string representation of a number to an integer.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static int ToInt(this object obj)
        => Convert.ToInt32(obj);

    public static int ToInt(this object? obj, int defaultValue)
    {
        if (!int.TryParse(Convert.ToString(obj), out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Converts to long.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    //[Obsolete("No more is required.")]
    public static long ToLong(this object obj)
        => Convert.ToInt64(obj);

    public static string? ToString(in object? value, in string defaultValue = "")
        => (value ?? defaultValue)?.ToString();

    public static IEnumerable<T> TypeOf<T>(IEnumerable items)
    {
        foreach (var item in items)
        {
            if (TypeOf<T>(item) is { } result)
            {
                yield return result;
            }
        }
    }

    public static T? TypeOf<T>(object obj) => obj.GetType() == typeof(T) ? (T)obj : default;

    public static IEnumerable<T> WhereIs<T>(IEnumerable items)
    {
        foreach (var item in items)
        {
            if (item is T result)
            {
                yield return result;
            }
        }
    }
}
namespace Library.Helpers;

public static class ArrayHelper
{
    public static T[] Append<T>(this T[] array, T item)
    {
        if (IsNullOrEmpty(array))
        {
            return new T[] { item };
        }
        var result = new T[array.Length + 1];
        array.CopyTo(result, 0);
        result[array.Length] = item;
        return result;
    }

    private static bool IsNullOrEmpty<T>([NotNullWhen(true)] this T[] array)
        => array is null or { Length: 0 };
}
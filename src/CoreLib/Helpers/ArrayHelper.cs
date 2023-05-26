namespace Library.Helpers;

public static class ArrayHelper
{
    /// <summary>
    /// Appends an item to the end of an array.
    /// </summary>
    /// <param name="array">The array to append the item to.</param>
    /// <param name="item">The item to append to the array.</param>
    /// <returns>A new array with the item appended.</returns>
    public static T[] Append<T>(this T[] array, T item)
    //This code adds an item to the end of an array
    {
        //If the array is empty, return a new array with the item
        if (IsNullOrEmpty(array))
        {
            return new T[] { item };
        }
        //Create a new array with one more element than the original array
        var result = new T[array.Length + 1];
        //Copy the elements of the original array to the new array
        array.CopyTo(result, 0);
        //Add the item to the end of the new array
        result[array.Length] = item;
        //Return the new array
        return result;
    }

    /// <summary>
    /// Checks if the given array is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array to check.</param>
    /// <returns>
    /// True if the array is null or empty, false otherwise.
    /// </returns>
    private static bool IsNullOrEmpty<T>([NotNullWhen(true)] this T[] array)
            => array is null or { Length: 0 };
}
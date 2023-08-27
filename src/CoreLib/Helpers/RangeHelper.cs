using Library.Helpers.Models;

namespace Library.Helpers;

public static class RangeHelper
{
    /// <summary>
    /// Converts a Range into an enumerable sequence of integers.
    /// </summary>
    /// <param name="range">The Range to be converted.</param>
    /// <returns>An IEnumerable sequence of integers within the specified Range.</returns>
    public static IEnumerable<int> AsEnumerable(this Range range)
    {
        foreach (var item in range)
        {
            yield return item; // Yield each integer within the specified Range.
        }
    }

    /// <summary>
    /// Gets a custom enumerator for the given range.
    /// </summary>
    /// <param name="range">The range to get the enumerator for.</param>
    /// <returns>A custom enumerator for the given range.</returns>
    public static IEnumerator<int> GetEnumerator(this Range range)
        => new CustomIntEnumerator(range);
}
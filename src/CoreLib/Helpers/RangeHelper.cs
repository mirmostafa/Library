using Library.Helpers.Models;

namespace Library.Helpers;

public static class RangeHelper
{
    public static IEnumerable<int> AsEnumerable(this Range range)
    {
        foreach (var item in range)
        {
            yield return item;
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
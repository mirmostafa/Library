using Library.Helpers.Models;

namespace Library.Helpers;

public static class RangeHelper
{
    /// <summary>
    /// Gets a custom enumerator for the given range. 
    /// </summary>
    /// <param name="range">The range to get the enumerator for.</param>
    /// <returns>A custom enumerator for the given range.</returns>
    public static CustomIntEnumerator GetEnumerator(this Range range)
        => new(range);
}
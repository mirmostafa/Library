using Library.Helpers.Models;

namespace Library.Helpers;

public static class RangeHelper
{
    public static CustomIntEnumerator GetEnumerator(this Range range)
        => new(range);
}
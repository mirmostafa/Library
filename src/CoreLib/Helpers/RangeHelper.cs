using Library.Helpers.Models;

namespace Library.Helpers;

public static class RangeHelper
{
    public static CustomIntEnumerator GetEnumerator(this Range range)
        => new(range);
    //public static IEnumerator<int> GetEnumerator(this Range range) 
    //    => Enumerable.Range(range.Start.Value - 1, range.End.Value).GetEnumerator();
}
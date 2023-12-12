using System.Collections;

using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
public interface IFluentCollection<TSelf, TItem> : IEnumerable<TItem>, IEnumerable
    where TSelf : IFluentCollection<TSelf, TItem>
{
    int Count
    {
        get;
    }

    bool IsReadOnly
    {
        get;
    }

    TSelf Add(TItem item);

    TSelf Clear();

    (TSelf List, bool Result) Contains(TItem item);

    TSelf CopyTo(TItem[] array, int arrayIndex);

    (TSelf List, bool Result) Remove(TItem item);
}
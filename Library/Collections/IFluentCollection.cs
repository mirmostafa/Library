using System.Collections;
using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
public interface IFluentCollection<TList, TItem> : IEnumerable<TItem>, IEnumerable
    where TList : IFluentCollection<TList, TItem>
{
    int Count
    {
        get;
    }

    bool IsReadOnly
    {
        get;
    }

    TList Add(TItem item);

    TList Clear();

    (TList List, bool Result) Contains(TItem item);

    TList CopyTo(TItem[] array, int arrayIndex);

    (TList List, bool Result) Remove(TItem item);
}

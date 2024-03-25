using System.Collections;

using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
public interface IFluentList<TSelf, TItem> : IFluentCollection<TSelf, TItem>, IEnumerable<TItem>, IEnumerable
    where TSelf : IFluentList<TSelf, TItem>
{
    TItem this[int index]
    {
        get;
        set;
    }

    (TSelf List, int Result) IndexOf(TItem item);

    TSelf Insert(int index, TItem item);

    TSelf RemoveAt(int index);
}
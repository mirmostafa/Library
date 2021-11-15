using System.Collections;
using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
public interface IFluentList<TList, TItem> : IFluentCollection<TList, TItem>, IEnumerable<TItem>, IEnumerable//, IFluentList<TItem>
    where TList : IFluentList<TList, TItem>
{
    TItem this[int index]
    {
        get;
        set;
    }

    (TList List, int Result) IndexOf(TItem item);

    TList Insert(int index, TItem item);

    TList RemoveAt(int index);
}
using System.Collections;

using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.Collections;

[Fluent]
public abstract class ReadOnlySpecializedList<TItem, TEnumerable> : IEnumerable<TItem?>, IIndexable<TItem?>, IReadOnlyCollection<TItem?>, IReadOnlyList<TItem?>
        where TEnumerable : SpecializedListBase<TItem?, TEnumerable>
{
    protected ReadOnlySpecializedList(IEnumerable<TItem?> items)
        => this.InnerList = new(items);

    protected ReadOnlySpecializedList()
        => this.InnerList = new();

    public TItem? this[int index] => this.InnerList[index];

    public int Count => this.InnerList.Count;

    protected List<TItem?> InnerList { get; }

    public TItem? ByCriteria(Predicate<TItem?> predicate)
        => this.FirstOrDefault(x => predicate.ArgumentNotNull(nameof(predicate)).Invoke(x));

    public bool Contains(TItem? item)
        => this.InnerList.Contains(item);

    public IEnumerator<TItem?> GetEnumerator()
        => this.InnerList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)this.InnerList).GetEnumerator();

    public TEnumerable GetNew(IEnumerable<TItem?> items)
        => this.OnGetNew(items);

    public TEnumerable InCriteria(Predicate<TItem?> predicate)
        => this.OnGetNew(this.Where(x => predicate.ArgumentNotNull(nameof(predicate)).Invoke(x)));

    public int IndexOf(TItem? item)
        => this.InnerList.IndexOf(item);

    protected abstract TEnumerable OnGetNew(IEnumerable<TItem?> items);

    protected TEnumerable This()
        => this.As<TEnumerable>()!;

    protected TEnumerable This(params Delegate[] delegates)
    {
        foreach (var item in delegates)
        {
            _ = item.Method.Invoke(this, null);
        }
        return this.This();
    }
}
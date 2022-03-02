using Library.DesignPatterns.Markers;
using Library.Validations;
using System.Collections;

namespace Library.Collections;
[Fluent]
public abstract class ReadOnlySpecializedList<TItem, TEnumerable> : IEnumerable<TItem?>, IIndexable<TItem?>, IReadOnlyCollection<TItem?>, IReadOnlyList<TItem?>
        where TEnumerable : SpecializedListBase<TItem?, TEnumerable>
{
    protected ReadOnlySpecializedList(IEnumerable<TItem?> items)
        => this.InnetList = new(items);
    protected ReadOnlySpecializedList()
        => this.InnetList = new();

    protected List<TItem?> InnetList { get; }

    public TItem? this[int index]
    {
        get => this.InnetList[index];
        set => this.InnetList[index] = value;
    }

    public int Count => this.InnetList.Count;

    protected TEnumerable This()
        => this.As<TEnumerable>()!;

    public TItem? ByCriteria(Predicate<TItem?> predicate)
        => this.FirstOrDefault(x => predicate.ArgumentNotNull(nameof(predicate)).Invoke(x));
    public bool Contains(TItem? item)
        => this.InnetList.Contains(item);

    public IEnumerator<TItem?> GetEnumerator()
        => this.InnetList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)this.InnetList).GetEnumerator();

    public TEnumerable InCriteria(Predicate<TItem?> predicate)
        => this.OnGetNew(this.Where(x => predicate.ArgumentNotNull(nameof(predicate)).Invoke(x)));
    public int IndexOf(TItem? item)
        => this.InnetList.IndexOf(item);

    public TEnumerable GetNew(IEnumerable<TItem?> items)
        => this.OnGetNew(items);

    protected abstract TEnumerable OnGetNew(IEnumerable<TItem?> items);
}

using System.Collections;

using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
public interface IFluentSet<TItem, TSelf> : IIndexable<TItem>
{
    TSelf Add(TItem item);

    TSelf Clear();

    bool Contains(TItem item);

    TSelf Copy();

    TSelf CopyTo(TItem[] array, int arrayIndex);

    IEnumerator<TItem> GetEnumerator();

    TSelf Insert(int index, TItem item);

    bool Remove(TItem item);

    TSelf RemoveAt(int index);
}

[Fluent]
public sealed class Set<TItem> : IList<TItem>, IFluentSet<TItem, Set<TItem>>
{
    private readonly List<TItem> _items = new();

    public Set()
    {
    }

    public Set(IEnumerable<TItem> items)
        => this.AddRange(items);

    int ICollection<TItem>.Count => this._items.Count;

    bool ICollection<TItem>.IsReadOnly => ((ICollection<TItem>)this._items).IsReadOnly;

    public TItem this[int index] => this._items[0];

    TItem IList<TItem>.this[int index]
    {
        get => this._items[0];
        set
        {
            if (this._items.Contains(value))
            {
                this._items[index] = value;
            }
        }
    }

    public static Set<TItem> operator &(Set<TItem> set1, Set<TItem> set2)
        => new Set<TItem>()
            .AddRange(set1.Where(((ICollection<TItem>)set2).Contains))
            .AddRange(set2.Where(((ICollection<TItem>)set1).Contains));

    public static Set<TItem> operator |(Set<TItem> set1, Set<TItem> set2)
        => new Set<TItem>(set1).AddRange(set2);

    void ICollection<TItem>.Add(TItem item)
    {
        if (!this._items.Contains(item))
        {
            this._items.Add(item);
        }
    }

    public Set<TItem> Add(TItem item)
        => this.Do(x => x.Add(item));

    void ICollection<TItem>.Clear()
        => this._items.Clear();

    public Set<TItem> Clear()
        => this.Do(x => x.Clear());

    bool ICollection<TItem>.Contains(TItem item)
        => this._items.Contains(item);

    public bool Contains(TItem item)
        => this.Do(x => x.Contains(item));

    public Set<TItem> Copy()
        => new(this);

    void ICollection<TItem>.CopyTo(TItem[] array, int arrayIndex)
        => this._items.CopyTo(array, arrayIndex);

    public Set<TItem> CopyTo(TItem[] array, int arrayIndex)
        => this.Do(x => x.CopyTo(array, arrayIndex));

    public IEnumerator<TItem> GetEnumerator()
        => this._items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this._items.GetEnumerator();

    int IList<TItem>.IndexOf(TItem item)
        => this._items.IndexOf(item);

    void IList<TItem>.Insert(int index, TItem item)
    {
        if (!this._items.Contains(item))
        {
            this._items.Insert(index, item);
        }
    }

    public Set<TItem> Insert(int index, TItem item)
        => this.Do(x => x.Insert(index, item));

    bool ICollection<TItem>.Remove(TItem item)
        => this._items.Remove(item);

    public bool Remove(TItem item)
        => this.Do(x => x.Remove(item));

    void IList<TItem>.RemoveAt(int index)
        => this._items.RemoveAt(index);

    public Set<TItem> RemoveAt(int index)
        => this.Do(x => x.RemoveAt(index));

    private Set<TItem> Do(Action<ICollection<TItem>> action)
    {
        action(this);
        return this;
    }

    private Set<TItem> Do(Action<IList<TItem>> action)
    {
        action(this);
        return this;
    }

    private TResult Do<TResult>(Func<ICollection<TItem>, TResult> action)
        => action(this);

    private TResult Do<TResult>(Func<IList<TItem>, TResult> action)
        => action(this);
}
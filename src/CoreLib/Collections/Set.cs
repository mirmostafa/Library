using System.Collections;

namespace Library.Collections;

public sealed class Set<TItem> : IList<TItem>, IIndexable<TItem>
{
    private readonly List<TItem> _items = new();

    public Set()
    {
    }

    public Set(IEnumerable<TItem> items)
        => this.AddRange(items);

    public int Count => this._items.Count;

    public bool IsReadOnly => ((ICollection<TItem>)this._items).IsReadOnly;

    public TItem this[int index]
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
            .AddRange(set1.Where(set2.Contains))
            .AddRange(set2.Where(set1.Contains));

    public static Set<TItem> operator |(Set<TItem> set1, Set<TItem> set2)
            => new Set<TItem>(set1).AddRange(set2);

    public void Add(TItem item)
    {
        if (!this._items.Contains(item))
        {
            this._items.Add(item);
        }
    }

    public void Clear()
        => this._items.Clear();

    public bool Contains(TItem item)
        => this._items.Contains(item);

    public Set<TItem> Copy()
        => new(this);

    public void CopyTo(TItem[] array, int arrayIndex)
            => this._items.CopyTo(array, arrayIndex);

    public IEnumerator<TItem> GetEnumerator()
        => this._items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this._items.GetEnumerator();

    public int IndexOf(TItem item)
        => this._items.IndexOf(item);

    public void Insert(int index, TItem item)
    {
        if (!this._items.Contains(item))
        {
            this._items.Insert(index, item);
        }
    }

    public bool Remove(TItem item)
        => this._items.Remove(item);

    public void RemoveAt(int index)
        => this._items.RemoveAt(index);
}
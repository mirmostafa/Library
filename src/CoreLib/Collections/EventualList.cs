using System.Collections;

using Library.EventsArgs;
using Library.Helpers;

namespace Library.Collections;

[Obsolete("Subject to remove", true)]
public sealed class EventualList<TItem> : IList<TItem>
{
    private readonly List<TItem> _innerList = new();

    public event EventHandler? Cleared;

    public event EventHandler<ItemActedEventArgs<TItem>>? ItemAdded;

    public event EventHandler<ItemActedEventArgs<int>>? ItemIndexChanged;

    public event EventHandler<ItemActedEventArgs<int>>? ItemIndexRemoved;

    public event EventHandler<ItemActedEventArgs<(int Index, TItem Item)>>? ItemInserted;

    public event EventHandler<ItemActedEventArgs<TItem>>? ItemRemoved;

    public int Count
        => this._innerList.Count;

    bool ICollection<TItem>.IsReadOnly
        => this._innerList.Cast().As<IList<TItem>>()!.IsReadOnly;

    public TItem this[int index]
    {
        get => this._innerList[index];
        set
        {
            this._innerList[index] = value;
            ItemIndexChanged?.Invoke(this, new(index));
        }
    }

    public void Add(TItem item)
    {
        this._innerList.Add(item);
        ItemAdded?.Invoke(this, new(item));
    }

    public void Clear()
    {
        this._innerList.Clear();
        Cleared?.Invoke(this, EventArgs.Empty);
    }

    public bool Contains(TItem item)
        => this._innerList.Contains(item);

    public void CopyTo(TItem[] array, int arrayIndex)
        => this._innerList.CopyTo(array, arrayIndex);

    public IEnumerator<TItem> GetEnumerator()
        => this._innerList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this._innerList.GetEnumerator();

    public int IndexOf(TItem item)
        => this._innerList.IndexOf(item);

    public void Insert(int index, TItem item)
    {
        this._innerList.Insert(index, item);
        this.ItemInserted?.Invoke(this, new((index, item)));
    }

    public bool Remove(TItem item)
    {
        var result = this._innerList.Remove(item);
        ItemRemoved?.Invoke(this, new(item));
        return result;
    }

    public void RemoveAt(int index)
    {
        this._innerList.RemoveAt(index);
        ItemIndexRemoved?.Invoke(this, new(index));
    }
}
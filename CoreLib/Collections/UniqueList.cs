using Library.Exceptions.Validations;
using System.Collections;

namespace Library.Collections;
public class UniqueList<T> : IList<T>
{
    private readonly List<T> _InnerList;

    public UniqueList()
        => this._InnerList = new();

    public UniqueList(IEnumerable<T> items)
        => this._InnerList = new(items);

    public T this[int index]
    {
        get => this._InnerList[index];
        set => this._InnerList[index] = value;
    }

    public int Count
        => this._InnerList.Count;

    public bool IsReadOnly
        => ((ICollection<T>)this._InnerList).IsReadOnly;

    public void Add(T item)
    {
        if (this.Contains(item))
        {
            throw new ObjectDuplicateValidationException(nameof(item));
        }
        this._InnerList.Add(item);
    }

    public void Clear()
        => this._InnerList.Clear();

    public bool Contains(T item)
        => this.Any(i => this.IsEqual(item, i));

    protected virtual bool IsEqual(T? item1, T? item2)
        => item1?.Equals(item2) ?? item2 is null;

    public void CopyTo(T[] array, int arrayIndex)
        => this._InnerList.CopyTo(array, arrayIndex);
    public IEnumerator<T> GetEnumerator()
        => this._InnerList.GetEnumerator();
    public int IndexOf(T item)
        => this._InnerList.IndexOf(item);
    public void Insert(int index, T item)
        => this._InnerList.Insert(index, item);
    public bool Remove(T item)
        => this._InnerList.Remove(item);
    public void RemoveAt(int index)
        => this._InnerList.RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator()
        => this._InnerList.GetEnumerator();
}

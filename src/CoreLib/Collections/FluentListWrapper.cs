using System.Collections;

using Library.Validations;

namespace Library.Collections;

public sealed class FluentListWrapper<TItem, TList>(TList list) : IFluentList<FluentListWrapper<TItem, TList>, TItem>
    where TList : IList<TItem>
{
    private readonly TList _list = list;

    public TItem this[int index]
    {
        get => this._list[index];
        set => this._list[index] = value;
    }

    public int Count { get; }
    public bool IsReadOnly { get; }

    public static explicit operator TList(FluentListWrapper<TItem, TList> x)
        => x.ArgumentNotNull()._list;

    public FluentListWrapper<TItem, TList> Add(TItem item) => this.Fluent(() => this._list.Add(item));

    public TList AsList() => this._list;

    public FluentListWrapper<TItem, TList> Clear() => this.Fluent(() => this._list.Clear);

    public (FluentListWrapper<TItem, TList> List, bool Result) Contains(TItem item) => (this, this._list.Contains(item));

    public FluentListWrapper<TItem, TList> CopyTo(TItem[] array, int arrayIndex) => this.Fluent(() => this._list.CopyTo(array, arrayIndex));

    public IEnumerator<TItem> GetEnumerator() => this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this._list).GetEnumerator();

    public (FluentListWrapper<TItem, TList> List, int Result) IndexOf(TItem item) => (this, this._list.IndexOf(item));

    public FluentListWrapper<TItem, TList> Insert(int index, TItem item) => this.Fluent(() => this._list.Insert(index, item));

    public FluentListWrapper<TItem, TList> Remove(TItem item) => this.Fluent(() => this._list.Remove(item));

    public FluentListWrapper<TItem, TList> RemoveAt(int index) => this.Fluent(() => this._list.RemoveAt(index));
}
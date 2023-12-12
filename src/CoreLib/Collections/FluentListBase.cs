using System.Collections;

using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
public abstract class FluentListBase<TItem, TList> : IFluentList<TList, TItem>, IBuilder<List<TItem>>
    where TList : FluentListBase<TItem, TList>
{
    private readonly List<TItem> _list;

    protected FluentListBase(List<TItem> list) =>
        this._list = list ?? [];

    protected FluentListBase(IEnumerable<TItem> list) =>
        this._list = new List<TItem>(list);

    protected FluentListBase(int capacity) =>
        this._list = new List<TItem>(capacity);

    protected FluentListBase() =>
        this._list = [];

    public int Count => this._list.Count;

    bool IFluentCollection<TList, TItem>.IsReadOnly { get; }

    public TItem this[int index]
    {
        get => this._list[index];
        set => this._list[index] = value;
    }

    private TList This => this.Cast().As<TList>()!;

    public TList Add(TItem item) =>
        this.This.Fluent(() => this._list.Add(item));

    public List<TItem> AsList() =>
        this._list;

    public List<TItem> Build() =>
        this.AsList();

    public TList Clear() =>
        this.This.Fluent(this._list.Clear);

    public (TList List, bool Result) Contains(TItem item) =>
        (this.This, this._list.Contains(item));

    public TList CopyTo(TItem[] array, int arrayIndex) =>
        this.This.Fluent(() => this._list.CopyTo(array, arrayIndex));

    public IEnumerator<TItem> GetEnumerator() =>
        this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable)this._list).GetEnumerator();

    public (TList List, int Result) IndexOf(TItem item) =>
        (this.This, this._list.IndexOf(item));

    public TList Insert(int index, TItem item) =>
        this.This.Fluent(() => this._list.Insert(index, item));

    public TList Remove(TItem item) =>
        this.This.Fluent(this._list.Remove(item));

    public TList RemoveAt(int index) =>
        this.This.Fluent(() => this._list.RemoveAt(index));
}
using System.Collections;
using System.Diagnostics;

using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
//x [Immutable]
public class FluentListBase<TItem, TList> : IFluentList<TList, TItem>
    where TList : FluentListBase<TItem, TList>
{
    private readonly IList<TItem> _list;
    private bool _isReadOnly;

    protected FluentListBase(IList<TItem> list)
        => this._list = list ?? new List<TItem>();

    protected FluentListBase(IEnumerable<TItem> list)
        => this._list = new List<TItem>(list);

    protected FluentListBase(int capacity)
        => this._list = new List<TItem>(capacity);

    protected FluentListBase()
        => this._list = new List<TItem>();

    public TItem this[int index]
    {
        get => this._list[index];
        set
        {
            _ = this.CheckReadOnly();
            this._list[index] = value;
        }
    }

    public int Count => this._list.Count;

    public bool IsReadOnly
    {
        get => this._isReadOnly || this._list.IsReadOnly;
        protected set => this._isReadOnly = value;
    }

    bool IFluentCollection<TList, TItem>.IsReadOnly { get; }

    private TList This => this.As<TList>()!;

    public TList Add(TItem item)
        => this.This.Fluent(this.CheckReadOnly).With(() => this._list.Add(item));

    public List<TItem> AsList() =>
        this._list.ToList();

    public TList Clear()
        => this.This.Fluent(this.CheckReadOnly).With(this._list.Clear);

    public (TList List, bool Result) Contains(TItem item)
        => (this.This, this._list.Contains(item));

    public TList CopyTo(TItem[] array, int arrayIndex)
        => this.This.Fluent(() => this._list.CopyTo(array, arrayIndex));

    public IEnumerator<TItem> GetEnumerator()
        => this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)this._list).GetEnumerator();

    public (TList List, int Result) IndexOf(TItem item)
            => this.OnIndexOf(item);

    public TList Insert(int index, TItem item)
        => this.OnInsert(index, item);

    public (TList List, bool Result) Remove(TItem item)
        => this.This.Fluent(this.CheckReadOnly).Result(() => this._list.Remove(item))!;

    public TList RemoveAt(int index)
        => this.This.Fluent(this.CheckReadOnly).With(() => this._list.RemoveAt(index));

    [DebuggerStepThrough]
    protected TList CheckReadOnly()
        => !this.IsReadOnly ? this.This : throw new Exceptions.InvalidOperationException("This list is marked as [Read Only].");

    private (TList List, int Result) OnIndexOf(TItem item)
        => (this.This, this._list.IndexOf(item));

    private TList OnInsert(int index, TItem item)
        => this.This.Fluent(this.This.CheckReadOnly).With(() => this._list.Insert(index, item));
}
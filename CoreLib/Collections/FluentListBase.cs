using System.Collections;
using System.Diagnostics;
using Library.Coding;
using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
[Immutable]
public class FluentListBase<TItem, TList> : IFluentList<TList, TItem>//, IImmutableList<TItem>
    where TList : FluentListBase<TItem, TList>
{
    private readonly IList<TItem> _list;
    private bool _isReadOnly;

    public bool IsReadOnly
    {
        get => this._isReadOnly || this._list.IsReadOnly;
        protected set => this._isReadOnly = value;
    }

    public int Count =>
        this._list.Count;

    bool IFluentCollection<TList, TItem>.IsReadOnly { get; }

    public TItem this[int index]
    {
        get => this._list[index]; set
        {
            this.CheckReadOnly();
            this._list[index] = value;
        }
    }

    [DebuggerStepThrough]
    protected TList CheckReadOnly() =>
       !this.IsReadOnly ? this.This : throw new Exceptions.InvalidOperationException("This list is marked as [Read Only].");

    protected FluentListBase(IList<TItem> list) =>
        this._list = list ?? new List<TItem>();
    protected FluentListBase(IEnumerable<TItem> list) =>
        this._list = new List<TItem>(list);
    protected FluentListBase(int capacity) =>
        this._list = new List<TItem>(capacity);
    protected FluentListBase() =>
        this._list = new List<TItem>();

    public (TList List, int Result) IndexOf(TItem item) =>
        (this.This, this._list.IndexOf(item));
    public TList Insert(int index, TItem item) =>
        this.Fluent(this.CheckReadOnly).This.Fluent(() => this._list.Insert(index, item));
    public TList RemoveAt(int index) =>
        this.Fluent(this.CheckReadOnly).This.Fluent(() => this._list.RemoveAt(index));
    public TList Add(TItem item) =>
        this.Fluent(this.CheckReadOnly).This.Fluent(() => this._list.Add(item));
    public TList Clear() =>
        this.Fluent(this.CheckReadOnly).This.Fluent(() => this._list.Clear());
    public (TList List, bool Result) Contains(TItem item) =>
        (this.This, this._list.Contains(item));
    public TList CopyTo(TItem[] array, int arrayIndex) =>
        this.This.Fluent(() => this._list.CopyTo(array, arrayIndex));
    public (TList List, bool Result) Remove(TItem item) =>
        this.Fluent(this.CheckReadOnly).This.FluentByResult(() => this._list.Remove(item));

    public IEnumerator<TItem> GetEnumerator() =>
        this._list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable)this._list).GetEnumerator();

    public List<TItem> AsList() =>
        this._list.ToList();

    private TList This =>
        this.As<TList>()!;
}

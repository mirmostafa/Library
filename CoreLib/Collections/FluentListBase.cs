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
    private readonly List<TItem> _List;

    public bool IsReadOnly { get; protected set; }

    public int Count =>
        this._List.Count;

    bool IFluentCollection<TList, TItem>.IsReadOnly { get; }

    public TItem this[int index]
    {
        get => this._List[index]; set
        {
            this.CheckReadOnly();
            this._List[index] = value;
        }
    }

    [DebuggerStepThrough]
    protected TList CheckReadOnly() =>
       !this.IsReadOnly ? this.This : throw new Exceptions.InvalidOperationException("This list is marked as [Read Only].");

    protected FluentListBase(List<TItem> list) =>
        this._List = list;
    protected FluentListBase(IEnumerable<TItem> list) =>
        this._List = new(list);
    protected FluentListBase(int capacity) =>
        this._List = new(capacity);
    protected FluentListBase() =>
        this._List = new();

    public (TList List, int Result) IndexOf(TItem item) =>
        (this.This, this._List.IndexOf(item));
    public TList Insert(int index, TItem item) =>
        this.Fluent(this.CheckReadOnly).This.Fluent(() => this._List.Insert(index, item));
    public TList RemoveAt(int index) =>
        this.Fluent(this.CheckReadOnly).This.Fluent(() => this._List.RemoveAt(index));
    public TList Add(TItem item) =>
        this.Fluent(this.CheckReadOnly).This.Fluent(() => this._List.Add(item));
    public TList Clear() =>
        this.Fluent(this.CheckReadOnly).This.Fluent(() => this._List.Clear());
    public (TList List, bool Result) Contains(TItem item) =>
        (this.This, this._List.Contains(item));
    public TList CopyTo(TItem[] array, int arrayIndex) =>
        this.This.Fluent(() => this._List.CopyTo(array, arrayIndex));
    public (TList List, bool Result) Remove(TItem item) =>
        this.Fluent(this.CheckReadOnly).This.FluentByResult(() => this._List.Remove(item));

    public IEnumerator<TItem> GetEnumerator() =>
        this._List.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable)this._List).GetEnumerator();

    public List<TItem> AsList() =>
        this._List.ToList();

    private TList This =>
        this.As<TList>()!;
}

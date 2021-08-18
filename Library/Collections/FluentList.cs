using System.Collections;
using Library.Coding;
using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
public class FluentList<T> : IFluentList<FluentList<T>, T>
{
    private readonly List<T> _List;

    public int Count => this._List.Count;

    bool IFluentCollection<FluentList<T>, T>.IsReadOnly { get; }

    public T this[int index] { get => this._List[index]; set => this._List[index] = value; }

    private FluentList(List<T> list) => this._List = list;
    private FluentList(IEnumerable<T> list) => this._List = new(list);

    private FluentList() : this(new()) { }

    public static FluentList<T> Create() => new();
    public static FluentList<T> Create(List<T> list) => new(list);
    public static FluentList<T> Create(IEnumerable<T> list) => new(list);

    public (FluentList<T> List, int Result) IndexOf(T item) => (this, this._List.IndexOf(item));
    public FluentList<T> Insert(int index, T item) => this.Fluent(() => this._List.Insert(index, item));
    public FluentList<T> RemoveAt(int index) => this.Fluent(() => this._List.RemoveAt(index));
    public FluentList<T> Add(T item) => this.Fluent(() => this._List.Add(item));
    public FluentList<T> Clear() => this.Fluent(() => this._List.Clear());
    public (FluentList<T> List, bool Result) Contains(T item) => (this, this._List.Contains(item));
    public FluentList<T> CopyTo(T[] array, int arrayIndex) => this.Fluent(() => this._List.CopyTo(array, arrayIndex));
    public (FluentList<T>, bool) Remove(T item) => this.FluentByResult(() => this._List.Remove(item));

    public IEnumerator<T> GetEnumerator() => this._List.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this._List).GetEnumerator();

    public static implicit operator List<T>(in FluentList<T> wrapper) => wrapper._List;
}

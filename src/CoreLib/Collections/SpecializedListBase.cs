using Library.DesignPatterns.Markers;

namespace Library.Collections;

[Fluent]
public abstract class SpecializedListBase<TItem, TEnumerable> : ReadOnlySpecializedList<TItem, TEnumerable>//, IList<TItem?>
        where TEnumerable : SpecializedListBase<TItem?, TEnumerable>
{
    protected SpecializedListBase(IEnumerable<TItem?> items)
        : base(items) { }

    protected SpecializedListBase()
        : base() { }

    public bool IsReadOnly => ((ICollection<TItem?>)this.InnerList).IsReadOnly;

    public TEnumerable Add(TItem? item)
    {
        this.OnInserting(this.Count, item);
        this.AsList().Add(item);
        return this.This();
    }

    public TEnumerable Clear()
    {
        this.AsList().Clear();
        return this.This();
    }

    public TEnumerable CopyTo(TItem?[] array, int arrayIndex)
    {
        this.AsList().CopyTo(array, arrayIndex);
        return this.This();
    }

    public TEnumerable Insert(int index, TItem? item)
    {
        this.OnInserting(index, item);
        this.AsList().Insert(index, item);
        return this.This();
    }

    public TEnumerable Remove(TItem? item)
    {
        _ = this.AsList().Remove(item);
        return this.This();
    }

    public TEnumerable RemoveAt(int index)
    {
        this.AsList().RemoveAt(index);
        return this.This();
    }

    //void ICollection<TItem?>.Add(TItem? item)
    //{
    //    this.OnInserting(this.Count, item);
    //    this.InnetList.Add(item);
    //}

    //void ICollection<TItem?>.Clear()
    //    => this.InnetList.Clear();

    //void ICollection<TItem?>.CopyTo(TItem?[] array, int arrayIndex)
    //    => this.InnetList.CopyTo(array, arrayIndex);

    //void IList<TItem?>.Insert(int index, TItem? item)
    //{
    //    this.OnInserting(index, item);
    //    this.InnetList.Insert(index, item);
    //}

    //bool ICollection<TItem?>.Remove(TItem? item)
    //    => this.InnetList.Remove(item);

    //void IList<TItem?>.RemoveAt(int index)
    //    => this.InnetList.RemoveAt(index);

    protected IList<TItem?> AsList()
        => this.InnerList.Enumerate().ToList();

    protected virtual void OnInserting(int index, TItem? item)
    { }
}
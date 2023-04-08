namespace Library.Collections;

public sealed class FluentList<TItem> : FluentListBase<TItem, FluentList<TItem>>, IFluentList<FluentList<TItem>, TItem>
{
    public FluentList()
        : base()
    {
    }

    protected FluentList(List<TItem> list)
        : base(list)
    {
    }

    protected FluentList(IEnumerable<TItem> list)
        : base(list)
    {
    }

    public static FluentList<TItem> Create()
        => new();
    public static FluentList<TItem> Create(List<TItem> list)
        => new(list);
    public static FluentList<TItem> Create(IEnumerable<TItem> list)
        => new(list);

    public static implicit operator List<TItem>(in FluentList<TItem> fluentList)
        => fluentList.AsList();
}
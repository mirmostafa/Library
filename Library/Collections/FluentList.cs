﻿namespace Library.Collections;

public class FluentList<TItem> : FluentListBase<TItem, FluentList<TItem>>, IFluentList<FluentList<TItem>, TItem>
{
    protected FluentList()
    {
    }

    protected FluentList(List<TItem> list) : base(list)
    {
    }

    protected FluentList(IEnumerable<TItem> list) : base(list)
    {
    }

    public static FluentList<TItem> Create() => new();
    public static FluentList<TItem> Create(List<TItem> list) => new(list);
    public static FluentList<TItem> Create(IEnumerable<TItem> list) => new(list);

    public static implicit operator List<TItem>(in FluentList<TItem> fluentList) => fluentList.AsList();

}
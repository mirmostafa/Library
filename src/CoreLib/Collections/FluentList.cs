using Library.Validations;

namespace Library.Collections;

public sealed class FluentList<TItem> : FluentListBase<TItem, FluentList<TItem>>, IFluentList<FluentList<TItem>, TItem>
{
    /// <summary>
    /// Constructor for FluentList class.
    /// </summary>
    /// <returns>An instance of FluentList class.</returns>
    public FluentList()
        : base()
    {
    }

    private FluentList(List<TItem> list)
        : base(list)
    {
    }

    private FluentList(IEnumerable<TItem> list)
        : base(list)
    {
    }

    /// <summary> Creates a new instance of FluentList<TItem>. </summary> <returns>A new instance of FluentList<TItem>.</returns>
    public static FluentList<TItem> Create() =>
        [];

    /// <summary>
    /// Creates a new FluentList from a given List.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the list.</typeparam>
    /// <param name="list">The list to create the FluentList from.</param>
    /// <returns>A new FluentList containing the items from the given list.</returns>
    public static FluentList<TItem> Create(List<TItem> list) =>
        new(list);

    /// <summary>
    /// Creates a new FluentList from the given IEnumerable.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the list.</typeparam>
    /// <param name="list">The IEnumerable to create the FluentList from.</param>
    /// <returns>A new FluentList containing the items from the given IEnumerable.</returns>
    public static FluentList<TItem> Create(IEnumerable<TItem> list) =>
        new(list);
    /// <summary>
    /// Converts a FluentList to a List.
    /// </summary>
    public static implicit operator List<TItem>(in FluentList<TItem> fluentList) =>
        fluentList.ArgumentNotNull().AsList();
}
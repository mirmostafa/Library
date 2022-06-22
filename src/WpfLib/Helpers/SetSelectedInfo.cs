namespace Library.Wpf.Helpers;

public record SetSelectedInfo<T>
{
    /// <summary>
    ///     Gets or sets the chain of items to search for. The last item in the chain will be selected.
    /// </summary>
    public IEnumerable<T>? Items { get; set; }

    /// <summary>
    ///     Gets or sets the method used to compare items in the control with items in the chain
    /// </summary>
    public Func<T, T, bool>? CompareMethod { get; set; }

    /// <summary>
    ///     Gets or sets the method used to convert items in the control to be compare with items in the chain
    /// </summary>
    public Func<object, T>? ConvertMethod { get; set; }

    /// <summary>
    ///     Gets or sets the method used to select the final item in the chain
    /// </summary>
    public SetSelectedEventHandler<T>? OnSelected { get; set; }

    /// <summary>
    ///     Gets or sets the method used to request more child items to be generated in the control
    /// </summary>
    public SetSelectedEventHandler<T>? OnNeedMoreItems { get; set; }
}

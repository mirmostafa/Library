namespace Library.EventsArgs;

public class ChangedEventArgs<T> : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ChangedEventArgs{T}" /> class.
    /// </summary>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    public ChangedEventArgs(T oldValue, T newValue)
    {
        this.OldValue = oldValue;
        this.NewValue = newValue;
    }

    /// <summary>
    ///     Gets the old value.
    /// </summary>
    /// <value>
    ///     The old value.
    /// </value>
    public T OldValue { get; }

    /// <summary>
    ///     Creates new value.
    /// </summary>
    /// <value>
    ///     The new value.
    /// </value>
    public T NewValue { get; set; }
}

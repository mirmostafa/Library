namespace Library.EventsArgs;

public sealed class ChangingEventArgs<T> : ActingEventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ChangingEventArgs{T}" /> class.
    /// </summary>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    public ChangingEventArgs(T oldValue, T newValue)
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
    ///     Gets the new value.
    /// </summary>
    /// <value>
    ///     The new value.
    /// </value>
    public T NewValue { get; set; }
}

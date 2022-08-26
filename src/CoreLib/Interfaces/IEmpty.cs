namespace Library.Interfaces;

public interface IEmpty<out T>
{
    /// <summary>
    /// Gets an empty instance of current class.
    /// </summary>
    /// <value>
    /// An empty instance.
    /// </value>
    static abstract T Empty { get; }
    /// <summary>
    /// Creates an empty instance of current class.
    /// </summary>
    /// <returns>An empty instance of current class.</returns>
    static abstract T NewEmpty();
}

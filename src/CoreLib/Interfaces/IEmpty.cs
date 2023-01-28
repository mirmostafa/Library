namespace Library.Interfaces;

public interface IEmpty<out TSelf>
{
    /// <summary>
    /// Gets an empty instance of current class.
    /// </summary>
    /// <value>
    /// An empty instance.
    /// </value>
    static abstract TSelf Empty { get; }
    /// <summary>
    /// Creates an empty instance of current class.
    /// </summary>
    /// <returns>An empty instance of current class.</returns>
    static abstract TSelf NewEmpty();
}

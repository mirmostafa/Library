namespace Library.Interfaces;

public interface IEmpty<out TClass>
{
    /// <summary>
    /// Gets an empty instance of currebt class.
    /// </summary>
    /// <value>
    /// An empty instance.
    /// </value>
    static abstract TClass Empty { get; }
    /// <summary>
    /// Creates an empty instance of currebt class.
    /// </summary>
    /// <returns>An empty instance of currebt class.</returns>
    static abstract TClass NewEmpty();
}

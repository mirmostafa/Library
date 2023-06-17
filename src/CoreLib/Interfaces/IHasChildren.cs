namespace Library.Interfaces;

public interface IHasChildren<out TChild>
{
    IEnumerable<TChild> Children { get; }
}

public interface IParent<TChild>
{
    /// <summary>
    /// Gets the children.
    /// </summary>
    /// <value>The children.</value>
    IList<TChild> Children { get; }
}
using System.Collections;

namespace Library.Interfaces;

public interface IHasChildren<TChild>
{
    IEnumerable<TChild> Children { get; }
}


public interface IChildrenList<TClass, TChild> : IHasChildren<TChild>
{
    /// <summary>
    /// Adds the child.
    /// </summary>
    /// <param name="children">The children.</param>
    /// <returns></returns>
    TClass AddChild(params TChild[] children);
}

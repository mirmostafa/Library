using System.Collections;

namespace Library.Interfaces;

public interface IHasChild
{
    IEnumerable Children { get; }
}

public interface IHasChild<TChild> : IHasChild
{
    new IEnumerable<TChild> Children { get; }
}

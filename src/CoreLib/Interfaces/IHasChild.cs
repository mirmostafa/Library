using System.Collections;

namespace Library.Interfaces;

public interface IHasChild<TChild>
{
    IEnumerable<TChild> Children { get; }
}

using System.Collections.Generic;

namespace Mohammad.Collections.ObjectModel
{
    public interface IEventualCollection<T> : IEventualEnumerable<T>, ICollection<T> {}
}
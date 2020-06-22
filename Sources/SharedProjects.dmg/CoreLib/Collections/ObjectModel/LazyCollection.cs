using Mohammad.Bcl;

namespace Mohammad.Collections.ObjectModel
{
    public class LazyCollection<T> : LazyInit<EventualCollection<T>>
    {
        public LazyCollection()
            : base(() => new EventualCollection<T>()) { }
        public LazyCollection(LazyInitMode mode)
            : base(() => new EventualCollection<T>(), mode) { }
    }
}
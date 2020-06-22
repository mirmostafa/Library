using System;

namespace Mohammad.Bcl
{
    public class LazyInitNew<T> : LazyInit<T>
        where T : class, new()
    {
        protected LazyInitNew(Func<T> creator)
            : base(creator) { }

        protected LazyInitNew(Func<T> creator, LazyInitMode mode)
            : base(creator, mode) { }

        public LazyInitNew(LazyInitMode mode = LazyInitMode.FirstGet)
            : base(() => new T(), mode) { }
    }
}
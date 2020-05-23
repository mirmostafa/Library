using System;

namespace Mohammad.Collections.Generic
{
    public sealed class LibEqualityComparer<T> : EqualityComparerBase<T>
    {
        /// <inheritdoc />
        public LibEqualityComparer(Func<T, T, bool> comparer, Func<T, int> getHashCode)
            : base(comparer, getHashCode) { }
    }
}
using System;
using System.Collections.Generic;

namespace Mohammad.Collections.Generic
{
    public abstract class EqualityComparerBase<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _Comparer;
        private readonly Func<T, int> _GetHashCode;

        protected EqualityComparerBase(Func<T, T, bool> comparer, Func<T, int> getHashCode)
        {
            this._GetHashCode = getHashCode;
            this._Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public override bool Equals(T x, T y) => this._Comparer(x, y);

        public override int GetHashCode(T obj) => this._GetHashCode == null
            ? Equals(obj, null)
                ? 0
                : obj.GetHashCode()
            : this._GetHashCode(obj);
    }
}
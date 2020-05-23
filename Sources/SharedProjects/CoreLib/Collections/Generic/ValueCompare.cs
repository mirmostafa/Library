using System;
using System.Collections.Generic;

namespace Mohammad.Collections.Generic
{
    public class ValueCompare<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _Compare;

        public ValueCompare(Func<T, T, int> compare)
        {
            if (compare == null)
                throw new ArgumentNullException(nameof(compare));
            this._Compare = compare;
        }

        public static ValueCompare<T> Get(Func<T, T, int> compare) => new ValueCompare<T>(compare);

        public int Compare(T x, T y) => this._Compare(x, y);
    }
}
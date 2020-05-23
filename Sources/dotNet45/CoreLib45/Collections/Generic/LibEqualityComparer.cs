#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Collections.Generic
{
    public sealed class LibEqualityComparer<T> : EqualityComparerBase<T>
    {
        /// <inheritdoc />
        public LibEqualityComparer(Func<T, T, bool> comparer, Func<T, int> getHashCode)
            : base(comparer, getHashCode)
        {
        }
    }
}
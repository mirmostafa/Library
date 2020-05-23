#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;

namespace Mohammad.Collections.Generic
{
    public interface IIndexerEnumerable<out T> : IEnumerable<T>
    {
        T this[int index] { get; }
    }

    public interface IIndexerEnumerable<out TResult, in TIndex> : IEnumerable<TResult>
    {
        TResult this[TIndex index] { get; }
    }
}
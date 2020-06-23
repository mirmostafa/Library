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
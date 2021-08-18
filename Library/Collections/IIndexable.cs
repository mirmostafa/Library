namespace Library.Collections;
public interface IIndexable<out TResult, TIndex>
{
    TResult this[TIndex index] { get; }
}

public interface IIndexable<out TResult> : IIndexable<TResult, int>, IReadOnlyList<TResult>
{
}

public interface IStringIndexable<out TResult> : IIndexable<TResult, string>
{
}

public interface IIndexedEnumerable<out TResult, TIndex> : IIndexable<TResult, TIndex>, IEnumerable<TResult>
{

}

public interface IIndexedEnumerable<out TResult> : IIndexedEnumerable<TResult, int>
{

}

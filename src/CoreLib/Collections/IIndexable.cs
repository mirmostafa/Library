namespace Library.Collections;

public interface IIndexable<in TIndex, out TResult>
{
    TResult this[TIndex index] { get; }
}

public interface IIndexable<out TResult> : IIndexable<int, TResult>
{
}

public interface IStringIndexable<out TResult> : IIndexable<string, TResult>
{
}
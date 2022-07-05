namespace Library.Interfaces;

public interface IMargable<out TOut, in TIn>
{
    TOut Merge(TIn o);
}

public interface IMergable<T> : IMargable<T, T>
{
}
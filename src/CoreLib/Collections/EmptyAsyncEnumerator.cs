using Library.Interfaces;

namespace Library.Collections;

internal class EmptyAsyncEnumerable<T> : IAsyncEnumerable<T>, IEmpty<IAsyncEnumerable<T>>
{
    private static IAsyncEnumerable<T> _empty;
    public static IAsyncEnumerable<T> Empty => _empty ??= NewEmpty();
    public static IAsyncEnumerable<T> NewEmpty()
        => new EmptyAsyncEnumerable<T>();

    private EmptyAsyncEnumerable()
    {

    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
        EmptyAsyncEnumerator<T>.Instance;

    private class EmptyAsyncEnumerator<U> : IAsyncEnumerator<T>
    {
        public static readonly EmptyAsyncEnumerator<U> Instance = new();

        public T Current => default!;

        public ValueTask DisposeAsync()
            => default;

        public ValueTask<bool> MoveNextAsync()
            => new(false);
    }
}
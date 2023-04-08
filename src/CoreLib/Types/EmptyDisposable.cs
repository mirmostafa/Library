using Library.Interfaces;

namespace Library.Types;

public sealed class EmptyDisposable : IDisposable, IEmpty<EmptyDisposable>
{
    private static EmptyDisposable? _empty;
    private bool _disposedValue;
    public static EmptyDisposable Empty => _empty ??= NewEmpty();

    public static EmptyDisposable NewEmpty() => new();

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
            {
            }
            this._disposedValue = true;
        }
    }
}
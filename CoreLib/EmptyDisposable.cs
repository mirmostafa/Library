using Library.Interfaces;

namespace Library;
public class EmptyDisposable : IDisposable, ISupportEmpty<EmptyDisposable>
{
    private bool _disposedValue;
    private static EmptyDisposable _empty;

    public static EmptyDisposable Empty => _empty ??= NewEmpty();

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
            {
            }
            this._disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public static EmptyDisposable NewEmpty() => new();
}
namespace Library;
public class EmptyDisposable : IDisposable
{
    private bool _disposedValue;

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
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public readonly static IDisposable Empty = new EmptyDisposable();
}
using System.Diagnostics.CodeAnalysis;

namespace Library.Helpers
{
    public abstract class PromiseOnDisposeBase : IAsyncDisposable, IDisposable
    {
        private bool _DisposedValue;

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public virtual ValueTask DisposeAsync()
        {
            try
            {
                this.Dispose();
                return default;
            }
            catch (Exception exc)
            {
                return new ValueTask(Task.FromException(exc));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._DisposedValue)
            {
                if (disposing)
                {
                    this.OnDispose();
                }
                this._DisposedValue = true;
            }
        }

        protected abstract void OnDispose();
    }
}
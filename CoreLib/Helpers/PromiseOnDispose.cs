using Library.Validations;

namespace Library.Helpers;

public class PromiseOnDispose : PromiseOnDisposeBase
{
    private readonly Action _OnDispose;

    public PromiseOnDispose(Action onDispose)
        => this._OnDispose = onDispose;

    public static PromiseOnDispose Begin(Action onBegin, Action onDispose)
    {
        try
        {
            onBegin.ArgumentNotNull(nameof(onBegin))();
            return new(onDispose);
        }
        catch
        {
            onDispose?.Invoke();
            throw;
        }
    }

    protected override void OnDispose()
        => this._OnDispose();
}

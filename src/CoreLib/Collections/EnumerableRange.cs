using System.Collections;

using Library.DesignPatterns.Markers;

namespace Library.Collections;

internal sealed class Enumerator<TItem>(Func<TItem, (bool isOk, TItem next)> tryGetNext, Func<TItem>? getInitialValue = default, Action? onDispose = null) : IEnumerator<TItem>
{
    private TItem _current = default!;
    private bool _isFirst = true;

    public TItem Current
    {
        get
        {
            if (this._isFirst)
            {
                var next = getInitialValue != null ? getInitialValue() : tryGetNext(this._current).next;
                this._current = next;
                this._isFirst = false;
            }
            return this._current;
        }
    }

    object? IEnumerator.Current => this.Current;

    public void Dispose()
        => onDispose?.Invoke();

    public bool MoveNext()
    {
        var isOk = true;
        TItem? next;

        if (this._isFirst)
        {
            if (getInitialValue != null)
            {
                next = getInitialValue();
            }
            else
            {
                (isOk, next) = tryGetNext(this._current);
            }

            this._current = next;
            this._isFirst = false;
        }
        else
        {
            (isOk, next) = tryGetNext(this._current);
        }

        if (isOk)
        {
            this._current = next;
            return true;
        }

        return false;
    }

    public void Reset()
    {
        this._isFirst = true;
        this._current = default!;
    }
}

[Immutable]
public sealed class LazyEnumerable<TItem>(IEnumerator<TItem> enumerator) : IEnumerable<TItem>
{
    public static IEnumerable<TItem> New(Func<TItem, (bool isOk, TItem next)> tryGetNext, Func<TItem>? getInitialValue = default, Action? onDispose = null)
        => new LazyEnumerable<TItem>(new Enumerator<TItem>(tryGetNext, getInitialValue, onDispose));

    public IEnumerator<TItem> GetEnumerator()
        => enumerator;

    IEnumerator IEnumerable.GetEnumerator()
        => enumerator;
}
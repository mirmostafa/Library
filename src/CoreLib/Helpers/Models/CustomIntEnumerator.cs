using System.Collections;

namespace Library.Helpers.Models;

public sealed class CustomIntEnumerator : IEnumerator<int>
{
    private readonly int _end;
    private readonly Func<CustomIntEnumerator, bool> _moveNext;
    private readonly int _start;

    public CustomIntEnumerator(Range range)
    {
        this._start = range.Start.Value - 1;
        this._end = range.End.Value;
        this.Current = this._start;
        var moveNextStrategy = detectMoveNextStrategy(this);

        this._moveNext = moveNextStrategy;

        static Func<CustomIntEnumerator, bool> detectMoveNextStrategy(CustomIntEnumerator me)
        {
            var moveNextStrategy =
                me._start < me._end
                ? ((CustomIntEnumerator me) =>
                {
                    me.Current++;
                    return me.Current <= me._end;
                })
                : (Func<CustomIntEnumerator, bool>)(me._start > me._end
                    ? ((CustomIntEnumerator me) =>
                                {
                                    me.Current--;
                                    return me.Current >= me._start;
                                })
                    : ((CustomIntEnumerator me) =>
                                {
                                    return false;
                                }));
            return moveNextStrategy;
        }
    }

    public int Current { get; private set; }

    object IEnumerator.Current => this.Current;

    public void Dispose()
    { }

    public bool MoveNext()
        => this._moveNext(this);

    public void Reset()
        => this.Current = this._start;
}
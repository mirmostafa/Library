using System.Collections;
using System.Diagnostics;

using Library.Collections;

namespace Library.Coding;

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Final : IDisposable, IEnumerable<Action>
{
    private readonly ActionList _actionList;

    public Final(ActionList actionList)
        => this._actionList = actionList;

    public Final(Action action)
        : this(new ActionList(new[] { action })) { }

    public static Final Block(Action final)
        => new(final);

    public static void TryFinial(Action final, Action action)
    {
        using var fin = new Final(final);
        action?.Invoke();
    }

    public void Dispose()
        => this.Done();

    public void Done()
        => this.ForEachEager(action => action?.Invoke());

    public IEnumerator<Action> GetEnumerator()
        => this._actionList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => (this._actionList as IEnumerable).GetEnumerator();
}
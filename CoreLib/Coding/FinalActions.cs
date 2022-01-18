using System.Collections;
using Library.Collections;

namespace Library.Coding;

public sealed class FinalActions : IDisposable, IEnumerable<Action>
{
    private readonly ActionList _actionList;

    public FinalActions(ActionList actionList) =>
        this._actionList = actionList;
    public FinalActions(Action action)
        : this(new ActionList(new[] { action })) { }

    public void Dispose() =>
        this.Done();

    public void Done() =>
        this.ForEachEager(action => action?.Invoke());

    public IEnumerator<Action> GetEnumerator() =>
        this._actionList.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() =>
        (this._actionList as IEnumerable).GetEnumerator();
    public static void TryFinall(Action finall, Action action)
    {
        using var fin = new FinalActions(finall);
        action?.Invoke();
    }
}

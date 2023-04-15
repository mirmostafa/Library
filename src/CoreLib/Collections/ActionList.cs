namespace Library.Collections;

[Obsolete("Subject to remove", true)]
public sealed class ActionList : FluentListBase<Action, ActionList>
{
    public ActionList()
    {
    }

    public ActionList(IList<Action> list)
        : base(list)
    {
    }

    public ActionList(IEnumerable<Action> list)
        : base(list)
    {
    }

    public ActionList(int capacity)
        : base(capacity)
    {
    }
}

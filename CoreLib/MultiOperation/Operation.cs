namespace Library.MultiOperation;

public sealed class Operation<TState>
{
    public Func<(TState State, int Index, int Count), TState> Action { get; internal set; }
    public int? Sequence { get; internal set; }
    public string? Description { get; internal set; }
    public Operation(Func<(TState State, int Index, int Count), TState> action, int? sequence = null, string? description = null) =>
        (this.Action, this.Sequence, this.Description) = (action, sequence, description);
}

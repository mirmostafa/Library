using Library.Validations;

namespace Library.MultiOperation;

public static class Operations
{
    public static OperationList<TState> New<TState>(TState state, string? name = null) =>
        new(state, name);

    public static OperationList<TState> AsSequenctial<TState>(this OperationList<TState> operations)
    {
        operations.ArgumentNotNull();
        var seq = operations.Max(x => x.Sequence) ?? 0;
        foreach (var item in operations)
        {
            if (item.Sequence is null)
            {
                item.Sequence = ++seq;
            }
        }
        return operations;
    }

    public static OperationList<TState> Build<TState>(this OperationList<TState> operations)
    {
        operations.ArgumentNotNull();
        operations.IsBuilt = true;
        return operations;
    }

    public static TState Run<TState>(this OperationList<TState> operations)
    {
        operations.ArgumentNotNull();
        Check.Require(operations.IsReadOnly, $"Please call .Build() method in {operations}", $"{operations} is not built yet.");

        operations.State = operations.DefaultState;
        var index = 0;
        var actions = operations.OrderBy(x => x.Sequence);
        foreach (var item in actions)
        {
            var buffer = operations.State;
            index++;
            operations.OnOperating((buffer, index, operations.Count));
            buffer = item.Action((buffer, index, operations.Count));
            operations.OnOperated((buffer, index, operations.Count));
            operations.State = buffer;
        }
        return operations.State;
    }

    public static OperationList<TState> Watch<TState>(this OperationList<TState> operations, Action<TState, int, int> display)
    {
        operations.ArgumentNotNull().Operated += (_, e) => display?.Invoke(e.Item.State, e.Item.Index, e.Item.Count);
        return operations;
    }
    public static OperationList<TState> WatchDoing<TState>(this OperationList<TState> operations, Action<TState, int, int> display)
    {
        operations.ArgumentNotNull().Operating += (_, e) => display?.Invoke(e.Item.State, e.Item.Index, e.Item.Count);
        return operations;
    }
    public static OperationList<TState> WatchDone<TState>(this OperationList<TState> operations, Action<TState, int, int> display)
    {
        operations.ArgumentNotNull().Operated += (_, e) => display?.Invoke(e.Item.State, e.Item.Index, e.Item.Count);
        return operations;
    }
}
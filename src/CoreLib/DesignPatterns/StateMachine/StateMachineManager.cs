using Library.Validations;

namespace Library.DesignPatterns.StateMachine;

public static class StateMachineManager
{
    public static async Task<(TState? Last, IEnumerable<(TState State, MoveDirection Direction)> History)> Dispatch<TState>(
        [DisallowNull] Func<Task<(TState State, MoveDirection Direction)>> start,
        [DisallowNull] Func<(TState? Current, IEnumerable<(TState State, MoveDirection Direction)> History), Task<(TState State, MoveDirection Direction)>> moveNext,
        [DisallowNull] Func<(TState? Current, IEnumerable<(TState State, MoveDirection Direction)> History), Task<(TState State, MoveDirection Direction)>> moveBack,
        Func<(TState? Current, IEnumerable<(TState State, MoveDirection Direction)> History), Task>? notify = null,
        Func<(TState? Current, IEnumerable<(TState State, MoveDirection Direction)> History), Task>? done = null,
        Stack<(TState State, MoveDirection Direction)>? history = null)
    {
        Check.IfArgumentNotNull(start);
        Check.IfArgumentNotNull(moveNext);
        Check.IfArgumentNotNull(moveBack);

        history ??= new();
        history.Clear();
        var proc = notify ?? (_ => Task.CompletedTask);
        var current = await start();
        return (await moveAsync(current), history);

        async Task<TState> moveAsync((TState State, MoveDirection Direction) flow)
        {
            if (flow.Direction == MoveDirection.Ended)
            {
                await (done?.Invoke((flow.State, history)) ?? Task.CompletedTask);
                return flow.State;
            }
            history.Push(flow);
            await proc((flow.State, history));
            switch (flow.Direction)
            {
                case MoveDirection.Foreword:
                    {
                        var current = await moveNext((flow.State, history));
                        return await moveAsync(current);
                    }

                case MoveDirection.Backword:
                    {
                        var current = await moveBack((flow.State, history));
                        return await moveAsync(current);
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(flow), flow.Direction, $"{nameof(flow.Direction)} is out of range.");
            }
        }
    }

    public static Task<(TState? Last, IEnumerable<(TState State, MoveDirection Direction)> History)> Dispatch<TState>([DisallowNull] this IAsyncStateMachine<TState> stateMachine)
        => Dispatch(
            stateMachine.ArgumentNotNull().StartAsync,
            stateMachine.MoveNextAsync,
            stateMachine.MoveBackAsync,
            stateMachine.NotifyStateAsync,
            stateMachine.DoneAsync
            );
}
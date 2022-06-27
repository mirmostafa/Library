using Library.Validations;

namespace Library.DesignPatterns;

public record FlowItem<TState>(TState? State, bool WasLast);

public interface IStateMachine<TState>
{
    FlowItem<TState> MoveNext(TState? current);

    FlowItem<TState> Start();
}

public static class StateMachineHelper
{
    public static FlowItem<TState>? MoveOverStateMachine<TState>([DisallowNull] Func<FlowItem<TState>> start, [DisallowNull] Func<TState?, FlowItem<TState>> moveNext, Action<FlowItem<TState>?>? process = null)
    {
        Check.IfArgumentNotNull(start);
        Check.IfArgumentNotNull(moveNext);

        var current = start();
        while (current is not null and { WasLast: false })
        {
            process?.Invoke(current);
            current = moveNext(current.State);
        }
        process?.Invoke(current);
        return current;
    }

    public static FlowItem<TState>? MoveOverStateMachine<TState>([DisallowNull] this IStateMachine<TState> stateMachine, Action<FlowItem<TState>?>? process = null)
        => MoveOverStateMachine(stateMachine.Start, stateMachine.MoveNext, process);
}
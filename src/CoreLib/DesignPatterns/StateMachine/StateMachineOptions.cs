namespace Library.DesignPatterns.StateMachine;

public class StateMachineOptions<TState>
{
    public StateMachineOptions(IAsyncStateMachine<TState> owner)
        => Owner = owner;

    public Stack<TState> History { get; } = new();
    public IAsyncStateMachine<TState> Owner { get; }
    public bool SupportMoveBack { get; set; }
}

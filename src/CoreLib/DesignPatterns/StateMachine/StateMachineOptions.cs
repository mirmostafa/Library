using Library.Extensions.Options;

namespace Library.DesignPatterns.StateMachine;

public sealed class StateMachineOptions<TState> : IOptions
{
    public StateMachineOptions(IAsyncStateMachine<TState> owner)
        => this.Owner = owner;

    public Stack<TState> History { get; } = new();

    public IAsyncStateMachine<TState> Owner { get; }

    public bool SupportMoveBack { get; set; }
}
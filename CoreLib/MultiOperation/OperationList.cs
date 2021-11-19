using Library.Collections;
using Library.DesignPatterns.Markers;
using Library.EventsArgs;

namespace Library.MultiOperation;

[Fluent]
public sealed class OperationList<TState> : FluentListBase<Operation<TState>, OperationList<TState>>
{
    public TState DefaultState { get; }
    public TState State { get; internal set; }
    public string? Name { get; init; }
    public bool IsBuilt { get => this.IsReadOnly; internal set => this.IsReadOnly = value; }
    public event EventHandler<ItemActingEventArgs<(TState State, int Index, int Count)>> Operating;
    public event EventHandler<ItemActedEventArgs<(TState State, int Index, int Count)>> Operated;

    internal void OnOperating((TState State, int Index, int Count) step) => this.Operating?.Invoke(this, new(step));
    internal void OnOperated((TState State, int Index, int Count) step) => this.Operated?.Invoke(this, new(step));

    public OperationList(TState state, string? name) =>
        (this.DefaultState, this.Name) = (state, name);


    public OperationList<TState> Add(Action action, int? sequence = null, string? description = null) =>
        this.Add(new Operation<TState>(x =>
        {
            action?.Invoke();
            return x.State;
        }, sequence, description));

    public OperationList<TState> Add(Action<TState> action, int? sequence = null, string? description = null) =>
        this.Add(new Operation<TState>(x =>
        {
            var result = x.State;
            action?.Invoke(result);
            return result;
        }, sequence, description));

    public OperationList<TState> Add(Action<TState, int> action, int? sequence = null, string? description = null) =>
        this.Add(new Operation<TState>(x =>
        {
            var result = x.State;
            action?.Invoke(result, sequence ?? default);
            return result;
        }, sequence, description));

    public OperationList<TState> Add(Func<TState, TState> action, int? sequence = null, string? description = null) =>
        this.Add(new Operation<TState>(x =>
        {
            var result = x.State;
            result = action(result);
            return result;
        }, sequence, description));

    public OperationList<TState> Add(Action<TState, int, int> action, int? sequence = null, string? description = null) =>
        this.Add(new Operation<TState>(x =>
        {
            var result = x.State;
            action?.Invoke(result, x.Index, x.Count);
            return result;
        }, sequence, description));

    public OperationList<TState> Add(Func<TState, int, TState> action, int? sequence = null, string? description = null) =>
        this.Add(new Operation<TState>(x =>
        {
            var result = x.State;
            result = action(result, x.Index);
            return result;
        }, sequence, description));

    public OperationList<TState> Add(Func<TState, int, int, TState> action, int? sequence = null, string? description = null) =>
        this.Add(new Operation<TState>(x =>
        {
            var result = x.State;
            result = action(result, x.Index, x.Count);
            return result;
        }, sequence, description));

    public override string? ToString() =>
        this.Name ?? base.ToString();
}

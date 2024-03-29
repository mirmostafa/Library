﻿using System.Diagnostics;

using Library.EventsArgs;

namespace Library.Threading.MultistepProgress;

public interface IProgressReport
{
    event EventHandler<ItemActedEventArgs<ProgressData?>>? Ended;

    event EventHandler<ItemActedEventArgs<ProgressData>>? Reported;

    static IProgressReport New() =>
        new ProgressReport();

    void End(in ProgressData? description = null);

    [DebuggerStepThrough]
    void Report(in ProgressData description);

    [DebuggerStepThrough]
    void Report(int max, int current, in string? description, in object? sender = null) =>
        this.Report(new(max, current, description, sender));
}

internal sealed class ProgressReport : IProgressReport
{
    public event EventHandler<ItemActedEventArgs<ProgressData?>>? Ended;

    public event EventHandler<ItemActedEventArgs<ProgressData>>? Reported;

    public void End(in ProgressData? progress) =>
        this.Ended?.Invoke(this, new(progress));

    [DebuggerStepThrough]
    public void Report(in ProgressData progress)
        => this.Reported?.Invoke(this, new(progress));
}

public record struct StepInfo<TState>(
    in Func<(TState State, IProgressReport SubProgress, CancellationToken cancellationToken), Task<TState>> AsyncAction,
    in string? Description,
    in int ProgressCount);

public readonly record struct ProgressData(in int? Max = null, in int? Current = null, in string? Description = null, object? Sender = null)
{
    public static implicit operator (string? Description, int? Max, int? Current)(in ProgressData value)
        => (value.Description, value.Max, value.Current);
    public static implicit operator ProgressData(in (int? Max, int? Current, string? Description) value)
        => new(value.Max, value.Current, value.Description);
    public static implicit operator ProgressData(in string description)
        => new(default, default, description);

    [return: NotNullIfNotNull(nameof(data.Description))]
    public static implicit operator string?(in ProgressData data)
        => data.ToString();

    public void Deconstruct(out string? description, out int? max, out int? current)
        => (description, max, current) = (this.Description, this.Max, this.Current);

    public override string? ToString()
        => this.Description;
}
using Library.EventsArgs;

namespace Library.Threading.MultistepProgress;

public interface IMultistepProcess
{
    event EventHandler<ItemActedEventArgs<string?>>? Ended;

    event EventHandler<ItemActedEventArgs<(int Max, int Current, string? Description)>>? Reported;

    static IMultistepProcess New()
        => new MultistepProcess();

    void End(in string? description = null);

    void Report(in string? description = null, in int max = 0, in int current = 0);
}

internal class MultistepProcess : IMultistepProcess
{
    public event EventHandler<ItemActedEventArgs<string?>>? Ended;

    public event EventHandler<ItemActedEventArgs<(int Max, int Current, string? Description)>>? Reported;

    public MultistepProcess()
    {
    }

    public void End(in string? description)
        => this.Ended?.Invoke(this, new(description));

    public void Report(in string? description = null, in int max = -1, in int current = -1)
        => this.Reported?.Invoke(this, new((max, current, description)));
}

public record struct StepInfo<TState>(in Func<(TState State, IMultistepProcess SubProgress), Task<TState>> AsyncAction, in string? Description, in int ProgressCount);
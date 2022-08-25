using Library.Collections;
using Library.Helpers;
using Library.ProgressiveOperations;

internal class Program
{
    private static async Task Main(string[] args)
    {
        FluentList<StepInfo<object?, string>> steps = new();
        StepInfo<object?, string> step = new(
            state =>
            {
                Thread.Sleep(50);
                return Task.FromResult(state);
            }, "Working...", 1);
        _ = steps.AddRange(ObjectHelper.Repeat(step, 200));
        Write("Hi. ");
        Library.Helpers.ConsoleHelper.ConsoleProgressBar bar = new(50);
        var manager = new MultistepProgressManager<object?>(null, steps, IMultistepProgress.GetNew(e =>
        {
            var o = e.Operation;
            var description = $"{o.Current} of {o.Max} - {e.Value}";
            //WriteLine(description);
            bar.Report(o.Current + 1, o.Max);
        }));
        _ = await manager.StartAsync();
    }
}
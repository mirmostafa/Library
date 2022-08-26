using Library.Helpers;
using Library.ProgressiveOperations;

internal class Program
{
    private static void Main()
    {
    }

    private static async Task MultistepProgressFullTest()
    {
        var simulationAction = ((int State, IMultistepProgress SubProgress) e) =>
        {
            Thread.Sleep(500);
            return Task.FromResult(e.State);
        };
        var step = new StepInfo<int>(async e =>
        {
            await doSubs(e, simulationAction);
            return e.State;
        }, "Working...", 5);
        var steps = new List<StepInfo<int>>(ObjectHelper.Repeat(step, 10));
        var mainReport = ((string Description, int Max, int Current) e) =>
        {
            WriteLine();
            displayReport(e);
            WriteLine();
        };
        var subReport = ((string Description, int Max, int Current) e) =>
        {
            CursorLeft = 0;
            displayReport(e);
        };
        var manager = new MultistepProgressManager<int>(0, steps, mainReport, subReport);
        _ = await manager.StartAsync();

        return;

        static void displayReport((string Description, int Max, int Current) e)
            => Write($"{e.Current:00} of {e.Max:00} - {e.Description}");

        static async Task doSubs((int State, IMultistepProgress SubProgress) e, Func<(int State, IMultistepProgress SubProgress), Task<int>> simulationAction)
            => await MultistepProgressManager<int>.StartAsync(e.State, new StepInfo<int>[] { new(simulationAction, "Loading...", 5), new(simulationAction, "Processing...", 3), new(simulationAction, "Saving...", 10) }, e.SubProgress);
    }
}
using Library.Helpers;
using Library.ProgressiveOperations;

internal class Program
{
    private static async Task Main(string[] args)
    {
        List<StepInfo<int>> steps = new();
        StepInfo<int> step = new(
            e =>
            {
                for (var i = 0; i <= 10; i++)
                {
                    Thread.Sleep(100);
                    e.SubProgress.Report("Sub", 10, e.State++);
                }

                return Task.FromResult(e.State);
            }, "Working...", 5);
        steps.AddRange(ObjectHelper.Repeat(step, 5));

        var mainReport = ((string Description, int Max, int Current) e) =>
        {
            WriteLine();
            displayReport(e);
        };
        var subReport = ((string Description, int Max, int Current) e) =>
        {
            CursorLeft = 0;
            displayReport(e);
        };
        var manager = new MultistepProgressManager<int>(0, steps, mainReport);//, subReport);
        _ = await manager.StartAsync();

        static void displayReport((string Description, int Max, int Current) e) => WriteLine($"{e.Current:00} of {e.Max:00} - {e.Description}");
    }
}
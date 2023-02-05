using Library.Coding;
using Library.Logging;

namespace UnitTests;


[Trait("Category", "Helpers")]
[Trait("Category", "Code Helpers")]
public class CodeHelperTest
{
    [Fact]
    public void CodeBlockLoggerTest()
    {
        var number = 5;
        VsOutputLogger logger = new();
        using (CodeBlockLogger.New(logger, "~~~~Get add 6 to a number…", "~~~~~Got."))
        {
            number += 6;
        }
    }

    [Fact]
    public async void LibStopwatchTest1()
    {
        var stopwatch = new LibStopwatch();
        using (stopwatch.Start())
        {
            await Task.Delay(1000);
        }
        var elapsed = stopwatch.Elapsed;
        Assert.True(elapsed.Ticks > 0);
    }

    [Fact]
    public async Task LibStopwatchTest2Async()
    {
        var stopwatch = LibStopwatch.StartNew();
        await Task.Delay(1000);
        _ = stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;
        Assert.True(elapsed.Ticks > 0);
    }
}
using Library.Coding;
using Library.Logging;

namespace UnitTest;

[TestClass]
public class CodeHelperTest
{
    [TestMethod]
    public void CodeBlockLoggerTest()
    {
        var number = 5;
        VsOutputLogger logger = new();
        using (CodeBlockLogger.New(logger, "~~~~Get add 6 to a number…", "~~~~~Got."))
        {
            number += 6;
        }
    }

    [TestMethod]
    public async void LibStopwatchTest()
    {
        var stopwatch = new LibStopwatch();
        using (stopwatch.Start())
        {
            await Task.Delay(1000);
        }
        var elapsed = stopwatch.Elapsed;
        Assert.IsTrue(elapsed.Ticks > 0);
    }
}
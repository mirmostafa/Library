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
    public void LibStopwatchTest1()
    {
        var stopwatch = new LibStopwatch();
        using (stopwatch.Start())
        {
            Task.Delay(1000).Wait();
        }
        var elapsed = stopwatch.Elapsed;
        Assert.IsTrue(elapsed.Ticks > 0);
    }

    [TestMethod]
    public void LibStopwatchTest2()
    {
        var stopwatch = LibStopwatch.StartNew();
        Task.Delay(1000).Wait();
        _ = stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;
        Assert.IsTrue(elapsed.Ticks > 0);
    }
}
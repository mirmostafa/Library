using Library.Threading;
using System.Diagnostics;

namespace LibraryTest;

[TestClass]
public class BackgroundTimerTest
{
    [TestMethod]
    public void Test1()
    {
        using var t = BackgroundTimer
                        .Start(() => Console.WriteLine("Test2"), 420, "My Task")
                        .Sleep(TimeSpan.FromSeconds(5))
                        .StopAsync();
    }

    [TestMethod]
    public void Test2()
    {
        using var t = BackgroundTimer
                        .New(() =>
                        {
                            var zero = 0;
                            var a = 5 / zero;
                        }, 420)
                        .OnError(ex => Debug.WriteLine(ex))
                        .Start()
                        .Sleep(TimeSpan.FromSeconds(5))
                        .StopAsync();
    }
}

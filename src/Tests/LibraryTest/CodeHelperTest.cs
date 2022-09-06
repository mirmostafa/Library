using Library.Exceptions;
using Library.Results;

namespace UnitTest;

[TestClass]
public class FunctionalTests
{
    [TestMethod]
    [ExpectedException(typeof(BreakException))]
    public void BreakTest()
    {
        Break();
    }

    [TestMethod]
    public void ResultCompositionTest()
    {
        var start = () => Result<int>.CreateSuccess(5);
        var step = (int x) => Result<int>.CreateSuccess(x + 1);
        var err = (int x) => Result<int>.CreateFail(value: x);
        var fail = (Result<int> x) => x;

        var actual = start.Compose(step, fail).Compose(step, fail).Compose(err, fail).Compose(step, fail).Compose(step, fail)();
        Assert.AreEqual(7, actual);
    }

    [TestMethod]
    public void SimeCompositionTest()
    {
        var start = () => 5;
        var add = (int x) => x + 5;
        var sub = (int x) => x - 5;
        var log = (string x) => Console.WriteLine(x);
        var log1 = (string message) => log(message);
        var log2 = (int x, string message) => log(message);
        var log3 = ((int X, string Message) x) => log($"Step 1 - {x}");
        var log4 = ((int X, int Step) x) => log($"Step {x.Step} - {x.X}");

        var starting = start.Compose(log2, "Start");
        var adding1 = starting.Compose(add).Compose(log1, x => $"Step 1 - {x}");
        var adding2 = adding1.Compose(add).Compose(log2, x => $"Step 2 - {x}");
        var ended = adding2.Compose(sub).Compose(log4, x => (x, 3));
        var result = ended();

        Assert.AreEqual(10, result);
    }
}
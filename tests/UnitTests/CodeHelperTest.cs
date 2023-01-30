using Library.Exceptions;
using Library.Results;

namespace UnitTests;


public class FunctionalTests
{
    [Fact]
    public void AddCompositionTest1()
    {
        var start = () => 5;
        var add = (int x) => x + 5;
        var sub = (int x) => x - 5;

        var starting = start;
        var adding = starting.Compose(add);
        var ended = adding.Compose(sub);
        var result = ended();

        Assert.Equal(5, result);
    }

    [Fact]
    public void AddCompositionTest2()
    {
        var add = (int x) => (int y) => x + y;
        var result = add(3)(4);
        Assert.Equal(7, result);

        var add3 = (int x) => (int y) => (int z) => x + y + z;
        result = add3(3)(4)(5);
        Assert.Equal(12, result);
    }

    [Fact]
    public void AddCompositionTest3()
    {
        var three = () => 3;
        var four = () => 4;
        var add = (Func<int> x) => (Func<int> y) => x() + y();
        var result = add(three)(four);

        Assert.Equal(7, result);
    }

    [Fact]
    public void BreakTest()
        => Assert.Throws<BreakException>(Break);

    [Fact]
    public void ResultCompositionTest()
    {
        var start = () => Result<int>.CreateSuccess(5);
        var step = (int x) => Result<int>.CreateSuccess(x + 1);
        var err = (int x) => Result<int>.CreateFail(value: x);
        var fail = (Result<int> x) => x;

        var actual = start.Compose(step, fail).Compose(step, fail).Compose(err, fail).Compose(step, fail).Compose(step, fail)();
        Assert.Equal(7, actual);
    }

    [Fact]
    public void SimpleCompositionTest1()
    {
        var five = () => 5;
        var addFive = (int x) => x + 5;
        var subFive = (int x) => x - 5;

        var starting = five;
        var adding = starting.Compose(addFive).Compose(addFive);
        var ended = adding.Compose(subFive);
        var result = ended();

        Assert.Equal(10, result);
    }

    [Fact]
    public void ComplexCompositionTest2()
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

        Assert.Equal(10, result);
    }
}
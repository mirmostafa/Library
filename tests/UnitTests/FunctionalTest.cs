using Library.Coding;
using Library.Exceptions;
using Library.Results;

namespace UnitTests;

[Trait("Category", "Code Helpers")]
public sealed class FunctionalTest
{
    private static readonly Action<int> _emptyIntAction = x => { };
    private static readonly Func<int> _returnsTwo = () => 2;
    private record EmptyRecord();

    [Fact]
    public void BreakTest()
        => Assert.Throws<BreakException>(Break);

    [Fact]
    public void Composition_AddTest1()
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
    public void Composition_AddTest2()
    {
        var add = (int x) => (int y) => x + y;
        var result = add(3)(4);
        Assert.Equal(7, result);
    }

    [Fact]
    public void Composition_AddTest3()
    {
        var add3 = (int x) => (int y) => (int z) => x + y + z;
        var result = add3(3)(4)(5);
        Assert.Equal(12, result);
    }

    [Fact]
    public void Composition_AddTest4()
    {
        var three = () => 3;
        var four = () => 4;
        var add = (Func<int> x) => (Func<int> y) => x() + y();
        var result = add(three)(four);

        Assert.Equal(7, result);
    }

    [Fact]
    public void Composition_ComplexTest2()
    {
        var start = () => 5;
        var add = (int x) => x + 5;
        var sub = (int x) => x - 5;
        var log = (string x) => Console.WriteLine(x);
        var log1 = (string message) => log(message);
        var log2 = (int x, string message) => log(message);
        var log3 = ((int X, int Step) x) => log($"Step {x.Step} - {x.X}");

        var starting = start.Compose(log2, "Start");
        var adding1 = starting.Compose(add).Compose(log1, x => $"Step 1 - {x}");
        var adding2 = adding1.Compose(add).Compose(log2, x => $"Step 2 - {x}");
        var ended = adding2.Compose(sub).Compose(log3, x => (x, 3));
        var result = ended();

        Assert.Equal(10, result);
    }

    [Fact]
    public void Composition_ResultTest()
    {
        var start = () => Result<int>.CreateSuccess(5);
        var step = (int x) => Result<int>.CreateSuccess(x + 1);
        var err = (int x) => Result<int>.CreateFailure(value: x);
        var fail = (Result<int> x) => x;

        var actual = start.Compose(step, fail).Compose(step, fail).Compose(err, fail).Compose(step, fail).Compose(step, fail)();
        Assert.Equal(7, actual);
    }

    [Fact]
    public void Composition_SimpleTest1()
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
    public void FluentByResultTest()
    {
        var actualTwo = 2.Fluent().Result(_returnsTwo);
        Assert.Equal((2, 2), actualTwo);
    }

    [Fact]
    public void FluentByResultTest2()
    {
        var actualThree = 3.Fluent().Result(_returnsTwo);
        Assert.Equal((3, 2), actualThree);
    }

    [Fact]
    public void FluentTest()
    {
        _ = this.Fluent(Methods.Empty);
        var two = 2.Fluent(() => { });
        Assert.Equal(2, two.Value);
    }

    [Fact]
    public void ForEachTest() 
        => Enumerable.Range(1, 1).ForEach(_emptyIntAction);

    [Fact]
    public void IfConditionTest()
    {
        var booleanTest = true;
        var trueResult = booleanTest.IfTrue(() => "true");
        Assert.Equal("true", trueResult);
    }

    [Fact]
    public void IfConditionTest2()
    {
        var booleanTest = false;
        var falseResult = booleanTest.IfFalse(() => "false");
        _ = booleanTest.IfFalse(Methods.Empty);

        Assert.Equal("false", falseResult);
    }

    [Fact]
    public void ListTest()
    {
        var list = List(5, 6, 7);
        _ = Assert.IsAssignableFrom<List<int>>(list);
        Assert.Equal(3, list.Count);
        Assert.Equal(6, list[1]);
        Assert.Equal(7, list[2]);
    }

    [Fact]
    public void LockTest() 
        => Lock(this, Methods.Empty);

    [Fact]
    public void NewTest() 
        => New<EmptyRecord>();

    [Fact]
    public void WhileFuncTest()
        => _ = While(() => false, _returnsTwo);

    [Fact]
    public void WhileTest()
        => While(() => false);
}
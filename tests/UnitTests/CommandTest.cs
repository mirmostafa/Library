using Library.Interfaces;

namespace UnitTests;

[Trait("Category", nameof(Library.DesignPatterns))]
public sealed class CommandTest
{
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(10, 25, 35)]
    public void AddTest(int x, int y, int expected)
    {
        var actual = Calculator.Add(x, y);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Div_DivByZeroTest()
        => Assert.Throws<DivideByZeroException>(() => Calculator.Div(10, 0));

    [Theory]
    [InlineData(1, 2, 0)]
    [InlineData(10, 25, 0)]
    public void DivTest(int x, int y, int expected)
    {
        var actual = Calculator.Div(x, y);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(10, 25, 250)]
    public void MulTest(int x, int y, int expected)
    {
        var actual = Calculator.Mul(x, y);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 2, -1)]
    [InlineData(10, 25, -15)]
    public void SubTest(int x, int y, int expected)
    {
        var actual = Calculator.Sub(x, y);
        Assert.Equal(expected, actual);
    }
}

readonly file record struct Adder(int X, int Y) : ICommand<int>
{
    public int Execute() => this.X + this.Y;
}
readonly file record struct Subtractor(int X, int Y) : ICommand<int>
{
    public int Execute() => this.X - this.Y;
}
readonly file record struct Multiplier(int X, int Y) : ICommand<int>
{
    public int Execute() => this.X * this.Y;
}
readonly file record struct Divider(int X, int Y) : ICommand<int>
{
    public int Execute() => this.X / this.Y;
}

internal static class Calculator
{
    public static int Add(int x, int y)
        => new Adder(x, y).Execute();

    public static int Div(int x, int y)
        => new Divider(x, y).Execute();

    public static int Mul(int x, int y)
        => new Multiplier(x, y).Execute();

    public static int Sub(int x, int y)
        => new Subtractor(x, y).Execute();
}
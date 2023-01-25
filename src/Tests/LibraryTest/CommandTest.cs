using Library.Interfaces;

namespace Library.UnitTest;

[TestClass]
public class CommandTest
{
    public record Adder(int X, int Y) : ICommand<int>
    {
        public int Execute() => this.X + this.Y;
    }
    public record Subtractor(int X, int Y) : ICommand<int>
    {
        public int Execute() => this.X - this.Y;
    }

    public record Multiplier(int X, int Y) : ICommand<int>
    {
        public int Execute() => this.X * this.Y;
    }
    public record Divider(int X, int Y) : ICommand<int>
    {
        public int Execute() => this.X / this.Y;
    }

    public static class Calculator
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
}
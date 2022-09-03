using Library.Coding;

internal class Program
{
    private static void Main()
    {
        var start = () => 5;
        var add = (int x) => x + 5;
        var log1 = (string message) => WriteLine(message);
        var log2 = (int x, string message) => WriteLine(message);
        var log3 = ((int X, string Message) x) => WriteLine($"Step 1 - {x}");
        var log4 = ((int X, int Step) x) => WriteLine($"Step {x.Step} - {x.X}");

        var func1 = start.Compose(log2, "Start")
                   .Compose(add).Compose(log1, x => $"Step 1 - {x}");
        var func2 = func1.Compose(add).Compose(log2, x => $"Step 2 - {x}")
                         .Compose(add).Compose(log4, x => (x, 3));
        var result = func2();
        WriteLine(result);
    }
}
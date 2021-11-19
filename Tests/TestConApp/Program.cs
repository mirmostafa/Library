using System;
using Library.MultiOperation;

namespace TestConApp;

internal partial class Program
{
    private static void Main()
    {
        var display = (int state, int index, int count) => Console.WriteLine($"state: {state}, index: {index}, count: {count}");
        var result = Operations.New(state: 5)
                               //.Add(display)
                               .Add((state) => 10)
                               //.Add(display)
                               .Add((state, index) => 20)
                               //.Add(display)
                               .Add((state, index, count) => 30)
                               //.Add(display)
                               .AsSequenctial()
                               .Build()
                               .Watch(display)
                               .Run();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Coding;
using Library.MultiOperation;

namespace TestConApp;

internal partial class Program
{
    private static void Main(params string[] args)
    {
        //var func1 = (int x) => x - 1;
        //var starter = () => 5;
        //var compose = starter.Compose(func1).Compose(func1).Compose(func1);
        //var result = compose();

        var starter = () => "This is a long long sentence.";
        var compose = starter.Compose(s => $"{s} Let's add")
                             .Compose(s => $"{s} some other")
                             .Compose(s => $"{s} statement")
                             .Compose(s => $"{s} to make sure")
                             .Compose(s => $"{s} the compose method")
                             .Compose(s => $"{s} is working")
                             .Compose(s => $"{s} like a charm.");
        var result = compose();
        Console.WriteLine(result);
        //OperationsTest();
        //HigherOrder();
    }

    private static void HigherOrder()
    {
        var sum = ((int? LastResult, int Current) row) => row.Current + row.LastResult ?? 0;
        var powerOf = (int x) => (int a) => a ^ x;
        var multiply = (int x) => (int a) => a * x;
        var transform = (IEnumerable<int> source, Func<int, int> transformer) => source.Select(transformer);
        var aggregate = (IEnumerable<int> source, Func<(int? LastResult, int Current), int> aggregator) =>
        {
            int? last = null;
            foreach (var item in source)
            {
                last = aggregator((last, item));
            }
            return last;
        };

        var data = Enumerable.Range(0, 1001);

        var powerOfThrees = transform(data, powerOf(3));
        var multiple5s = transform(data, multiply(5));
        var total = aggregate(data, sum); //! Not higher-ordered
    }

    private static void OperationsTest()
    {
        var display = (int state, int index, int count) => Console.WriteLine($"state: {state}, index: {index}, count: {count}");
        var result = Operations.New(5)
                               .Add((state) => 10)
                               .Add((state, index) => 20)
                               .Add((state, index, count) => 30)
                               //.AsSequenctial()
                               .Build()
                               .Watch(display)
                               .Run();
        var r = Operations.New(Task.FromResult(1))
                          .Add(state => Task.FromResult(2))
                          .Add(state => Task.FromResult(3))
                          .Add(state => Task.FromResult(4))
                          .Add(state => Task.FromResult(5))
                          .Build()
                          .Watch((state, index, count) => Console.WriteLine(state.Id))
                          .Run();
        r.Wait();
    }
}

public static class TestExtensions
{

}
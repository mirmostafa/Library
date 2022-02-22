using Library.Coding;
using Library.Helpers;
using Library.MultiOperation;
using Library.Web;

namespace TestConApp;

internal partial class Program
{
    private static async Task Main(params string[] args)
    {
        _logger.Debug("Starting");
        await WebApiCreator.New("http://localhost:1234")
                           .MapGet("/", () => "Hi")
                           .MapGet("/bye", () => "BYE")
                           .MapGet("/about", () => WebApiHelper.ToHtml("<center>Written by <h1>Mohammad Mirmostafa</h1></center>"))
                           .RunAsync();
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

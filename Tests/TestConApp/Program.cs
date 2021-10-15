using Library.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Immutable;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TestConApp;

internal partial class Program
{
    private static ILogger _logger = null!;

    public static async Task Main()
    {
        var a = ImmutableList<string>.Empty;
        a = a.Add("Hi");
        return;
        using var t = BackgroundTimer
                        .Start(() => Console.WriteLine("Hi"), 420, "My Task")
                        .Sleep(TimeSpan.FromSeconds(5))
                        .StopAsync();
        Console.ReadKey();

        var cts = new CancellationTokenSource();
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://localhost:5001/slow", cts.Token)
                                       .WaitAsync(TimeSpan.FromMilliseconds(4200), cts.Token);
    }
}
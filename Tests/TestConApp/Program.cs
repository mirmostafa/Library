using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Library;
using Library.Helpers;
using Library.Helpers.ConsoleHelper;
using Microsoft.Extensions.Logging;

namespace TestConApp;

internal class Program
{
    private static ILogger _logger;
    public static void Main()
    {

    }

    private static void ProgressBarTest()
    {
        Console.Write("Doing... ");
        using (var progress = new ConsoleProgressBar())
        {
            const int max = 200;
            for (var i = 0; i <= max; i++)
            {
                progress.Report((double)i / max);
                Thread.Sleep(20);
            }
        }
        Console.WriteLine("Done.");
    }

    public static void Test<T>([DisallowNull] T t)
        where T : notnull => t.IfArgumentNotNull(nameof(t));

    private static void SourceGeneratorTest()
    {
        var dto = new Models.ProductDto { };

        var vm = new AutoNotifyTestModel();
        var text = vm.Text;
        _logger.LogInformation($"Text = {text}");
        var count = vm.Count;
        _logger.LogInformation($"Count = {count}");
        vm.PropertyChanged += (o, e) => _logger.LogInformation($"Property {e.PropertyName} was changed");
        vm.Text = "abc";
        vm.Count = 123;
    }

    [ModuleInitializer]
    public static void Startup()
    {
        setupLogger();
        LibLogger.AddLogger(_logger);
        _logger.LogInformation("System initialized.");
        
        static void setupLogger()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.TimestampFormat = "HH:mm:ss";
                    options.UseUtcTimestamp = false;
                });
            });
            var logger = loggerFactory.CreateLogger<Program>();
            using (logger.BeginScope("[Scope is enabled]"))
            {
                //logger.LogInformation("System is started");
            }
            _logger = logger;
        }
    }
}
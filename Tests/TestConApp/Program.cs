using Library;
using Microsoft.Extensions.Logging;

namespace TestConApp;

internal class Program
{
    public static void Main()
    {
        var logger = SetupLogger();
        logger.LogInformation("System is started");
        LibLogger.AddLogger(logger);
        var dto = new Models.ProductDto { };

        var vm = new AutoNotifyTestModel();
        var text = vm.Text;
        logger.LogInformation($"Text = {text}");
        var count = vm.Count;
        logger.LogInformation($"Count = {count}");
        vm.PropertyChanged += (o, e) => logger.LogInformation($"Property {e.PropertyName} was changed");
        vm.Text = "abc";
        vm.Count = 123;
    }

    private static ILogger<Program> SetupLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.TimestampFormat = "HH:mm:ss";
                options.UseUtcTimestamp = false;
                //options.SingleLine = true;
            });
        });
        var logger = loggerFactory.CreateLogger<Program>();
        using (logger.BeginScope("[Scope is enabled]"))
        {
            //logger.LogInformation("System is started");
        }
        return logger;
    }
}
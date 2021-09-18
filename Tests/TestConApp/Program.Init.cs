using System.Runtime.CompilerServices;
using Library;
using Microsoft.Extensions.Logging;

namespace TestConApp;

internal partial class Program
{
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
            _logger = logger;
        }
    }
}

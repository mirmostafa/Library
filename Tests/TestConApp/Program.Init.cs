using System.Runtime.CompilerServices;
using Library;
using Library.Logging;
using Microsoft.Extensions.Logging;

namespace TestConApp;

internal partial class Program
{
    private static Microsoft.Extensions.Logging.ILogger _logger = null!;

    [ModuleInitializer]
    public static void Startup()
    {
        setupLogger();
        LibLogger.AddLogger(_logger);
        _logger.Info("System initialized.");

        static void setupLogger()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(options =>
                {
                    //! Not fancy
                    //x options.IncludeScopes = true;
                    //x options.TimestampFormat = "HH:mm:ss";
                    options.UseUtcTimestamp = false;
                });
            });
            var logger = loggerFactory.CreateLogger<Program>();
            _logger = logger;
        }
    }
}

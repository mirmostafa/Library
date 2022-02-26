using System.Runtime.CompilerServices;
using Library;
using Library.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace TestConApp;

internal class ProgramBase
{
    protected static WebApplication WebApp { get; private set; }
    protected static IMsLoggerMessageWapper Logger { get; private set; } = null!;

    [ModuleInitializer]
    public static void Startup()
    {
        var builder = WebApplication.CreateBuilder();
        setupLogger(builder);
        WebApp = builder.Build();
        Logger.Info("System initialized.");

        static void setupLogger(WebApplicationBuilder builder)
        {
            builder.Logging.AddSimpleConsole(options =>
            {
                options.UseUtcTimestamp = false;
            });
            Logger = new MsLoggerMessageWapper(WebApp.Logger).Configure(options => { });
            LibLogger.AddLogger(WebApp.Logger);
        }
    }
}

internal partial class Program : ProgramBase
{
}

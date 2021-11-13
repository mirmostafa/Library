using System.Diagnostics.CodeAnalysis;
using Library.Exceptions;
using Library.Logging;
using Library.Mapping;
using Library.Wpf.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Wpf.Windows;

public abstract class LibApp : Application
{
    protected LibApp()
    {
        SetupServices();
        SetupLogger();
        AppLogger.Debug("Application constructed.");
        return;

        void SetupServices()
        {
            var services = new ServiceCollection();
            this.ConfigureServices(services);
            var result = services.BuildServiceProvider();
            DI.Initialize(result);
        }

        void SetupLogger()
        {
            this.Logger = new FastLogger();
            LibLogger.AddLogger(this.Logger);
        }
    }

    public static string? ApplicationTitle => ApplicationHelper.ApplicationTitle ?? Current?.MainWindow?.Title;

    public static FastLogger AppLogger => Current.As<LibApp>()!.Logger;

    public FastLogger Logger { get; private set; }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string? Title => ApplicationTitle;

    protected virtual void HandleException(Exception exception)
    {
        switch (exception)
        {
            case BreakException:
                break;
            case OperationCancelledException ex:
                this.Logger.Warn(ex.Instruction ?? ex.Message);
                _ = MsgBox2.Error(ex.Instruction ?? string.Empty, ex.Message ?? "Operation cancelled by user.", ex.Owner?.ToString() ?? title());
                break;
            case IException ex:
                this.Logger.Warn(ex.Instruction ?? ex.Message);
                _ = MsgBox2.Error(ex.Instruction ?? string.Empty, ex.Message, ex.Title ?? ex.Owner?.ToString() ?? title(), detailsExpandedText: ex.Details);
                break;
            case Exception ex:
                this.Logger.Log(ex.GetFullMessage(), LogLevel.Fatal, stackTrace: ex.StackTrace);
                _ = MsgBox2.Exception(ex, caption: title());
                break;
            default:
                break;
        }

        string title()
            => exception.As<IException>()?.Title ?? ApplicationTitle ?? exception.GetType().Name;
    }

    protected virtual void OnConfigureServices(ServiceCollection services)
    {
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        Current.Dispatcher.UnhandledException += (_, e) =>
        {
            this.HandleException(e.Exception);
            e.Handled = true;
        };
        base.OnStartup(e);
    }

    private void ConfigureServices(ServiceCollection services)
    {
        AddDefaultServices(services);
        this.OnConfigureServices(services);

        static void AddDefaultServices(ServiceCollection services)
            => _ = services.AddScoped<IMapper, Mapper>()
                           .AddScoped<ILogger>(_ => AppLogger)
                           .AddScoped<IEventualLogger>(_ => AppLogger);
    }
}

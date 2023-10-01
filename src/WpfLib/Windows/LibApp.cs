using System.Diagnostics.CodeAnalysis;

using Library.Exceptions;
using Library.Exceptions.Validations;
using Library.Mapping;
using Library.Wpf.Dialogs;

using Microsoft.Extensions.DependencyInjection;

namespace Library.Wpf.Windows;

public abstract class LibApp : Bases.ApplicationBase
{
    private static FastLogger? _logger;

    protected LibApp()
    {
        this.Logger = new FastLogger();
        LibLogger.AddLogger(this.Logger);

        var services = new ServiceCollection();
        this.ConfigureServices(services);
        var result = services.BuildServiceProvider();
        DI.Initialize(result);

        AppLogger.Debug("Application constructed.");
    }

    public static string? ApplicationTitle => ApplicationHelper.ApplicationTitle ?? Current?.MainWindow?.Title;

    public static FastLogger AppLogger => _logger ??= Current.Cast().To<LibApp>().Logger;

    public FastLogger Logger { get; private set; }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string? Title => ApplicationTitle;

    protected virtual void HandleException(Exception exception)
    {
        var title = exception.Cast().As<IException>()?.Title ?? ApplicationTitle ?? exception?.GetType().Name;
        string? owner(IException ex) => ex.Title ?? ex.Owner?.ToString() ?? title;
        switch (exception)
        {
            case BreakException:
                break;

            case IValidationException ex:
                _ = MsgBox2.Error(ex.Instruction ?? string.Empty, ex.Message, owner(ex), detailsExpandedText: ex.Details);
                break;

            case OperationCancelledException ex:
                this.Logger.Warn(ex.Instruction ?? ex.Message, sender: owner(ex));
                _ = MsgBox2.Error(ex.Instruction ?? string.Empty, ex.Message ?? "Operation canceled by user.", owner(ex));
                break;

            case System.Data.SqlClient.SqlException ex when ex?.Message?.Contains("A transport-level error has occurred when receiving results from the server") ?? false:
                this.Logger.Fatal(ex.GetFullMessage(), sender: ex.Source ?? title);
                _ = MsgBox2.Error("Error on connecting to database", text: ex.Message, detailsExpandedText: "Make sure SQL Server service is up and function, and the database exists.");
                break;

            case IException ex:
                this.Logger.Error(ex.Instruction ?? ex.Message, sender: owner(ex));
                _ = MsgBox2.Error(ex.Instruction ?? string.Empty, ex.Message, owner(ex), detailsExpandedText: ex.Details);
                break;

            case Exception ex:
                this.Logger.Log(ex.GetFullMessage(), LogLevel.Fatal, stackTrace: ex.StackTrace, sender: ex.Source ?? title);
                _ = MsgBox2.Exception(ex, caption: title);
                break;

            default:
                break;
        }
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

        static void AddDefaultServices(ServiceCollection services) =>
            _ = services.AddScoped<IMapper, Mapper>()
                        .AddScoped<ILogger>(_ => AppLogger)
                        .AddScoped<IEventualLogger>(_ => AppLogger);
    }
}
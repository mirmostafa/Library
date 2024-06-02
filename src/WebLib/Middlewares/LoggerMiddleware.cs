using System.Diagnostics;

using Library.EventsArgs;
using Library.Web.Middlewares.Markers;

namespace Library.Web.Middlewares;

[MonitoringMiddleware]
public sealed class LoggerMiddleware : Markers.IMiddleware
{
    private readonly Func<HttpContext, Task> _activeInvoker;
    private readonly ILogger<LoggerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        this._logger = logger;
        this._next = next;
        this._activeInvoker = this._logger.IsEnabled(LogLevel.Debug) || this._logger.IsEnabled(LogLevel.Trace)
            ? this.InvokeFull
            : this.InvokeSimple;
    }

    [DebuggerStepThrough]
    public Task Invoke(HttpContext httpContext)
        => this._activeInvoker(httpContext);

    private async Task InvokeFull(HttpContext httpContext)
    {
        if (httpContext.Request.Method == "OPTIONS")
        {
            await this._next(httpContext);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        this._logger.LogDebug(new EventId(this.GetHashCode(), nameof(LoggerMiddleware)), "Calling {API}", httpContext.Request.Path);
        await this._next(httpContext);
        stopwatch.Stop();
        this._logger.LogTrace(new EventId(this.GetHashCode(), nameof(LoggerMiddleware)), "Called  {API} with status code: {StatusCode} in {Elapsed}", httpContext.Request.Path, httpContext.Response.StatusCode, stopwatch.Elapsed);
    }

    private async Task InvokeSimple(HttpContext httpContext)
        => await this._next(httpContext);
}

public static class LoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<LoggerMiddleware>();
}
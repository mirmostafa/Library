using System.Diagnostics;

using Library.EventsArgs;
using Library.Web.Bases;
using Library.Web.Middlewares.Markers;

namespace Library.Web.Middlewares;

[MonitoringMiddleware]
public sealed class LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger) : MiddlewareBase(next)
{
    private readonly ILogger<LoggerMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    [DebuggerStepThrough]
    public new async Task Invoke(HttpContext httpContext)
    {
        var timer = Stopwatch.StartNew();
        try
        {
            this._logger.LogDebug("Executing {api}", httpContext.Request.Path);
            await this._next(httpContext);
            return;
        }
        finally
        {
            timer.Stop();
            this._logger.LogTrace("Executed {api} in {elapsed}", httpContext.Request.Path, timer.Elapsed);
        }
    }

    protected override Task OnExecutingAsync(ItemActingEventArgs<HttpContext> e) => throw new NotImplementedException();
}

public static class LoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<LoggerMiddleware>();
}
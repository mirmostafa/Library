using Library.EventsArgs;
using Microsoft.AspNetCore.Http;

namespace Library.Web;

public abstract class MiddlewareBase
{
    private readonly RequestDelegate _next;

    protected MiddlewareBase(RequestDelegate next) =>
            this._next = next;

    [System.Diagnostics.DebuggerStepThrough]
    public async Task Invoke(HttpContext httpContext)
    {
        var onExecutingEventArgs = new ItemActingEventArgs<HttpContext>(httpContext);
        await this.OnExecutingAsync(onExecutingEventArgs);
        if (!onExecutingEventArgs.Handled)
        {
            await this._next(httpContext);
        }

        var onExecutedEventArgs = new ItemActedEventArgs<HttpContext>(httpContext);
        await this.OnExecutedAsync(onExecutedEventArgs);
    }
    protected virtual async Task OnExecutedAsync(ItemActedEventArgs<HttpContext> e) =>
        await Task.CompletedTask;

    protected abstract Task OnExecutingAsync(ItemActingEventArgs<HttpContext> e);
}
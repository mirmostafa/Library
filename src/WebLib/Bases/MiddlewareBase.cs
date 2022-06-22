using System.Security.Claims;
using Library.Coding;
using Library.EventsArgs;

namespace Library.Web.Bases;

public abstract class MiddlewareBase
{
    private readonly RequestDelegate _next;

    protected MiddlewareBase(RequestDelegate next)
        => this._next = next;

    protected ClaimsPrincipal? User { get; private set; }

    [System.Diagnostics.DebuggerStepThrough]
    public async Task Invoke(HttpContext httpContext)
    {
        CodeHelper.CatchResult(async () => await initialize(httpContext));
        await executing(httpContext);
        await executed(httpContext);
        return;

        async Task executing(HttpContext httpContext)
        {
            var onExecutingEventArgs = new ItemActingEventArgs<HttpContext?>(httpContext);
            await this.OnExecutingAsync(onExecutingEventArgs);
            if (!onExecutingEventArgs.Handled)
            {
                await this._next(httpContext);
            }
        }

        async Task executed(HttpContext httpContext)
        {
            var onExecutedEventArgs = new ItemActedEventArgs<HttpContext?>(httpContext);
            await this.OnExecutedAsync(onExecutedEventArgs);
        }

        async Task initialize(HttpContext httpContext)
        {
            this.User = httpContext?.User;
            await this.OnInitializeAsync(httpContext);
        }
    }

    protected virtual async Task OnInitializeAsync(HttpContext? httpContext)
        => await Task.CompletedTask;

    protected virtual async Task OnExecutedAsync(ItemActedEventArgs<HttpContext?> e)
        => await Task.CompletedTask;

    protected abstract Task OnExecutingAsync(ItemActingEventArgs<HttpContext?> e);
}
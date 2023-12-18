using System.Diagnostics;
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

    [DebuggerStepThrough]
    public async Task Invoke(HttpContext httpContext)
    {
        _ = await CodeHelper.CatchResultAsync(() => initialize(httpContext));
        await executing(httpContext);
        return;

        async Task executing(HttpContext httpContext)
        {
            var onExecutingEventArgs = new ItemActingEventArgs<HttpContext>(httpContext);
            await this.OnExecutingAsync(onExecutingEventArgs);
            if (!onExecutingEventArgs.Handled)
            {
                await this._next(httpContext);
            }
        }

        async Task initialize(HttpContext httpContext)
        {
            this.User = httpContext.User;
            await this.OnInitializeAsync(httpContext);
        }
    }

    protected abstract Task OnExecutingAsync(ItemActingEventArgs<HttpContext> e);

    protected virtual async Task OnInitializeAsync(HttpContext httpContext)
        => await Task.CompletedTask;
}
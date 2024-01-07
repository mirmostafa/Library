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

    //[DebuggerStepThrough]
    public async Task Invoke(HttpContext httpContext)
    {
        var onExecutingEventArgs = new ItemActingEventArgs<HttpContext>(httpContext);
        await this.OnExecutingAsync(onExecutingEventArgs);
        if (!onExecutingEventArgs.Handled)
        {
            await this._next(httpContext);
        }
    }

    protected abstract Task OnExecutingAsync(ItemActingEventArgs<HttpContext> e);
}
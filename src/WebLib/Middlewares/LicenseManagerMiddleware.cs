using Library.EventsArgs;
using Library.Web.Middlewares.Markers;

namespace Library.Web.Middlewares;

[ShortCircuitMiddleware]
public class LicenseManagerMiddleware : InfraMiddlewareBase, IInfraMiddleware
{
    public LicenseManagerMiddleware(RequestDelegate next)
        : base(next)
    {
    }

    protected override async Task OnExecutingAsync(ItemActingEventArgs<HttpContext?> args)
        => await Task.CompletedTask;
}
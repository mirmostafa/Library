using System.Text;

using Library.EventsArgs;
using Library.Web.Middlewares.Markers;

namespace Library.Web.Middlewares;

[ContentGeneratorMiddleware]
public sealed class InterceptorMiddleware(RequestDelegate next) : InfraMiddlewareBase(next)
{
    protected override async Task OnExecutingAsync(ItemActingEventArgs<HttpContext?> args)
    {
        var httpContext = args.Item;
        var path = httpContext?.Request.Path.ToString().Trim().Trim('/').ToLower();
        if (path?.EndsWith("/infra") is true && httpContext is { } http)
        {
            await http.Response.WriteAsync("<h1><center>MES Infrastructure is up.</center></h1>", Encoding.UTF8);
        }
    }
}
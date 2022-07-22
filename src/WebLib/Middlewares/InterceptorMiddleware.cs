using System.Text;

using Library.EventsArgs;
using Library.Web.Middlewares.Markers;

namespace Library.Web.Middlewares;

[ContentGeneratorMiddleware]
public sealed class InterceptorMiddleware : InfraMiddlewareBase
{
    public InterceptorMiddleware(RequestDelegate next)
        : base(next)
    {
        
    }

    protected override async Task OnExecutingAsync(ItemActingEventArgs<HttpContext?> args)
    {
        var httpContext = args.Item;
        var path = httpContext?.Request.Path.ToString().Trim().Trim('/').ToLower();
        if (path?.EndsWith("/infra") is true)
        {
            await httpContext?.Response.WriteAsync("<h1><center>MES Infrastructure is up.</center></h1>", Encoding.UTF8);
        }
    }
}
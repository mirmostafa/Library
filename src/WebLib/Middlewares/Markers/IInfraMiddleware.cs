namespace Library.Web.Middlewares.Markers;

public interface IInfraMiddleware
{
    Task Invoke(HttpContext httpContext);
}
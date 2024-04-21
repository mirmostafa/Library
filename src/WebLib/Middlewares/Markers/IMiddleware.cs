namespace Library.Web.Middlewares.Markers;

public interface IMiddleware
{
    Task Invoke(HttpContext httpContext);
}
using Microsoft.AspNetCore.Http;

namespace Library.Web.Middlewares.Markers
{
    internal interface IInfraMiddleware
    {
#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        Task Invoke(HttpContext httpContext);
    }
}
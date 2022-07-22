using Microsoft.AspNetCore.Http;

namespace Library.Web.Middlewares
{
    public abstract class InfraMiddlewareBase : MesMiddlewareBase
    {
        protected InfraMiddlewareBase(RequestDelegate next)
            : base(next)
        {
        }
    }
}
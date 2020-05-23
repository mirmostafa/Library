using System.Web;

namespace Mohammad.Web.Api
{
    public abstract class WebApiApplicationBase<TApplication> : HttpApplication
        where TApplication : WebApiApplicationBase<TApplication>
    {
        protected WebApiApplicationBase() => Current = this as TApplication;

        public static TApplication Current { get; private set; }
    }
}
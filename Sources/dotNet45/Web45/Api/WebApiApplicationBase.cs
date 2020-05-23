using System.Web;

namespace Mohammad.Web.Api
{
    public abstract class WebApiApplicationBase<TApplication> : HttpApplication
        where TApplication : WebApiApplicationBase<TApplication>
    {
        public static TApplication Current { get; private set; }
        protected WebApiApplicationBase() => Current = this as TApplication;
    }
}
using System;
using System.Web.Http.Filters;

namespace Mohammad.Web.Api.Internals
{
    internal class ApiHandleExceptionsAttribute : ExceptionFilterAttribute
    {
        private readonly Action<HttpActionExecutedContext> _OnHandlingException;

        public ApiHandleExceptionsAttribute(Action<HttpActionExecutedContext> onHandlingException) => this._OnHandlingException = onHandlingException;

        /// <inheritdoc />
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            this._OnHandlingException?.Invoke(actionExecutedContext);
            base.OnException(actionExecutedContext);
        }
    }
}
using System;
using System.Web.Http.ExceptionHandling;

namespace Mohammad.Web.Api.Internals
{
    internal class ApiExceptionLogger : ExceptionLogger
    {
        private readonly Action<ExceptionLoggerContext> _OnLoggingException;

        public ApiExceptionLogger(Action<ExceptionLoggerContext> onLoggingException) => this._OnLoggingException = onLoggingException;

        /// <inheritdoc />
        public override void Log(ExceptionLoggerContext context)
        {
            this._OnLoggingException?.Invoke(context);
            base.Log(context);
        }
    }
}
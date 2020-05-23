#region Code Identifications

// Created on     2017/09/25
// Last update on 2017/10/30 by Mohammad Mir mostafa 

#endregion

using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Mohammad.Web.Api.Internals;

// ReSharper disable once CheckNamespace
namespace Mohammad.Helpers
{
    public static class WebApiConfigHelper
    {
        public static void Initialize(this HttpConfiguration config,
            Action<HttpActionContext> onApiExecuting = null,
            Action<HttpActionExecutedContext> onApiExecuted = null,
            Action<HttpActionExecutedContext> onHandlingException = null,
            Action<ExceptionLoggerContext> onLoggingException = null)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Filters.Add(new ApiProxyAttribute(onApiExecuting, onApiExecuted));
            if (onHandlingException != null)
            {
                config.Filters.Add(new ApiHandleExceptionsAttribute(onHandlingException));
            }

            if (onLoggingException != null)
            {
                config.Services.Add(typeof(IExceptionLogger), new ApiExceptionLogger(onLoggingException));
            }
        }
    }
}
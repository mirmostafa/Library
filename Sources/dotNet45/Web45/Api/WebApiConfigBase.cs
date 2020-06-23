// Created on     2017/12/17
// Last update on 2018/01/03 by Mohammad Mir mostafa 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Mohammad.DesignPatterns.Creational;
using Mohammad.Diagnostics;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Logging;
using Mohammad.Validation.Exceptions;
using Mohammad.Web.Api.Exceptions;

namespace Mohammad.Web.Api
{
    public abstract class WebApiConfigBase<TWebApiConfig> : Singleton<TWebApiConfig>
        where TWebApiConfig : WebApiConfigBase<TWebApiConfig>
    {
        //! Singleton App
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object _WebApiConfigHelperLockObject = new object();

        public DateTime AppStartTime { get; private set; }

        public static void Log(string log, LogLevel level = LogLevel.Debug)
        {
            Instance.OnLogging(log, level);
        }

        public static void Register(HttpConfiguration config)
        {
            Instance.OnInitializing(config);

            Instance.AppStartTime = DateTime.Now;
            Diag.RedirectDebugsToOutputPane(true);

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", new {id = RouteParameter.Optional});

            Instance.OnInitialized(config);
        }

        protected virtual bool IsValidArgument((IEnumerable<KeyValuePair<string, object>> Arguments, KeyValuePair<string, object> CurrentArgument) e) => true;

        protected virtual void OnApiExecuted(HttpActionExecutedContext httpActionExecutedContext)
        {
        }

        protected virtual void OnApiExecuting(HttpActionContext actionContext)
        {
        }

        protected virtual HttpResponseMessage OnHandedException(HttpActionExecutedContext actionExecutedContext, (HttpStatusCode Code, string Message) error) =>
            actionExecutedContext.Request.CreateErrorResponse(error.Code, error.Message);

        protected virtual void OnInitialized(HttpConfiguration config)
        {
        }

        protected virtual void OnInitializing(HttpConfiguration config)
        {
            config.Initialize(this.ApiExecuting, this.ApiExecuted, this.HandleException, LogException);
        }

        protected virtual void OnLogging(string log, LogLevel level)
        {
            Debug.WriteLine($"[{DateTime.Now}] {log}");
        }

        protected virtual void OnRequestReceived(HttpRequestMessage request)
        {
        }

        protected virtual void OnResponseReceived(HttpResponseMessage response)
        {
        }

        private void ApiExecuted(HttpActionExecutedContext httpActionExecutedContext)
        {
            this.OnApiExecuted(httpActionExecutedContext);
        }

        private void ApiExecuting(HttpActionContext actionContext)
        {
            this.OnApiExecuting(actionContext);
            try
            {
                this.OnRequestReceived(actionContext.Request);

                if (actionContext.ActionArguments != null)
                {
                    foreach (var argument in actionContext.ActionArguments)
                    {
                        var e = (actionContext.ActionArguments, argument);
                        if (this.IsValidArgument(e))
                        {
                            continue;
                        }

                        if (argument.Value != null)
                        {
                            continue;
                        }

                        var argumentBinding =
                            actionContext.ActionDescriptor?.ActionBinding.ParameterBindings.FirstOrDefault(pb => pb.Descriptor.ParameterName == argument.Key);

                        if (argumentBinding?.Descriptor?.IsOptional ?? true)
                        {
                            continue;
                        }

                        WebApiApplication.Current.CompleteRequest();
                        actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Arguments value for {argument.Key} cannot be null");
                        return;
                    }
                }

                if (actionContext.ModelState.IsValid)
                {
                    return;
                }

                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                foreach (var state in actionContext.ModelState.Values)
                foreach (var error in state.Errors)
                {
                    Debug.WriteLine($"Validator: {error.ErrorMessage}");
                }
            }
            finally
            {
                this.OnResponseReceived(actionContext.Response);
            }
        }

        private void HandleException(HttpActionExecutedContext actionExecutedContext)
        {
            (HttpStatusCode code, string message) error = (HttpStatusCode.BadRequest, actionExecutedContext.Exception?.GetBaseException().Message);

            switch (actionExecutedContext.Exception)
            {
                case ObjectNotFoundException ex:
                    error = (HttpStatusCode.NotFound, ex.GetBaseException().Message);
                    break;
                case ObjectDuplicateException ex:
                    error = (HttpStatusCode.Ambiguous, ex.GetBaseException().Message);
                    break;
                case ValidationException ex:
                    error = (HttpStatusCode.ExpectationFailed, ex.GetBaseException().Message);
                    break;
                case ApiExceptionBase ex:
                    error = ex.Info;
                    break;
                case PairMessageStatusCodeExceptionBase<HttpStatusCode> ex:
                    error = (ex.StatusCode, ex.GetBaseException().Message);
                    break;
                case ExceptionBase ex:
                    error = (HttpStatusCode.BadRequest, ex.GetBaseException().Message);
                    break;
                case NotImplementedException ex:
                    error = (HttpStatusCode.NotImplemented, ex.GetBaseException().Message);
                    break;
                case HttpResponseException ex
                    when ex.Message ==
                         "Processing of the HTTP request resulted in an exception. Please see the HTTP response returned by the 'Response' property of this exception for details."
                    :
                    error = (HttpStatusCode.InternalServerError, "Test");
                    break;
                case Exception ex:
                    error = (HttpStatusCode.InternalServerError, ex.GetBaseException().Message);
                    break;
            }

            actionExecutedContext.Response = this.OnHandedException(actionExecutedContext, error);
        }

        private static void LogException(ExceptionLoggerContext context)
        {
            var exception = context.Exception;
            if (!(exception is ExceptionBase))
            {
                CodeHelper.LockAndCatch(() =>
                    {
                        var filePath = $@"Logs\WebAPI.Bugs.{DateTime.Now.ToShortDateString().Replace("\\", "-").Replace("/", "-")}.log";
                        filePath = string.Concat(Debugger.IsAttached ? HttpRuntime.AppDomainAppPath : @".\", filePath);
                        var logFile = new FileInfo(filePath);
                        if (!logFile.Directory.Exists)
                        {
                            logFile.Directory.Create();
                        }

                        File.AppendAllText(logFile.FullName, $@"[{DateTime.Now}]{context.Request.RequestUri}{Environment.NewLine}{exception}");
                    },
                    _WebApiConfigHelperLockObject);
            }

            Log(exception.GetBaseException().Message);
        }
    }
}
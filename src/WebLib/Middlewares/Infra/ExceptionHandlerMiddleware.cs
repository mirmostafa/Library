using System.Diagnostics;
using System.Net;
using System.Text.Json;

using Library.Exceptions;
using Library.Web.Middlewares.Markers;
using Library.Web.Results;

namespace Library.Web.Middlewares.Infra;

[MonitoringMiddleware]
public sealed class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger) : IInfraMiddleware
{
    private JsonSerializerOptions? _jsonOptions;

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await this.HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        Debugger.Break();
        //! Should always exist, but best to be safe!
        if (exception is null)
        {
            return;
        }

        //ApiResult result;
        int status;
        string message;
        List<object> extra = [];

        if (exception is IApiException apiException)
        {
            status = apiException.StatusCode ?? HttpStatusCode.BadRequest.Cast().ToInt();
            message = apiException.Message;
        }
        else
        {
            if (exception is NotImplementedException)
            {
                status = HttpStatusCode.NotImplemented.Cast().ToInt();
                message = "Sorry! This function is under development and is not done yet to be used. Please retry later.";
            }
            else
            {
                status = HttpStatusCode.InternalServerError.Cast().ToInt();
                message = exception.ToString();
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                if (traceId is not null)
                {
                    extra.Add(traceId);
                }
            }
            logger.LogError("Exception Handler Middleware Report: Error Message: '{message}'{Environment.NewLine}Status Code: {status}", message, Environment.NewLine, status);
        }
        this._jsonOptions ??= new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        ApiResult result = new(false, status, message, extraData: extra);
        var json = JsonSerializer.Serialize(result, this._jsonOptions);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = result.Status?.Cast().ToInt() ?? HttpStatusCode.BadRequest.Cast().ToInt();
        await context.Response.WriteAsync(json);
    }
}
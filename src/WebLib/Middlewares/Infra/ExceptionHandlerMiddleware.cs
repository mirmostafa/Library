using System.Collections.Immutable;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

using Library.Exceptions;
using Library.Results;
using Library.Web.Middlewares.Markers;
using Library.Web.Results;

namespace Library.Web.Middlewares.Infra;

[MonitoringMiddleware]
public sealed class ExceptionHandlerMiddleware : IInfraMiddleware
{
    private readonly RequestDelegate _Next;
    private readonly ILogger<ExceptionHandlerMiddleware> _Logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        this._Next = next;
        this._Logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this._Next(context);
        }
        catch (Exception ex)
        {
            await this.HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
#if DEBUG
        Debugger.Break();
#endif
        //! Should always exist, but best to be safe!
        if (exception is null)
        {
            return;
        }

        //ApiResult result;
        int status;
        string message;
        Dictionary<string, object> extra = new();

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
                    extra.Add("traceId", traceId);
                }
            }
            this._Logger.LogError($"Exception Handler Middleware Report: Error Message: '{message}'{Environment.NewLine}Status Code: {status}");
        }
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        ApiResult result = new(false, status, message, ExtraData: extra.Select(x => (x.Key, x.Value)));
        var json = JsonSerializer.Serialize(result, jsonOptions);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = result.Status?.Cast().ToInt() ?? HttpStatusCode.BadRequest.Cast().ToInt();
        await context.Response.WriteAsync(json);
    }
}
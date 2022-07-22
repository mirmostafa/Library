using System.Diagnostics;
using System.Net;
using System.Text.Json;

using Library.Exceptions;
using Library.Web.Middlewares.Markers;
using Library.Web.Results;

namespace Library.Web.Middlewares.Infra;

[MonitoringMiddleware]
public class ExceptionHandlerMiddleware : IInfraMiddleware
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
        //! Try and retrieve the error from the ExceptionHandler middleware
        //var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
        //var exception = exceptionDetails?.Error;

        //! Should always exist, but best to be safe!
        if (exception is null)
        {
            return;
        }

        ApiResult result;
        if (exception is IApiException apiException)
        {
            result = new(apiException.StatusCode ?? HttpStatusCode.BadRequest.ToInt(), apiException.Message);
        }
        else
        {
            if (exception is NotImplementedException)
            {
                var statusCode = HttpStatusCode.NotImplemented.ToInt();
                result = new(statusCode, "Sorry! This function is under development and is not done yet to be used. Please retry later.");

            }
            else
            {
                var statusCode = HttpStatusCode.InternalServerError.ToInt();
                result = new(statusCode, exception.GetBaseException().Message);

                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                if (traceId is not null)
                {
                    result.Extra.Add("traceId", traceId);
                }
            }
            this._Logger.LogError($"Exception Handler Middleware Report: Error Message: '{result.Message}'{Environment.NewLine}Status Code: {result.StatusCode}");
        }
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(result, jsonOptions);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = result.StatusCode?.ToInt() ?? HttpStatusCode.BadRequest.ToInt();
        await context.Response.WriteAsync(json);
    }
}
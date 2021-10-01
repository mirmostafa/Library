using System.Net;
using Library.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HanyCo.Infra.Web;

public interface IApiResult : IActionResult
{

}

public interface IApiResult<TResult> : IActionResult
{

}

public class ApiResult : StatusCodeResult, IApiResult
{
    public ApiResult(int? statusCode = null, string? message = null, bool? isSucceed = null)
        : base(statusCode ?? HttpStatusCode.OK.ToInt()) => this.Message = message;

    public Dictionary<string, object> Extra { get; } = new();

    public string? Message { get; }
}

public class ObjectApiResult<TResult> : ObjectResult, IApiResult<TResult>
{
    public ObjectApiResult(object? value) : base(value)
    {
    }
}

public static class ApiHelper
{
    public static IApiResult<TResult?> New<TResult>(TResult? result, string? message, int statusCode, bool isSucceed)
        => new ObjectApiResult<TResult?>(result) { StatusCode = statusCode };

    public static IApiResult New(string? message, int statusCode, bool isSucceed)
        => new ApiResult(statusCode);

    public static IApiResult<TResult?> New<TResult>(TResult? result)
        => New(result, null, HttpStatusCode.OK.ToInt(), true);

    public static IApiResult<TResult?> New<TResult>(TResult? result, bool isSucceed)
        => New(result, null, isSucceed ? HttpStatusCode.OK.ToInt() : HttpStatusCode.BadRequest.ToInt(), isSucceed);

    public static IApiResult New(int statusCode)
        => New(null, statusCode, statusCode == HttpStatusCode.OK.ToInt());

    public static IApiResult New(string message, int statusCode)
        => New(message, statusCode, statusCode == HttpStatusCode.OK.ToInt());

    public static IApiResult New(string message)
        => New(message, HttpStatusCode.OK.ToInt(), true);
}

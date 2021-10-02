using System.Net;
using Library.Helpers;
using Library.Web.Results;

namespace Library.Web.Helpers;

public static class ApiHelper
{
    public static IApiResult<TResult?> Result<TResult>(int statusCode, string? message, bool isSucceed, TResult? result)
        => new ObjectApiResult<TResult?>(result) { StatusCode = statusCode };

    public static IApiResult Result(int? statusCode, string? message, bool? isSucceed)
        => new ApiResult(statusCode, message, isSucceed);
    public static IApiResult Result(HttpStatusCode? statusCode, string? message, bool? isSucceed)
        => Result(statusCode?.ToInt(), message, isSucceed);

    public static IApiResult<TResult?> Result<TResult>(TResult? result, string? message, HttpStatusCode statusCode, bool isSucceed)
        => Result<TResult?>(statusCode.ToInt(), message, isSucceed, result);

    public static IApiResult<TResult?> OkResult<TResult>(TResult? result)
        => Result(HttpStatusCode.OK.ToInt(), null, true, result);
    public static IApiResult OkResult()
        => Result(HttpStatusCode.OK.ToInt());

    public static IApiResult<TResult?> Result<TResult>(TResult? result, bool isSucceed)
        => Result(isSucceed ? HttpStatusCode.OK.ToInt() : HttpStatusCode.BadRequest.ToInt(), null, isSucceed, result);

    public static IApiResult Result(int statusCode)
        => Result(statusCode, null, HttpStatusCodeHelper.ToHttpStatusCode(statusCode).IsSucceed());

    public static IApiResult Result(string message, int statusCode)
        => Result(statusCode, message, statusCode == HttpStatusCode.OK.ToInt());

    public static IApiResult Result(string message)
        => Result(HttpStatusCode.OK.ToInt(), message, true);

    public static bool IsSucceed(this IApiResult result)
        => HttpStatusCodeHelper.ToHttpStatusCode(result?.StatusCode).IsSucceed();
}

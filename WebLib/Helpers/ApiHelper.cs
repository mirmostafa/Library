using System.Net;
using Library.Helpers;
using Library.Web.Results;

namespace Library.Results;

public static class ApiHelper
{
    public static IApiResult<TResult?> Result<TResult>(TResult? result, string? message, int statusCode, bool isSucceed)
        => new ObjectApiResult<TResult?>(result) { StatusCode = statusCode };

    public static IApiResult Result(string? message, int statusCode, bool isSucceed)
        => new ApiResult(statusCode);

    public static IApiResult<TResult?> Result<TResult>(TResult? result)
        => Result(result, null, HttpStatusCode.OK.ToInt(), true);

    public static IApiResult<TResult?> Result<TResult>(TResult? result, bool isSucceed)
        => Result(result, null, isSucceed ? HttpStatusCode.OK.ToInt() : HttpStatusCode.BadRequest.ToInt(), isSucceed);

    public static IApiResult Result(int statusCode)
        => Result(null, statusCode, statusCode == HttpStatusCode.OK.ToInt());

    public static IApiResult Result(string message, int statusCode)
        => Result(message, statusCode, statusCode == HttpStatusCode.OK.ToInt());

    public static IApiResult Result(string message)
        => Result(message, HttpStatusCode.OK.ToInt(), true);
}

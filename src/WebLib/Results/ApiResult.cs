using System.Net;

using Library.Results;
using Library.Web.Helpers;

namespace Library.Web.Results;

public class ApiResult : ResultBase, IApiResult
{
    public ApiResult(int? statusCode = null, string? message = null)
        : base(statusCode, message) { }

    public ApiResult(HttpStatusCode? statusCode = null, string? message = null)
        : base(statusCode?.ToInt(), message) { }

    public HttpStatusCode? HttpStatusCode => HttpStatusCodeHelper.ToHttpStatusCode(this.Status?.ToInt());
    public override bool IsSucceed => this.Status?.ToInt() is null or (>= 200 and < 300);

    public static ApiResult BadRequest(string? message = null)
        => New(System.Net.HttpStatusCode.BadRequest, message);

    public static explicit operator HttpStatusCode?(ApiResult result)
        => result.HttpStatusCode;

    public static ApiResult New(int statusCode, string? message = null)
        => new(statusCode, message);

    public static ApiResult New(HttpStatusCode statusCode, string? message = null)
        => new(statusCode, message);

    public static ApiResult NoContent(string? message = null)
        => New(System.Net.HttpStatusCode.NoContent, message);

    public static ApiResult NotFound(string? message = null)
        => New(System.Net.HttpStatusCode.NotFound, message);

    public static ApiResult Ok(string? message = null)
        => New(System.Net.HttpStatusCode.OK, message);

    public static ApiResult Unauthorized(string? message = null)
        => New(System.Net.HttpStatusCode.Unauthorized, message);
}
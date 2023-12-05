using System.Collections.Immutable;
using System.Net;

using Library.Results;
using Library.Web.Helpers;

namespace Library.Web.Results;

public class ApiResult : ResultBase, IApiResult
{
    public ApiResult(int? statusCode = null, string? message = null)
        : base(null, statusCode, message, innerResult: null) { }

    public ApiResult(HttpStatusCode? statusCode = null, string? message = null)
        : base(null, statusCode?.Cast().ToInt(), message, innerResult: null) { }

    public ApiResult(in bool? Succeed = null, in object? Status = null, in string? Message = null, in IEnumerable<(object Id, object Error)>? Errors = null, in IEnumerable<object>? extraData = null)
        : base(Succeed, Status, Message, Errors, extraData, innerResult: null)
    {
    }

    public HttpStatusCode? HttpStatusCode => HttpStatusCodeHelper.ToHttpStatusCode(this.Status?.Cast().ToInt());
    public override bool IsSucceed => this.Status?.Cast().ToInt() is null or (>= 200 and < 300);

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
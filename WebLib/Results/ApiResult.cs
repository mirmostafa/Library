using System.Net;
using Library.Helpers;
using Library.Web.Helpers;

namespace Library.Web.Results;

public class ApiResult : IApiResult
{
    public int? StatusCode { get; }
    public HttpStatusCode? HttpStatusCode => HttpStatusCodeHelper.ToHttpStatusCode(this.StatusCode);
    public string? Message { get; private set; }
    public bool IsSucceed => this.StatusCode?.ToInt() is null or >= 200 and < 300;
    public bool Failure => !this.IsSucceed;

    public Dictionary<string, object> Extra { get; } = new();
    public List<string> Errors { get; } = new();

    public ApiResult(HttpStatusCode? statusCode = null, string? message = null)
        => (this.StatusCode, this.Message) = (statusCode?.ToInt(), message);

    public ApiResult(int? statusCode = null, string? message = null)
        => (this.StatusCode, this.Message) = (statusCode, message);

    public void Deconstruct(out int? statusCode, out string? message)
        => (statusCode, message) = (this.StatusCode, this.Message);

    public static ApiResult Ok(string? message = null) => new(System.Net.HttpStatusCode.OK, message);

    public static ApiResult New(int statusCode, string? message = null)
        => new(statusCode, message);

    public static ApiResult New(HttpStatusCode statusCode, string? message = null)
        => new(statusCode, message);

    public static ApiResult BadRequest(string? message = null)
        => New(System.Net.HttpStatusCode.BadRequest, message);

    public static ApiResult Unauthorized(string? message = null)
        => New(System.Net.HttpStatusCode.Unauthorized, message);

    public static ApiResult NotFound(string? message = null)
        => New(System.Net.HttpStatusCode.NotFound, message);

    public static ApiResult NoContent(string? message = null)
        => New(System.Net.HttpStatusCode.NoContent, message);

    public static explicit operator HttpStatusCode?(ApiResult result)
        => result.HttpStatusCode;
}

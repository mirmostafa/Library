using System.Net;
using Library.Helpers;
using Library.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Results;

public class ApiResult : StatusCodeResult, IApiResult
{
    //public ApiResult(int? statusCode = null, string? message = null, bool? isSucceed = null)
    public ApiResult(int? statusCode = null, string? message = null, bool? isSucceed = null)
        : base(statusCode ?? HttpStatusCode.OK.ToInt())
        => (this.Message, this.IsSucceed) = (message, isSucceed ?? HttpStatusCodeHelper.ToHttpStatusCode(statusCode).IsSucceed());

    public Dictionary<string, object> Extra { get; } = new();

    public string? Message { get; }
    public bool IsSucceed { get; init; }
    string? IApiResult.Message { get; init; }
}
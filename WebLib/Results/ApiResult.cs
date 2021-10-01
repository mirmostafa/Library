using System.Net;
using Library.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Results;

public class ApiResult : StatusCodeResult, IApiResult
{
    public ApiResult(int? statusCode = null, string? message = null, bool? isSucceed = null)
        : base(statusCode ?? HttpStatusCode.OK.ToInt()) => this.Message = message;

    public Dictionary<string, object> Extra { get; } = new();

    public string? Message { get; }
}
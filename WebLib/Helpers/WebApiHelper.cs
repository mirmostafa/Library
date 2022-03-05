using Library.Cqrs;
using Library.Validations;
using Library.Web.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Library.Helpers;

public static class WebApiHelper
{
    public static ApiResult ToApiResult(this CommandResult commandResult, string? message = null)
        => new(commandResult.ArgumentNotNull(nameof(commandResult)).IsSucceed ? HttpStatusCode.OK : HttpStatusCode.BadRequest, message);

    public static ApiResult<TResult> ToApiResult<TResult>(this CommandResult<TResult> commandResult, string? message = null)
        => new(commandResult.ArgumentNotNull(nameof(commandResult)).IsSucceed ? HttpStatusCode.OK : HttpStatusCode.BadRequest, message, commandResult.Result);

    public static ContentResult ToHtml(string content)
            => new()
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
}

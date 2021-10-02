using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Library.Web.Results;

public interface IApiResult : IActionResult, IStatusCodeActionResult
{
    string? Message { get; init; }
    public bool IsSucceed { get; init; }
    Dictionary<string, object>? Extra { get; }
}

using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Results;

public class ObjectApiResult<TResult> : ObjectResult, IApiResult<TResult>
{
    public ObjectApiResult(object? value) : base(value)
    {
    }
}

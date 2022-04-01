using Library.Coding;
using Library.Validations;
using Library.Web.Results;

namespace Library.Web.Helpers;

public static class WebResultFluent
{
    public static TWebResult OnSucceed<TWebResult>(this TWebResult result, Action<TWebResult> action)
        where TWebResult : IApiResult
    {
        _ = result.ArgumentNotNull().IsSucceed.IfTrue(() => action(result));
        return result;
    }
    public static TWebResult OnFailure<TWebResult>(this TWebResult result, Action<TWebResult> action)
        where TWebResult : IApiResult
    {
        _ = result.ArgumentNotNull().IsFailure.IfTrue(() => action(result));
        return result;
    }
    public static TWebResult OnDone<TWebResult>(this TWebResult result, Action<TWebResult> action)
        where TWebResult : IApiResult
    {
        action(result);
        return result;
    }
}

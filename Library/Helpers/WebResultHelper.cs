using Library.Web;

namespace Library.Helpers;

public static class ResultHelper
{
    public static WebResult OnSuccess(this WebResult result, Func<WebResult, WebResult> func) 
        => result.Failure ? result : func(result);

    public static WebResult OnSuccess(this WebResult result, Action<WebResult> action)
    {
        if (result.Failure)
        {
            return result;
        }

        action(result);

        return WebResult.Ok();
    }

    public static WebResult OnSuccess<T>(this WebResult<T> result, Action<T?> action)
    {
        if (result.Failure)
        {
            return result;
        }

        action(result.Value);

        return WebResult.Ok();
    }

    public static WebResult OnSuccess<T>(this WebResult<T> result, Func<T?, WebResult> func)
        => result.Failure ? result : func(result.Value);

    public static WebResult OnFailure(this WebResult result, Action<WebResult> action)
    {
        if (result.Failure)
        {
            action(result);
        }

        return result;
    }

    public static WebResult OnBoth(this WebResult result, Action<WebResult> action)
    {
        action(result);

        return result;
    }

    public static T OnBoth<T>(this WebResult result, Func<WebResult, T> func) 
        => func(result);
}
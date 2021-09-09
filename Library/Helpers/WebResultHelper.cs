using Library.Web;

namespace Library.Helpers;

public static class ResultHelper
{
    public static Result OnSuccess(this Result result, Func<Result> func) => result.Failure ? result : func();

    public static Result OnSuccess(this Result result, Action action)
    {
        if (result.Failure)
        {
            return result;
        }

        action();

        return Result.Ok();
    }

    public static Result OnSuccess<T>(this Result<T> result, Action<T?> action)
    {
        if (result.Failure)
        {
            return result;
        }

        action(result.Value);

        return Result.Ok();
    }

    public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
        => result.Failure ? Result.Fail<T>(result.Error!) : Result.Ok(func());

    public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
        => result.Failure ? Result.Fail<T>(result.Error!) : func();

    public static Result OnSuccess<T>(this Result<T> result, Func<T?, Result> func)
        => result.Failure ? result : func(result.Value);

    public static Result OnFailure(this Result result, Action action)
    {
        if (result.Failure)
        {
            action();
        }

        return result;
    }

    public static Result OnBoth(this Result result, Action<Result> action)
    {
        action(result);

        return result;
    }

    public static T OnBoth<T>(this Result result, Func<Result, T> func) => func(result);
}
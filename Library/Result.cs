using System.Diagnostics.CodeAnalysis;

namespace Library;

public class Result
{
    public bool Success { get; private set; }
    public string Error { get; private set; }

    public bool Failure => !this.Success;

    protected Result(bool success, string error)
    {
        Check.Require(success || !string.IsNullOrEmpty(error));
        Check.Require(!success || string.IsNullOrEmpty(error));

        this.Success = success;
        this.Error = error;
    }

    public static Result Fail(string message) => new(false, message);

    public static Result<T> Fail<T>(string message) => new(default, false, message);

    public static Result Ok() => new(true, string.Empty);

    public static Result<T> Ok<T>(T value) => new(value, true, string.Empty);

    public static Result Combine(params Result[] results)
    {
        foreach (var result in results)
        {
            if (result.Failure)
            {
                return result;
            }
        }

        return Ok();
    }
}


public class Result<T> : Result
{
    private T? _value;
    public T? Value
    {
        get
        {
            Check.Require(this.Success);

            return this._value;
        }
        [param: AllowNull]
        private set => this._value = value;
    }

    protected internal Result([AllowNull] T value, bool success, string error)
        : base(success, error)
    {
        Check.Require(value is not null || !success);

        this.Value = value;
    }
}

public static class ResultExtensions
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

    public static Result<T> OnSuccess<T>(this Result result, Func<T> func) => result.Failure ? Result.Fail<T>(result.Error) : Result.Ok(func());

    public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func) => result.Failure ? Result.Fail<T>(result.Error) : func();

    public static Result OnSuccess<T>(this Result<T> result, Func<T?, Result> func) => result.Failure ? result : func(result.Value);

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

using Library.Dynamic;
using Library.Interfaces;
using Library.Windows;

namespace Library.Results;

public class Result : ResultBase, IEmpty<Result>
{
    private static readonly dynamic _staticFields = new Expando();

    public Result(in object? statusCode = null, in NotificationMessage? fullMessage = null)
        : base(statusCode, fullMessage) { }

    public static Result Empty { get; } = _staticFields.Empty ??= NewEmpty();
    public static Result Fail => _staticFields.Fail ??= CreateFail();
    public static Result Success => _staticFields.Success ??= CreateSuccess();

    public static Result CreateFail(in string? message = null, in object? erroCode = null)
        => new(erroCode ?? -1, message) { IsSucceed = false };

    public static Result CreateFail(in NotificationMessage? message, in object? erroCode = null)
        => new(erroCode ?? -1, message) { IsSucceed = false };

    public static Result CreateFail(in string message, in string instruction, in string tiltle, in string details, in object? statusCode = null)
        => new(statusCode, new NotificationMessage(message, instruction, tiltle, details));

    public static Result CreateSuccess(in NotificationMessage? fullMessage = null, in object? statusCode = null)
        => new(statusCode, fullMessage) { IsSucceed = true };

    public static explicit operator Result(bool b)
        => b ? Success : Fail;

    public static implicit operator bool(Result result!!)
        => result.IsSucceed;

    public static Result New()
        => new();

    public static Result NewEmpty()
        => New();
}
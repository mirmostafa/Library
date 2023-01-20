//using System;

//using Library.Dynamic;
//using Library.Interfaces;
//using Library.Validations;
//using Library.Windows;

//namespace Library.Results;

//public record Result : ResultBase, IEmpty<Result>
//{
//    private static readonly dynamic _staticFields = new Expando();

//    public Result(in object? status = null, in NotificationMessage? fullMessage = null)
//        : base(status, fullMessage) { }

//    public static Result Empty { get; } = _staticFields.Empty ??= NewEmpty();

//    public static Result Fail => _staticFields.Fail ??= CreateFail();

//    public static Result Success => _staticFields.Success ??= CreateSuccess();

//    public static Result CreateFail(in string? message = null, in object? error = null)
//        => new(error ?? -1, message) { IsSucceed = false };

//    public static Result CreateFail(in NotificationMessage? message, in object? error = null)
//        => new(error ?? -1, message) { IsSucceed = false };

//    public static Result CreateFail(in string message, in string instruction, in string tiltle, in string details, in object? error = null)
//        => new(error, new NotificationMessage(message, instruction, tiltle, details)) { IsSucceed = false };

//    public static Result CreateSuccess(in NotificationMessage? fullMessage = null, in object? status = null)
//        => new(status, fullMessage) { IsSucceed = true };

//    public static explicit operator Result(bool b)
//        => b ? Success : Fail;

//    public static Result From([DisallowNull] in ResultBase other)
//        => Add(other, new Result());

//    public static Result From([DisallowNull] in Result @this, in Result other)
//        => Add(@this, other);

//    public static implicit operator bool(Result result)
//        => result.NotNull().IsSucceed;

//    public static Result New()
//        => new();

//    public static Result NewEmpty()
//        => New();

//    public static Result operator +(Result left, Result right)
//        => left.With(right);

//    public Task<Result> ToAsync()
//        => Task.FromResult(this);

//    public Result With(in Result other)
//        => Result.From(this, other);
//}
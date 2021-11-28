using System.Diagnostics.CodeAnalysis;

namespace Library.Results;

public sealed class Result : ResultBase
{
    public Result(int? statusCode = null, string? message = null)
        : base(statusCode, message) { }

    public static Result Success => new();
    public static Result Fail => CreateFail(-1);

    public static Result CreateFail(int erroCode = -1, string? message = null) =>
        new(erroCode, message);
    public static Result CreateSuccess(int erroCode = 0, string? message = null) =>
        new(erroCode, message);
}

public sealed class Result<TValue> : ResultBase
{
    public TValue Value { get; }

    public Result(TValue value, int? statusCode = null, string? message = null)
        : base(statusCode, message) => this.Value = value;

    public static Result<TValue?> Fail =>
        CreateFail(-1);

    public static Result<TValue?> CreateFail(int errorCode = -1, string? message = null, TValue? value = default!) =>
        new(value, errorCode, message);

    [return: NotNull]
    public static Result<TValue?> CreateSuccess(TValue value, int errorCode = 0, string? message = null) =>
        new(value, errorCode, message);

    public static implicit operator TValue(Result<TValue> result) =>
        result.Value;

    public void Deconstruct(out int? StatusCode, out string? Message, out TValue Value) =>
        (StatusCode, Message, Value) = (this.StatusCode, this.Message, this.Value);

    public void Deconstruct(out bool isSucceed, out TValue Value) =>
        (isSucceed, Value) = (this.IsSucceed, this.Value);
}

using System.Diagnostics.CodeAnalysis;

namespace Library.Results;

public class Result : ResultBase
{
    public Result(int? statusCode = null, string? message = null)
        : base(statusCode, message) { }

    public static Result Success => CreateSuccess();
    public static Result Fail => CreateFail();

    public static Result CreateFail(string? message = null, int erroCode = -1) =>
        new(erroCode, message) { IsSucceed = false };
    public static Result CreateSuccess(int erroCode = 0, string? message = null) =>
        new(erroCode, message) { IsSucceed = true };
}

public class Result<TValue> : ResultBase
{
    public TValue Value { get; }

    public Result(in TValue value, in int? statusCode = null, in string? message = null)
        : base(statusCode, message) =>
        this.Value = value;

    public static Result<TValue?> Fail =>
        CreateFail(errorCode: -1);

    [return: NotNull]
    public static Result<TValue?> CreateFail(in string? message = null, in TValue? value = default, in int errorCode = -1) =>
        new(value, errorCode, message);

    [return: NotNull]
    public static Result<TValue?> CreateSuccess(in TValue value, in string? message = null, in int errorCode = 0) =>
        new(value, errorCode, message);

    public bool Equals(Result<TValue?> other) =>
        other is not null &&
        (other.StatusCode, other.IsSucceed) == (this.StatusCode, this.IsSucceed) &&
        (other.Value?.Equals(this.Value) ?? this.Value is null);

    public void Deconstruct(out int? StatusCode, out string? Message, out TValue Value) =>
        (StatusCode, Message, Value) = (this.StatusCode, this.Message, this.Value);
    public void Deconstruct(out bool isSucceed, out TValue Value) =>
        (isSucceed, Value) = (this.IsSucceed, this.Value);

    public static implicit operator TValue(in Result<TValue> result) =>
        result.Value;
    public static implicit operator bool(in Result<TValue> result) =>
        result.IsSucceed;
}

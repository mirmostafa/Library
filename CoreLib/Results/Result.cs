namespace Library.Results;

public sealed class Result : ResultBase
{
    public Result(int? statusCode = null, string? message = null)
        : base(statusCode, message) { }

    public static Result Success => new();
    public static Result Fail =>
        CreateFail(-1);
    public static Result CreateFail(int erroCode = -1, string? message = null) =>
        new(erroCode, message);

    public void Deconstruct(out int? ErrorCode, out string? Message)
        => (ErrorCode, Message) = (this.StatusCode, this.Message);
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
    public static Result<TValue?> CreateSuccess(TValue value, int errorCode = 0, string? message = null) =>
        new(value, errorCode, message);

    public static implicit operator TValue(Result<TValue> result) =>
        result.Value;
    public static implicit operator bool(Result<TValue> result) =>
        result?.IsSucceed ?? false;

    public void Deconstruct(out int? StatusCode, out string? Message, TValue Value) =>
        (StatusCode, Message, Value) = (this.StatusCode, this.Message, this.Value);
}

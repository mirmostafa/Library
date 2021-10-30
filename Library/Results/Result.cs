namespace Library.Results;

public sealed class Result : ResultBase
{
    public Result(int? statusCode = null, string? message = null)
        : base(statusCode, message) { }
}

public sealed class Result<TValue> : ResultBase
{
    public TValue Value { get; }
    public Result(TValue value, int? statusCode = null, string? message = null)
        : base(statusCode, message) => this.Value = value;
}

public sealed record TryMethodResult<TResult>
{
    public TryMethodResult(bool succeed, TResult result)
        => (this.Succeed, this.Result) = (succeed, result);

    public bool Succeed { get; }
    public TResult Result { get; }

    public void Deconstruct(out bool succeed, out TResult result)
        => (succeed, result) = (this.Succeed, this.Result);

    public static implicit operator TResult(in TryMethodResult<TResult> result)
        => result.Result;

    public static implicit operator bool(in TryMethodResult<TResult> result)
        => result.Succeed;
}
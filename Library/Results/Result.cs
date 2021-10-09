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
namespace Library.Results;

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
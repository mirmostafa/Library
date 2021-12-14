namespace Library.Results;

public sealed class TryMethodResult<TResult> : Result<TResult>
{
    public TryMethodResult(bool isSucceed, TResult result)
        : base(result) 
        => this.IsSucceed = isSucceed;
}
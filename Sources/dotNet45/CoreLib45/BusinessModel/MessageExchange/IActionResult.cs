


namespace Mohammad.BusinessModel.MessageExchange
{
    public interface IActionResult
    {
        int StatusCode { get; }
        string Message { get; }

        bool IsSucceed { get; }
    }

    public interface IActionResult<TResult> : IActionResult
    {
        TResult Result { get; set; }
    }
}
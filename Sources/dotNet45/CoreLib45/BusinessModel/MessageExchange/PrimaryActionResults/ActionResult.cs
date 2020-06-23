namespace Mohammad.BusinessModel.MessageExchange.PrimaryActionResults
{
    public abstract class ActionResultBase : IActionResult
    {
        public int StatusCode { get; protected set; }
        public string Message { get; protected set; }
        public bool IsSucceed { get; }

        protected ActionResultBase(int code, string message, bool isSucceed)
        {
            this.StatusCode = code;
            this.Message = message;
            this.IsSucceed = isSucceed;
        }
    }

    public sealed class ActionResult : ActionResultBase
    {
        public ActionResult(int code, string message, bool isSucceed)
            : base(code, message, isSucceed)
        {
        }
    }
}
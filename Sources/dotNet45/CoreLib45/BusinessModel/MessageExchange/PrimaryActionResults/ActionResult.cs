#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

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
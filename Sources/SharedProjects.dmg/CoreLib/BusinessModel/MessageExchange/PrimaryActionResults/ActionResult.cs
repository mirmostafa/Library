#region Code Identifications

// Created on     2017/11/12
// Last update on 2017/12/17 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.BusinessModel.MessageExchange.PrimaryActionResults
{
    public abstract class ActionResultBase : IActionResult
    {
        protected ActionResultBase((string Message, int Code) result)
        {
            this.StatusCode = result.Code;
            this.Message = result.Message;
        }

        public int StatusCode { get; protected set; }
        public string Message { get; protected set; }
    }

    public sealed class ActionResult : ActionResultBase
    {
        public ActionResult((string Message, int Code) result)
            :base(result)
        {
            
        }
    }
}
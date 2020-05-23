#region Code Identifications

// Created on     2017/12/17
// Last update on 2017/12/17 by Mohammad Mir mostafa 

#endregion

using System.Net;
using Mohammad.Helpers;

namespace Mohammad.BusinessModel.MessageExchange.PrimaryActionResults
{
    public sealed class ActionResult<TResult> : ActionResultBase, IActionResult<TResult>
    {
        /// <inheritdoc />
        public ActionResult((string Message, int Code) result, TResult actionResult)
            : base(result) => this.Result = actionResult;

        public ActionResult(TResult actionResult)
            : base(("Succeed", HttpStatusCode.OK.ToInt())) => this.Result = actionResult;

        public TResult Result { get; set; }
    }
}
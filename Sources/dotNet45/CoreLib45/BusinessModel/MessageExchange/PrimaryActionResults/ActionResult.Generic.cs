#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Net;
using Mohammad.Helpers;

namespace Mohammad.BusinessModel.MessageExchange.PrimaryActionResults
{
    public sealed class ActionResult<TResult> : ActionResultBase, IActionResult<TResult>
    {
        public TResult Result { get; set; }

        /// <inheritdoc />
        public ActionResult(int code, string message, bool isSucceed, TResult actionResult)
            : base(code, message, isSucceed) => this.Result = actionResult;

        public ActionResult(bool isSucceed, TResult actionResult)
            : base(HttpStatusCode.OK.ToInt(), "Succeed", isSucceed) => this.Result = actionResult;
    }
}
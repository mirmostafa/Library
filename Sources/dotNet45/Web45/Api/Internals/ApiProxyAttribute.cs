#region Code Identifications

// Created on     2017/10/30
// Last update on 2017/10/30 by Mohammad Mir mostafa 

#endregion

using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Mohammad.Web.Api.Internals
{
    internal class ApiProxyAttribute : ActionFilterAttribute
    {
        private readonly Action<HttpActionExecutedContext> _OnApiExecuted;
        private readonly Action<HttpActionContext> _OnApiExecuting;

        public ApiProxyAttribute(Action<HttpActionContext> onApiExecuting, Action<HttpActionExecutedContext> onApiExecuted)
        {
            this._OnApiExecuting = onApiExecuting;
            this._OnApiExecuted = onApiExecuted;
        }

        /// <inheritdoc />
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            this._OnApiExecuting?.Invoke(actionContext);
            base.OnActionExecuting(actionContext);
        }

        /// <inheritdoc />
        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            this._OnApiExecuted?.Invoke(actionContext);
            base.OnActionExecuted(actionContext);
        }
    }
}
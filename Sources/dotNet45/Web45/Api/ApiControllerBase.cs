// Created on     2017/12/17
// Last update on 2018/01/03 by Mohammad Mirmostafa 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Mohammad.BusinessModel.MessageExchange;
using Mohammad.BusinessModel.MessageExchange.PrimaryActionResults;
using Mohammad.Helpers;
using Mohammad.Web.Api.MessageExchange;

namespace Mohammad.Web.Api
{
    public abstract class ApiControllerBase : ApiController
    {
        protected virtual IActionResult DefaultActionResult { get; } = new OkActionResult();

        protected virtual IHttpActionResult CreateHttpActionResult<TResult>(Func<IActionResult<TResult>> createResponseMessage)
        {
            if (createResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(createResponseMessage));
            }

            return this.CreateHttpActionResult(createResponseMessage());
        }

        protected virtual IHttpActionResult CreateHttpActionResult(Func<IActionResult> createResponseMessage)
        {
            if (createResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(createResponseMessage));
            }

            return this.CreateHttpActionResult(createResponseMessage());
        }

        protected virtual IHttpActionResult CreateHttpActionResult<TResult>(IActionResult<TResult> result) =>
            result.IsSucceed
                ? new ResponseMessageResult(this.Request.CreateResponse((HttpStatusCode)result.StatusCode, result, new JsonMediaTypeFormatter()))
                : new ResponseMessageResult(this.Request.CreateErrorResponse((HttpStatusCode)result.StatusCode, result.Message));

        protected virtual IHttpActionResult CreateHttpActionResult(IActionResult result) =>
            result.IsSucceed
                ? new ResponseMessageResult(this.Request.CreateResponse((HttpStatusCode)result.StatusCode, result))
                : new ResponseMessageResult(this.Request.CreateErrorResponse((HttpStatusCode)result.StatusCode, result.Message));

        protected virtual async Task<IHttpActionResult> CreateHttpActionResultAsync(Func<IActionResult> createResponseMessage)
        {
            return await this.Async(() => this.CreateHttpActionResult(createResponseMessage));
        }

        // Bad smell code
        //protected virtual async Task<IHttpActionResult> CreateHttpActionResultAsync(IActionResult result)
        //{
        //    return await this.Async(() => this.CreateHttpActionResult(result));
        //}

        protected virtual IHttpActionResult CreateHttpActionResult(int code, string message, bool isSucceed)
        {
            return this.CreateHttpActionResult(() => new ActionResult(code, message, isSucceed));
        }

        protected virtual IHttpActionResult CreateHttpActionResult<TResult>(int code, string message, bool isSucceed, TResult result)
        {
            return this.CreateHttpActionResult(() => new ActionResult<TResult>(code, message, isSucceed, result));
        }

        protected virtual IHttpActionResult CreateHttpActionResult<TResult>(bool isSucceed, TResult result)
        {
            return this.CreateHttpActionResult(() => new ActionResult<TResult>(isSucceed, result));
        }

        protected virtual async Task<IHttpActionResult> OkAsync() => await this.Async(this.Ok);

        protected virtual async Task Async(Action action)
        {
            await Task.Run(action);
        }

        protected virtual async Task<TResult> Async<TResult>(Func<TResult> action) => await Task.Run(action);

        protected IEnumerable<string> GetHeaders(string key)
        {
            var found = this.Request.Headers.TryGetValues(key, out var result);
            return found ? result : Enumerable.Empty<string>();
        }

        protected virtual IHttpActionResult Run<TResult>(Func<(TResult Result, string Message)> action)
        {
            (int Code, string Message, bool IsSucceed, TResult Result) buffer = (200, null, true, default);
            try
            {
                var (result, message) = action();
                buffer.Result = result;
                buffer.Message = message;
                buffer.IsSucceed = true;
            }
            catch (NotImplementedException ex)
            {
                buffer.Code = HttpStatusCode.NotImplemented.ToInt();
                buffer.Message = this.GetLocalizedString(ex.GetBaseException().Message);
                buffer.IsSucceed = false;
            }

            return this.CreateHttpActionResult(buffer.Code, buffer.Message, buffer.IsSucceed, buffer.Result);
        }

        protected virtual IHttpActionResult Run(Func<string> action)
        {
            (int Code, string Message, bool IsSucceed) buffer = (200, null, true);
            try
            {
                buffer.Message = action();
                buffer.IsSucceed = true;
            }
            catch (NotImplementedException ex)
            {
                buffer.Code = HttpStatusCode.NotImplemented.ToInt();
                buffer.Message = this.GetLocalizedString(ex.GetBaseException().Message);
                buffer.IsSucceed = false;
            }

            return this.CreateHttpActionResult(buffer.Code, buffer.Message, buffer.IsSucceed);
        }

        protected virtual async Task<IHttpActionResult> RunAsync<TResult>(Func<(TResult Result, string Message)> action)
        {
            return await this.Async(() => this.Run(action));
        }

        protected virtual string GetLocalizedString(string text) => text;

        protected virtual async Task<IHttpActionResult> RunAsync(Func<string> action)
        {
            return await this.Async(() => this.Run(action));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Mohammad.BusinessModel.MessageExchange
{
    public interface IActionResult
    {
        int StatusCode { get; }
        string Message { get; }
    }

    public interface IActionResult<TResult> : IActionResult
    {
        TResult Result { get; set; }
    }
}

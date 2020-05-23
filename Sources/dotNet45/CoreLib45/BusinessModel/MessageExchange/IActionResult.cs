#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

namespace Mohammad.BusinessModel.MessageExchange
{
    public interface IActionResult
    {
        int    StatusCode { get; }
        string Message    { get; }

        bool IsSucceed { get; }
    }

    public interface IActionResult<TResult> : IActionResult
    {
        TResult Result { get; set; }
    }
}
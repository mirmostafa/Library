using Library.Results;
using Library.Wpf.Dialogs;

namespace Library.Wpf.Helpers;

public static class ResultHelper
{
    public static TResult Show<TResult>(this TResult result, object? owner = null, string? instruction = null, string? successMessage = null)
        where TResult : ResultBase
    {
        _ = result.ThrowOnFail(owner, instruction);
        MsgBox2.Inform(instruction, successMessage);

        return result;
    }
}
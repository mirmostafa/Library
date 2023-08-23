using Library.Results;
using Library.Wpf.Dialogs;

namespace Library.Wpf.Helpers;

public static class WpfResultHelper
{
    public static TResult ShowOrThrow<TResult>(this TResult result, object? owner = null, string? instruction = null, string? successMessage = null)
        where TResult : ResultBase
    {
        _ = result.ThrowOnFail(owner, instruction);
        var text = result.Message.IfNullOrEmpty(successMessage ?? owner?.ToString());
        MsgBox2.Inform(instruction, text);

        return result;
    }

    public static async Task<TResult> ShowOrThrowAsync<TResult>(this Task<TResult> result, object? owner = null, string? instruction = null, string? successMessage = null)
        where TResult : ResultBase =>
        ShowOrThrow(await result, owner, instruction);
}
using System.Diagnostics.CodeAnalysis;

using Library.Results;
using Library.Wpf.Dialogs;
using Library.Wpf.Windows;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace Library.Wpf.Helpers;

public static class PageHelper
{
    public static async Task<Result> AskToSaveIfChangedAsync<TPage>([DisallowNull] this TPage page, [DisallowNull] string ask = "Do you want to save changes?")
        where TPage : IStatefulPage, IAsyncSavePage
        => page.NotNull().IsViewModelChanged
            ? MsgBox2.AskWithCancel(ask) switch
            {
                TaskDialogResult.Cancel or TaskDialogResult.Close => Result.Failure,
                TaskDialogResult.Yes => await page.SaveoDbAsync(),
                TaskDialogResult.No => Result.Success,
                _ => Result.Failure
            }
            : Result.Success;
}
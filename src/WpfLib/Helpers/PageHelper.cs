using System.Diagnostics.CodeAnalysis;

using Library.Interfaces;
using Library.Results;
using Library.Wpf.Dialogs;
using Library.Wpf.Windows;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace Library.Wpf.Helpers;

public static class PageHelper
{
    public static async Task<Result> AskToSaveAsync<TPage>(this TPage page, [DisallowNull] string ask = "Do you want to save changes?")
        where TPage : IStatefulPage, IAsyncSaveService
        => page.NotNull().IsViewModelChanged
            ? MsgBox2.AskWithCancel(ask) switch
            {
                TaskDialogResult.Cancel or TaskDialogResult.Close => Result.Fail,
                TaskDialogResult.Yes => await page.SaveChangesAsync(),
                TaskDialogResult.No => Result.Success,
                _ => Result.Success
            }
            : Result.Success;
}
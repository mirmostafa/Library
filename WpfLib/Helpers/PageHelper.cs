using Library.Results;
using Library.Wpf.Dialogs;
using Library.Wpf.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Library.Wpf.Helpers;

public static class PageHelper
{
    public static async Task<Result> AskToSaveAsync<TPage>(this TPage page)
        where TPage : IStatefulPage, ISaveChangesAsync
    {
        if (page.IsViewModelChanged)
        {
            var askToSave = MsgBox2.AskWithCancel("Do you want to save changes?");
            if (askToSave is TaskDialogResult.Cancel or TaskDialogResult.Close)
            {
                return Result.Fail;
            }

            if (askToSave is TaskDialogResult.Yes)
            {
                return await page.SaveChangesAsync();
            }
        }
        return Result.Success;
    }
}
using System.IO;

using Library.Results;
using Library.Wpf.Dialogs;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace Library.Wpf.Windows.UI;

public static class FileUiTools
{
    public static Result SaveToFile(IEnumerable<(string FilePath, string Text)> files, string? title = "Saving to file")
    {
        var fileList = files?.ToList();

        var validation = fileList.Check().ArgumentNotNull()
            .RuleFor(x => x!.All(item => !item.FilePath.IsNullOrEmpty()), () => new ArgumentNullException("File Path cannot be empty", (Exception?)null));
        if (!validation.TryParse(out var vr))
        {
            return vr;
        }

        var isYesToAll = false;
        foreach (var (filePath, fileText) in fileList!)
        {
            var dir = Path.GetDirectoryName(filePath) ?? Environment.CurrentDirectory;
            if (!Directory.Exists(dir))
            {
                _ = Directory.CreateDirectory(dir);
            }
            if (File.Exists(filePath))
            {
                if (!isYesToAll)
                {
                    var resp = askToSkipOrReplace(title, filePath);
                    switch (resp)
                    {
                        case TaskDialogResult.Ok:
                            isYesToAll = true;
                            break;

                        case TaskDialogResult.Cancel:
                            return Result.CreateFailure(new OperationCanceledException());

                        case TaskDialogResult.Yes:
                            break;

                        case TaskDialogResult.No:
                            continue;

                        default:
                            return Result.CreateFailure(new InvalidOperationException());
                    }
                }
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, fileText);
        }
        return Result.Success;

        static void setResp(object? o, EventArgs e, TaskDialogResult res) => MsgBox2.GetOnButtonClick(o, e).Parent.Close(res);

        static TaskDialogResult askToSkipOrReplace(string? title, string filePath)
        {
            var skipButton = ButtonInfo.New("&Skip", (o, e) => setResp(o, e, TaskDialogResult.No), isDefault: true).ToButton();
            var replButton = ButtonInfo.New("&Replace", (o, e) => setResp(o, e, TaskDialogResult.Yes)).ToButton();
            var replAllButton = ButtonInfo.New("Replace &all", (o, e) => setResp(o, e, TaskDialogResult.Ok), useElevationIcon: true).ToButton();
            var cancelButton = ButtonInfo.New("&Cancel", (o, e) => setResp(o, e, TaskDialogResult.Cancel)).ToButton();
            return MsgBox2.AskWithWarn($"The destination already has a file named \"{Path.GetFileName(filePath)}\".", text: title, $"Replace or Skip Files", controls: new[] { skipButton, replButton, replAllButton, cancelButton });
        }
    }
}
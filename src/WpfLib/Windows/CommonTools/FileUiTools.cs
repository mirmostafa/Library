using System.IO;

using Library.Exceptions.Validations;
using Library.Results;
using Library.Wpf.Dialogs;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace Library.Wpf.Windows.CommonTools;

public static class FileUiTools
{
    public static Result SaveToFile(IEnumerable<(string FilePath, string Text)> files, string? title = "Saving to file")
    {
        var fileList = files?.ToList();

        var validation = fileList.Check().ArgumentNotNull()
            .RuleFor(x => x!.Any(), () => new NoItemValidationException("No file to save"))
            .RuleFor(x => x!.All(item => !item.FilePath.IsNullOrEmpty()), () => new ArgumentNullException($"`FilePath` cannot be empty", (Exception?)null));
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
                            return Result.Fail(new OperationCanceledException());

                        case TaskDialogResult.Yes:
                            break;

                        case TaskDialogResult.No:
                            continue;
                            break;

                        default:
                            return Result.Fail(new InvalidOperationException());
                    }
                }
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, fileText);
        }
        return Result.Succeed;

        static TaskDialogResult askToSkipOrReplace(string? title, string filePath)
        {
            var skipButton = ButtonInfo.New("&Skip", (o, e) => setResp(o, e, TaskDialogResult.No), isDefault: true).ToButton();
            var replButton = ButtonInfo.New("&Replace", (o, e) => setResp(o, e, TaskDialogResult.Yes)).ToButton();
            var replAllButton = ButtonInfo.New("Replace &all", (o, e) => setResp(o, e, TaskDialogResult.Ok), useElevationIcon: true).ToButton();
            var cancelButton = ButtonInfo.New("&Cancel", (o, e) => setResp(o, e, TaskDialogResult.Cancel)).ToButton();
            return MsgBox2.AskWithWarn($"The destination already has a file named \"{Path.GetFileName(filePath)}\".", text: title, $"Replace or Skip Files", controls: [skipButton, replButton, replAllButton, cancelButton]);

            static void setResp(object? o, EventArgs e, TaskDialogResult res) => MsgBox2.GetOnButtonClick(o, e).Parent.Close(res);
        }
    }
}
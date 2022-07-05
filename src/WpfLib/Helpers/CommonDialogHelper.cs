using System.Windows.Forms;

namespace Library.Wpf.Helpers;

public static class CommonDialogHelper
{
    public static (bool isOk, string FileName) Save(string? fileName = null, string? DefaultExt = null, string? Filter = null)
    {
        var dialog = new SaveFileDialog
        {
            FileName = fileName,
            DefaultExt = DefaultExt,
            Filter = Filter
        };
        var result = dialog.ShowDialog();
        return (result == DialogResult.OK, dialog.FileName ?? string.Empty);
    }
}
using System.Windows.Controls;

namespace Library.Wpf.Dialogs;

public class UserControlDialog
{
    private readonly UserControlDialogBox _userControlDialogBox;

    public UserControlDialog(in UserControl userControl) =>
        this._userControlDialogBox = new() { UserControl = userControl };

    public static UserControlDialog Create(in UserControl userControl) =>
        new(userControl);

    public UserControlDialog SetTile(in string title) =>
        this.Fluent(this._userControlDialogBox.DialogTitle = title);

    public UserControlDialog SetOkEnabled(in bool okEnabled) =>
        this.Fluent(this._userControlDialogBox.IsOkEnabled = okEnabled);

    public UserControlDialog SetCancelEnabled(in bool cancelEnabled) =>
        this.Fluent(this._userControlDialogBox.IsCancelEnabled = cancelEnabled);

    public UserControlDialog SetOwner(in Window owner) =>
        this.Fluent(this._userControlDialogBox.Owner = owner);

    public UserControlDialog SetPrompt(string? prompt) =>
        this.Fluent(this._userControlDialogBox.Prompt = prompt);

    public bool? Show() =>
        this._userControlDialogBox.ShowDialog();
}

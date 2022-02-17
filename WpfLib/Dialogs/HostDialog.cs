using Library.Wpf.Bases;

namespace Library.Wpf.Dialogs;

public class HostDialog
{
    private readonly HostDialogBox _dialogBox;

    public HostDialog(in PageBase source) =>
        this._dialogBox = new() { ClientUi = source };

    public static HostDialog Create(in PageBase source) =>
        new(source);

    public HostDialog SetTile(in string title) =>
        this.Fluent(this._dialogBox.DialogTitle = title);

    public HostDialog SetOkEnabled(in bool okEnabled) =>
        this.Fluent(this._dialogBox.IsOkEnabled = okEnabled);

    public HostDialog SetCancelEnabled(in bool cancelEnabled) =>
        this.Fluent(this._dialogBox.IsCancelEnabled = cancelEnabled);

    public HostDialog SetOwner(in Window owner) =>
        this.Fluent(this._dialogBox.Owner = owner);

    public HostDialog SetPrompt(string? prompt) =>
        this.Fluent(this._dialogBox.Prompt = prompt);

    public bool? Show() =>
        this._dialogBox.ShowDialog();
}

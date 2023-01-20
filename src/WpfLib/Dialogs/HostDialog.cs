using Library.Results;
using Library.Wpf.Bases;
using Library.Wpf.Pages;

namespace Library.Wpf.Dialogs;

public class HostDialog
{
    private readonly HostDialogBox _dialogBox;

    private HostDialog(in LibPageBase source)
        => this._dialogBox = new() { ClientUi = source, DialogTitle = source?.Title };

    public static HostDialog Create(in LibPageBase source)
        => new(source);

    public static HostDialog Create<TPage>() where TPage : LibPageBase, new()
        => new(new TPage());

    public static bool ShowDialog(object content, string title, string prompt, Func<LibPageBase, Result> onValidation)
    {
        var hostPage = new HostPage
        {
            Content = content
        };
        var result = Create(hostPage)
            .SetOwnerToDefault()
            .SetTile(title)
            .SetPrompt(prompt)
            .SetValidation(onValidation)
            .Show()
            .IsTrue()
            .With(_ => hostPage.Content = default);
        return result;
    }

    public HostDialog OnLoad(Action action)
        => this.Fluent(() => this._dialogBox.Loaded += (_, _) => action?.Invoke());

    public HostDialog SetCancelEnabled(in bool cancelEnabled)
        => this.Fluent(this._dialogBox.IsCancelEnabled = cancelEnabled);

    public HostDialog SetDirection(FlowDirection direction)
        => this.Fluent(this._dialogBox.Direction = direction);

    public HostDialog SetOkEnabled(in bool okEnabled)
        => this.Fluent(this._dialogBox.IsOkEnabled = okEnabled);

    public HostDialog SetOwner(in Window owner)
        => this.Fluent(this._dialogBox.Owner = owner);

    public HostDialog SetOwnerToDefault()
        => this.SetOwner(Application.Current.MainWindow);

    public HostDialog SetPrompt(string? prompt)
        => this.Fluent(this._dialogBox.Prompt = prompt);

    public HostDialog SetTile(in string title)
        => this.Fluent(this._dialogBox.DialogTitle = title);

    /// <summary>
    /// Sets the validation. <br/> If set, the dialog will use it. <br/> Else if not, the dialog
    /// will try to cast the page to <see cref="Library.Validations.IValidator"/> interfaces
    /// family. <br/> Otherwise no validation will be applied.
    /// </summary>
    /// <param name="onValidation">The OnValidation function.</param>
    public HostDialog SetValidation(Func<LibPageBase, Result> onValidation)
        => this.Fluent(this._dialogBox.OnValidate = onValidation);

    public bool? Show()
        => this._dialogBox.ShowDialog();
}
using System.ComponentModel;
using Library.Validations;
//using ClientType = System.Windows.Controls.UserControl;
using ClientType = Library.Wpf.Bases.PageBase;

namespace Library.Wpf.Dialogs;

/// <summary>
/// Interaction logic for UserControlDialogBox.xaml
/// </summary>
internal partial class HostDialogBox
{
    public string? DialogTitle
    {
        get => (string?)this.GetValue(DialogTitleProperty);
        set => this.SetValue(DialogTitleProperty, value);
    }
    public static readonly DependencyProperty DialogTitleProperty = ControlHelper.GetDependencyProperty<string?, HostDialogBox>(nameof(DialogTitle));

    public ClientType? ClientUi
    {
        get => (ClientType)this.GetValue(ClientUiProperty);
        set => this.SetValue(ClientUiProperty, value);
    }
    public static readonly DependencyProperty ClientUiProperty = ControlHelper.GetDependencyProperty<ClientType?, HostDialogBox>(nameof(ClientUi), onPropertyChanged: (s, _) =>
 {
     if (s.ClientUi is INotifyPropertyChanged x)
     {
         x.PropertyChanged += (_, _) => s.ValidationErrorText = null;
     }
 });

    public bool IsOkEnabled
    {
        get => (bool)this.GetValue(IsOkEnabledProperty);
        set => this.SetValue(IsOkEnabledProperty, value);
    }
    public static readonly DependencyProperty IsOkEnabledProperty = ControlHelper.GetDependencyProperty<bool, HostDialogBox>(nameof(IsOkEnabled), defaultValue: true);

    public bool IsCancelEnabled
    {
        get => (bool)this.GetValue(IsCancelEnabledProperty);
        set => this.SetValue(IsCancelEnabledProperty, value);
    }
    public static readonly DependencyProperty IsCancelEnabledProperty = ControlHelper.GetDependencyProperty<bool, HostDialogBox>(nameof(IsCancelEnabled), defaultValue: true);

    public string? Prompt
    {
        get => (string?)this.GetValue(PromptProperty);
        set => this.SetValue(PromptProperty, value);
    }
    public static readonly DependencyProperty PromptProperty = ControlHelper.GetDependencyProperty<string?, HostDialogBox>(nameof(Prompt));

    public string? ValidationErrorText
    {
        get => (string?)this.GetValue(ValidationErrorTextProperty);
        set => this.SetValue(ValidationErrorTextProperty, value);
    }
    public static readonly DependencyProperty ValidationErrorTextProperty = ControlHelper.GetDependencyProperty<string?, HostDialogBox>(nameof(ValidationErrorText));

    public HostDialogBox() =>
        this.InitializeComponent();

    private async void OkButton_Click(object sender, RoutedEventArgs e)
    {
        var validationResult = this.ClientUi switch
        {
            IValidatable validatable => validatable.Validate(),
            IAsyncValidatable asyncValidatable => await asyncValidatable.ValidateAsync(),
            _ => null,
        };
        if (validationResult?.IsSucceed ?? true)
        {
            this.DialogResult = true;
            this.Close();
        }
        else
        {
            this.ValidationErrorText = validationResult?.Message;
        }
    }
}

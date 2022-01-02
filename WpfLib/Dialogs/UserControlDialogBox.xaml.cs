using System.ComponentModel;
using System.Windows.Controls;
using Library.Validations;

namespace Library.Wpf.Dialogs;

/// <summary>
/// Interaction logic for UserControlDialogBox.xaml
/// </summary>
internal partial class UserControlDialogBox : Window
{
    public string? DialogTitle
    {
        get => (string?)this.GetValue(DialogTitleProperty);
        set => this.SetValue(DialogTitleProperty, value);
    }
    public static readonly DependencyProperty DialogTitleProperty = ControlHelper.GetDependencyProperty<string?, UserControlDialogBox>(nameof(DialogTitle));

    public UserControl? UserControl
    {
        get => (UserControl)this.GetValue(UserControlProperty);
        set => this.SetValue(UserControlProperty, value);
    }
    public static readonly DependencyProperty UserControlProperty = ControlHelper.GetDependencyProperty<UserControl?, UserControlDialogBox>(nameof(UserControl),
        onPropertyChanged: (s, _) =>
        {
            if (s.UserControl is INotifyPropertyChanged x)
            {
                x.PropertyChanged += (_, _) => s.ValidationErrorText = null;
            }
        });
    public bool IsOkEnabled
    {
        get => (bool)this.GetValue(IsOkEnabledProperty);
        set => this.SetValue(IsOkEnabledProperty, value);
    }
    public static readonly DependencyProperty IsOkEnabledProperty = ControlHelper.GetDependencyProperty<bool, UserControlDialogBox>(nameof(IsOkEnabled), defaultValue: true);

    public bool IsCancelEnabled
    {
        get => (bool)this.GetValue(IsCancelEnabledProperty);
        set => this.SetValue(IsCancelEnabledProperty, value);
    }
    public static readonly DependencyProperty IsCancelEnabledProperty = ControlHelper.GetDependencyProperty<bool, UserControlDialogBox>(nameof(IsCancelEnabled), defaultValue: true);

    public string? Prompt
    {
        get => (string?)this.GetValue(PromptProperty);
        set => this.SetValue(PromptProperty, value);
    }
    public static readonly DependencyProperty PromptProperty = ControlHelper.GetDependencyProperty<string?, UserControlDialogBox>(nameof(Prompt));

    public string? ValidationErrorText
    {
        get => (string?)this.GetValue(ValidationErrorTextProperty);
        set => this.SetValue(ValidationErrorTextProperty, value);
    }
    public static readonly DependencyProperty ValidationErrorTextProperty = ControlHelper.GetDependencyProperty<string?, UserControlDialogBox>(nameof(ValidationErrorText));

    public UserControlDialogBox() =>
        this.InitializeComponent();

    private async void OkButton_Click(object sender, RoutedEventArgs e)
    {
        var validationResult = this.UserControl switch
        {
            IValidatable validatable => validatable.IsValid(),
            IAsyncValidatable asyncValidatable => await asyncValidatable.IsValidAsync(),
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

using System.ComponentModel;

using Library.Results;
using Library.Wpf.Helpers;
//using ClientType = System.Windows.Controls.UserControl;
using ClientType = Library.Wpf.Bases.LibPageBase;

namespace Library.Wpf.Dialogs;

/// <summary>
/// Interaction logic for UserControlDialogBox.xaml
/// </summary>
internal partial class HostDialogBox
{
    public Func<ClientType, Result>? OnValidate { get; set; }
    public string? DialogTitle
    {
        get => (string?)this.GetValue(DialogTitleProperty);
        set => this.SetValue(DialogTitleProperty, value);
    }
    public static readonly DependencyProperty DialogTitleProperty = ControlHelper.GetDependencyProperty<string?, HostDialogBox>(nameof(DialogTitle));

    public FlowDirection? Direction
    {
        get => (FlowDirection?)this.GetValue(DirectionProperty);
        set => this.SetValue(DirectionProperty, value);
    }
    public static readonly DependencyProperty DirectionProperty = ControlHelper.GetDependencyProperty<FlowDirection?, HostDialogBox>(nameof(Direction));

    public ClientType? ClientUi
    {
        get => (ClientType)this.GetValue(ClientUiProperty);
        set => this.SetValue(ClientUiProperty, value);
    }
    public static readonly DependencyProperty ClientUiProperty = ControlHelper.GetDependencyProperty<ClientType?, HostDialogBox>(nameof(ClientUi), onPropertyChanged: (s, _) =>
    {
        if (s.ClientUi is not null)
        {
            s.HostFrame.Navigate(s.ClientUi);
        }
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

    public HostDialogBox()
        => this.InitializeComponent();

    private async void OkButton_Click(object sender, RoutedEventArgs e)
    {
        var validation = this.OnValidate is not null
            ? this.OnValidate(this.ClientUi!)
            : this.ClientUi switch
            {
                IValidatable validatable => validatable.Validate(),
                IAsyncValidatable asyncValidatable => await asyncValidatable.ValidateAsync(),
                _ => Result.Empty,
            };
        validation.ThrowOnFail(this.Title);
        this.DialogResult = true;
        this.Close();
        //if (!validation.IsSucceed)
        //{
        //    this.ValidationErrorText = validation.Message;
        //}
        //else
        //{
        //    this.DialogResult = true;
        //    this.Close();
        //}
    }

    private void Me_Loaded(object sender, RoutedEventArgs e)
    {

    }
}

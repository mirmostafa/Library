namespace Library.Wpf.Dialogs;

/// <summary>
/// Interaction logic for ConnectionStringDialogBox.xaml
/// </summary>
public partial class ConnectionStringDialogBox : Window
{
    public ConnectionStringDialogBox()
        => this.InitializeComponent();

    public static ConnectionStringDialogBox New()
        => new();

    public ConnectionStringDialogBox SetTitle(string? title)
        => this.With(() => this.Title = title ?? "Add Connection");
}
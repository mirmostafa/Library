using System.Data.SqlClient;
using Library.Data.SqlServer.Dynamics;
using Library.Exceptions.Validations;
using Library.Wpf.Dialogs;

namespace Library.Wpf.Internals.Dialogs;
/// <summary>
///     Interaction logic for ConnectionStringDialog.xaml
/// </summary>
public partial class ConnectionStringDialog
{
    private bool IsWorking
    {
        get => (bool)this.GetValue(IsWorkingProperty);
        set => this.SetValue(IsWorkingProperty, value);
    }
    public static readonly DependencyProperty IsWorkingProperty = ControlHelper.GetDependencyProperty<bool, ConnectionStringDialog>(nameof(IsWorking));

    private ConnectionStringDialog() => this.InitializeComponent();

    public static (bool? IsOk, string? ConnectionString) ShowDlg(string? connectionString = null, Window? owner = null)
    {
        var dlg = new ConnectionStringDialog { ConnectionString = connectionString, Owner = owner };
        return (dlg.ShowDialog(), dlg.ShowDialog() is true ? dlg.ConnectionString : connectionString);
    }

    public string? ConnectionString
    {
        get
        {
            var scsb = this.ParseConnectionString(true);
            return scsb.ConnectionString;
        }
        init
        {
            var scsb = new SqlConnectionStringBuilder(value);
            this.ServersComboBox.Text = scsb.DataSource;
            this.AuthWindowsRadioButton.IsChecked = scsb.IntegratedSecurity;
            this.UserNameTextBox.Text = scsb.UserID;
            this.PasswordTextBox.Password = scsb.Password;
            this.DbsComboBox.Text = scsb.InitialCatalog;
            this.TimeoutTextBox.Text = scsb.ConnectTimeout.ToString();
        }
    }

    public bool ValidateResult { get; init; }

    private SqlConnectionStringBuilder ParseConnectionString(bool full)
    {
        var scsb = new SqlConnectionStringBuilder();
        if (!this.ServersComboBox.Text.IsNullOrEmpty())
        {
            scsb.DataSource = this.ServersComboBox.Text;
        }

        scsb.IntegratedSecurity = this.AuthWindowsRadioButton.IsChecked ?? false;
        if ((this.AuthUserPassRadioButton.IsChecked ?? false) && !this.UserNameTextBox.Text.IsNullOrEmpty())
        {
            scsb.UserID = this.UserNameTextBox.Text;
        }

        if ((this.AuthUserPassRadioButton.IsChecked ?? false) && !this.PasswordTextBox.Password.IsNullOrEmpty() && (this.SavePassCheckBox.IsChecked ?? false))
        {
            scsb.Password = this.PasswordTextBox.Password;
        }

        if (full && !this.DbsComboBox.Text.IsNullOrEmpty())
        {
            scsb.InitialCatalog = this.DbsComboBox.Text;
        }

        if (this.TimeoutTextBox.Text.IsNumber())
        {
            scsb.ConnectTimeout = this.TimeoutTextBox.Text.ToInt();
        }

        return scsb;
    }

    private async Task DoValidationAsync(bool full)
    {
        _ = this.ConnectionString.NotNull(() => new ValidationException("Please fill the form"));
        //if (this.ValidateResult)
        {
            var ex = await AdoHelper.CheckConnectionStringAsync(this.ConnectionString);
            if (ex is not null)
            {
                throw ex;
            }
        }
    }

    private void ServersComboBox_OnDropDownOpened(object sender, EventArgs e)
    {
        if (this.ServersComboBox.Items.Count == 0)
        {
            this.RefreshServers();
        }
    }

    private async void RefreshServers()
    {
        if (!this.RefreshButton.IsEnabled)
        {
            return;
        }

        this.RefreshButton.IsEnabled = false;
        var servers = await Task.Run(() => Server.Servers.Select(server => server.Name).ToArray());
        this.ServersComboBox.ItemsSource = servers;
        this.RefreshButton.IsEnabled = true;
    }

    private void RefreshButton_OnClick(object sender, RoutedEventArgs e) => this.RefreshServers();
    private void AuthWindowsRadioButton_OnChecked(object sender, RoutedEventArgs e) => this.UpdateUi();
    private void AuthUserPassRadioButton_OnChecked(object sender, RoutedEventArgs e) => this.UpdateUi();
    private void Window_OnLoaded(object sender, RoutedEventArgs e) => this.UpdateUi();

    private void UpdateUi()
    {
        if (this.SelectDbGroupBox == null)
        {
            return;
        }

        this.SelectDbGroupBox.IsEnabled = (this.AuthWindowsRadioButton.IsChecked ?? false) || !this.UserNameTextBox.Text.IsNullOrEmpty();
        this.UserInfoPanel.IsEnabled = this.AuthUserPassRadioButton.IsChecked ?? false;
    }

    private void UserNameTextBoxText_OnChanged(object sender, TextChangedEventArgs e) => this.UpdateUi();

    private async void DbsComboBoxDropDown_OnOpened(object sender, EventArgs e)
    {
        await this.DoValidationAsync(false);
        this.DbsComboBox.Items.Clear();
        var dbs = Database.GetDatabases(this.ParseConnectionString(false).ConnectionString).Select(db => db.Name).OrderBy(_ => _).ToArray();
        foreach (var db in dbs)
        {
            _ = this.DbsComboBox.Items.Add(db);
        }
    }

    private async void TestConnectionButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.IsWorking = true;
        try
        {
            await this.DoValidationAsync(true);
            MsgBox2.Inform("Connection is OK.", window: this);
        }
        catch (Exception ex)
        {
            _ = MsgBox2.Exception(ex, window: this);
        }
        finally
        {
            this.IsWorking = false;
        }
    }

    private async void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        await this.DoValidationAsync(true);

        this.DialogResult = true;
        this.Close();
    }
}
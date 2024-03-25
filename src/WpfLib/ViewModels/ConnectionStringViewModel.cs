using Microsoft.Data.SqlClient;

using Library.ComponentModel;
using Library.Wpf.Bases;
using Library.Wpf.Markers;

namespace Library.Wpf.ViewModels;

[ViewModel]
internal class ConnectionStringViewModel : NotifyPropertyChanged, IViewModel
{
    private ConnectionStringAuthenticationType _authenticationType;
    private string? _databaseName;
    private string? _password;
    private string? _serverName;
    private int? _timeout;
    private string? _userName;

    public ConnectionStringAuthenticationType AuthenticationType { get => this._authenticationType; set => this.SetProperty(ref this._authenticationType, value); }

    public string? DatabaseName { get => this._databaseName; set => this.SetProperty(ref this._databaseName, value); }

    public string? Password { get => this._password; set => this.SetProperty(ref this._password, value); }

    public string? ServerName { get => this._serverName; set => this.SetProperty(ref this._serverName, value); }

    public int? Timeout { get => this._timeout; set => this.SetProperty(ref this._timeout, value); }

    public string? UserName { get => this._userName; set => this.SetProperty(ref this._userName, value); }

    public static ConnectionStringViewModel FromConnectionString(string connectionString)
    {
        var engine = new SqlConnectionStringBuilder(connectionString);
        return FromData(
            engine.IntegratedSecurity ? ConnectionStringAuthenticationType.SqlServer : ConnectionStringAuthenticationType.Windows,
            engine.DataSource,
            engine.UserID,
            engine.Password,
            engine.InitialCatalog,
            engine.ConnectTimeout);
    }

    public static ConnectionStringViewModel FromData(
        ConnectionStringAuthenticationType authenticationType = ConnectionStringAuthenticationType.Windows,
        string? serverName = null,
        string? userName = null,
        string? password = null,
        string? databaseName = null,
        int? timeout = null)
    {
        var result = new ConnectionStringViewModel
        {
            AuthenticationType = authenticationType,
            Password = password,
            ServerName = serverName,
            UserName = userName,
            DatabaseName = databaseName,
            Timeout = timeout,
        };

        return result;
    }

    public static ConnectionStringViewModel FromSqlConnectionStringBuilder(SqlConnectionStringBuilder builder)
        => FromConnectionString(builder.ArgumentNotNull().ConnectionString);
}

internal enum ConnectionStringAuthenticationType
{
    Windows,
    SqlServer
}
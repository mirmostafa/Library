using System.Data.SqlClient;
using Library.DesignPatterns.Markers;

namespace Library.Data.SqlServer.Builders;

[Fluent]
public class ConnectionStringBuilder
{
    private readonly SqlConnectionStringBuilder _builder;

    private ConnectionStringBuilder()
        => this._builder = new();

    private ConnectionStringBuilder(string connectionString)
        => this._builder = new(connectionString);

    public static ConnectionStringBuilder Create()
        => new();

    public static ConnectionStringBuilder Create(string connectionString)
        => new(connectionString);

    public ConnectionStringBuilder WithDataBase(string value)
        => this.Fluent(() => this._builder.DataSource = value);

    public ConnectionStringBuilder AsUserName(string value)
        => this.Fluent(() => this._builder.UserID = value);

    public ConnectionStringBuilder WithPassword(string value)
        => this.Fluent(() => this._builder.Password = value);

    public ConnectionStringBuilder ForServer(string value)
        => this.Fluent(() => this._builder.DataSource = value);

    public ConnectionStringBuilder IsIntegratedSecurity(bool value)
        => this.Fluent(() => this._builder.IntegratedSecurity = value);

    public ConnectionStringBuilder ShouldPersistSecurityInfo(bool value)
        => this.Fluent(() => this._builder.PersistSecurityInfo = value);

    public ConnectionStringBuilder WithConnectTimeout(int value)
        => this.Fluent(() => this._builder.ConnectTimeout = value);

    public ConnectionStringBuilder AttachDbFilename(string value)
        => this.Fluent(() => this._builder.AttachDBFilename = value);

    public ConnectionStringBuilder ApplicationName(string value)
        => this.Fluent(() => this._builder.ApplicationName = value);

    public ConnectionStringBuilder HasMultipleActiveResultSets(bool value)
        => this.Fluent(() => this._builder.MultipleActiveResultSets = value);

    public ConnectionStringBuilder IsEncrypted(bool value)
        => this.Fluent(() => this._builder.Encrypt = value);

    public ConnectionStringBuilder IsUserInstance(bool value)
        => this.Fluent(() => this._builder.UserInstance = value);

    public ConnectionStringBuilder IsReadOnly(bool value)
        => this.Fluent(() => this._builder.ApplicationIntent = value ? ApplicationIntent.ReadOnly : ApplicationIntent.ReadWrite);

    public string Build()
        => this._builder.ConnectionString;

    public void Validate() { }

    public static void Validate(string connectionString)
        => Create(connectionString).Validate();

    public static string Build(string server,
        string? userName = null,
        string? password = null,
        string? database = null,
        bool? isIntegratedSecurity = null,
        bool? shouldPersistSecurityInfo = null,
        int? connectTimeout = 30,
        string? attachDbFilename = null,
        string? applicationName = null,
        bool? hasMultipleActiveResultSets = null,
        bool? isEncrypt = null,
        bool? isUserInstance = null,
        bool? isReadOnly = null)
        => Create()
            .ForServer(server)
            .IfTrue(!userName.IsNullOrEmpty(), builder => builder.AsUserName(userName!))
            .IfTrue(!password.IsNullOrEmpty(), builder => builder.WithPassword(password!))
            .IfTrue(!database.IsNullOrEmpty(), builder => builder.WithDataBase(database!))
            .IfTrue(isIntegratedSecurity.HasValue, builder => builder.IsIntegratedSecurity(isIntegratedSecurity!.Value))
            .IfTrue(shouldPersistSecurityInfo.HasValue, builder => builder.ShouldPersistSecurityInfo(shouldPersistSecurityInfo!.Value))
            .IfTrue(connectTimeout.HasValue, builder => builder.WithConnectTimeout(connectTimeout!.Value))
            .IfTrue(!attachDbFilename.IsNullOrEmpty(), builder => builder.AttachDbFilename(attachDbFilename!))
            .IfTrue(!applicationName.IsNullOrEmpty(), builder => builder.ApplicationName(applicationName!))
            .IfTrue(hasMultipleActiveResultSets.HasValue, builder => builder.HasMultipleActiveResultSets(hasMultipleActiveResultSets!.Value))
            .IfTrue(isEncrypt.HasValue, builder => builder.IsEncrypted(isEncrypt!.Value))
            .IfTrue(isUserInstance.HasValue, builder => builder.IsUserInstance(isUserInstance!.Value))
            .IfTrue(isReadOnly.HasValue, builder => builder.IsReadOnly(isReadOnly!.Value))
            .Build();
}
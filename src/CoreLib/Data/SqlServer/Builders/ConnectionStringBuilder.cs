using System.Data.SqlClient;

using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.Data.SqlServer.Builders;

[Fluent]
public class ConnectionStringBuilder : IValidatable<ConnectionStringBuilder>, IBuilder<string>
{
    private readonly SqlConnectionStringBuilder _builder;

    private ConnectionStringBuilder(string? connectionString = null)
        => this._builder = connectionString.IsNullOrEmpty() ? new() : new(connectionString);

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
            .Server(server).Fluent()
            .IfTrue(!userName.IsNullOrEmpty(), builder => builder.UserName(userName!))
            .IfTrue(!password.IsNullOrEmpty(), builder => builder.Password(password!))
            .IfTrue(!database.IsNullOrEmpty(), builder => builder.DataBase(database!))
            .IfTrue(isIntegratedSecurity.HasValue, builder => builder.IsIntegratedSecurity(isIntegratedSecurity!.Value))
            .IfTrue(shouldPersistSecurityInfo.HasValue, builder => builder.ShouldPersistSecurityInfo(shouldPersistSecurityInfo!.Value))
            .IfTrue(connectTimeout.HasValue, builder => builder.ConnectTimeout(connectTimeout!.Value))
            .IfTrue(!attachDbFilename.IsNullOrEmpty(), builder => builder.AttachDbFilename(attachDbFilename!))
            .IfTrue(!applicationName.IsNullOrEmpty(), builder => builder.ApplicationName(applicationName!))
            .IfTrue(hasMultipleActiveResultSets.HasValue, builder => builder.MultipleActiveResultSets(hasMultipleActiveResultSets!.Value))
            .IfTrue(isEncrypt.HasValue, builder => builder.IsEncrypted(isEncrypt!.Value))
            .IfTrue(isUserInstance.HasValue, builder => builder.IsUserInstance(isUserInstance!.Value))
            .IfTrue(isReadOnly.HasValue, builder => builder.IsReadOnly(isReadOnly!.Value)).GetValue()
            .BuildAll();

    public static ConnectionStringBuilder Create()
        => new();

    public static ConnectionStringBuilder Create(string connectionString)
        => new(connectionString);

    public static Result<ConnectionStringBuilder> Validate(string connectionString)
        => Create(connectionString).Validate();

    public ConnectionStringBuilder ApplicationName(string value)
        => this.Fluent(() => this._builder.ApplicationName = value);

    public ConnectionStringBuilder AttachDbFilename(string value)
        => this.Fluent(() => this._builder.AttachDBFilename = value);

    public string BuildAll()
        => this._builder.ConnectionString;

    public ConnectionStringBuilder ConnectTimeout(int value)
        => this.Fluent(() => this._builder.ConnectTimeout = value);

    public ConnectionStringBuilder DataBase(string value)
        => this.Fluent(() => this._builder.DataSource = value);

    public ConnectionStringBuilder IsEncrypted(bool value)
        => this.Fluent(() => this._builder.Encrypt = value);

    public ConnectionStringBuilder IsIntegratedSecurity(bool value)
        => this.Fluent(() => this._builder.IntegratedSecurity = value);

    public ConnectionStringBuilder IsReadOnly(bool value)
        => this.Fluent(() => this._builder.ApplicationIntent = value ? ApplicationIntent.ReadOnly : ApplicationIntent.ReadWrite);

    public ConnectionStringBuilder IsUserInstance(bool value)
        => this.Fluent(() => this._builder.UserInstance = value);

    public ConnectionStringBuilder MultipleActiveResultSets(bool value)
        => this.Fluent(() => this._builder.MultipleActiveResultSets = value);

    public ConnectionStringBuilder Password(string value)
        => this.Fluent(() => this._builder.Password = value);

    public ConnectionStringBuilder Server(string value)
        => this.Fluent(() => this._builder.DataSource = value);

    public ConnectionStringBuilder ShouldPersistSecurityInfo(bool value)
        => this.Fluent(() => this._builder.PersistSecurityInfo = value);

    public ConnectionStringBuilder UserName(string value)
        => this.Fluent(() => this._builder.UserID = value);

    public Result<ConnectionStringBuilder> Validate()
        => Result<ConnectionStringBuilder>.New(this);
}
using System.Data.SqlClient;
using Library.DesignPatterns.Markers;

namespace Library.Data.SqlServer;
[Fluent]
public class ConnectionStringBuilder
{
    private readonly SqlConnectionStringBuilder _builder = new();

    public static ConnectionStringBuilder Create() => new();
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
    public string Build() => this._builder.ConnectionString;

    public static string Build(string dataSource,
        string? userId = null,
        string? password = null,
        string? initialCatalog = null,
        bool? integratedSecurity = null,
        bool? persistSecurityInfo = null,
        int? connectTimeout = 30,
        string? attachDbFilename = null,
        string? applicationName = null,
        bool? multipleActiveResultSets = null,
        bool? encrypt = null,
        bool? userInstance = null) =>
            Create()
           .ForServer(dataSource)
           .IfTrue(!userId.IsNullOrEmpty(), builder => builder.AsUserName(userId!))
           .IfTrue(!initialCatalog.IsNullOrEmpty(), builder => builder.WithDataBase(initialCatalog!))
           .IfTrue(integratedSecurity.HasValue, builder => builder.IsIntegratedSecurity(integratedSecurity!.Value))
           .IfTrue(persistSecurityInfo.HasValue, builder => builder.ShouldPersistSecurityInfo(persistSecurityInfo!.Value))
           .IfTrue(connectTimeout.HasValue, builder => builder.WithConnectTimeout(connectTimeout!.Value))
           .IfTrue(!attachDbFilename.IsNullOrEmpty(), builder => builder.WithDataBase(attachDbFilename!))
           .IfTrue(!applicationName.IsNullOrEmpty(), builder => builder.WithDataBase(applicationName!))
           .IfTrue(multipleActiveResultSets.HasValue, builder => builder.HasMultipleActiveResultSets(multipleActiveResultSets!.Value))
           .IfTrue(encrypt.HasValue, builder => builder.IsEncrypted(encrypt!.Value))
           .IfTrue(userInstance.HasValue, builder => builder.IsUserInstance(userInstance!.Value))
           .Build();
}
﻿using Microsoft.Data.SqlClient;

using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.Data.SqlServer.Builders;

/// <summary>
/// Fluent builder for creating SQL Server connection strings.
/// </summary>
[Fluent]
public sealed class ConnectionStringBuilder : IValidatable<ConnectionStringBuilder>, IBuilder<string>
{
    private readonly SqlConnectionStringBuilder _builder;

    /// <summary>
    /// Private constructor for creating a ConnectionStringBuilder.
    /// </summary>
    /// <param name="connectionString">Optional initial connection string.</param>
    private ConnectionStringBuilder(string? connectionString = null)
        => this._builder = connectionString.IsNullOrEmpty() ? new() : new(connectionString);

    /// <summary>
    /// Build a connection string with various optional parameters.
    /// </summary>
    /// <param name="server">The SQL Server instance name or IP address.</param>
    /// <param name="userName">The SQL Server login username.</param>
    /// <param name="password">The SQL Server login password.</param>
    /// <param name="database">The name of the initial database to connect to.</param>
    /// <param name="isIntegratedSecurity">Indicates whether integrated security should be used.</param>
    /// <param name="shouldPersistSecurityInfo">Indicates whether security info should be persisted.</param>
    /// <param name="connectTimeout">The connection timeout value.</param>
    /// <param name="attachDbFilename">The name of the primary database file.</param>
    /// <param name="applicationName">The application name to be used in SQL Server.</param>
    /// <param name="hasMultipleActiveResultSets">Indicates whether multiple active result sets are allowed.</param>
    /// <param name="isEncrypt">Indicates whether the connection should be encrypted.</param>
    /// <param name="isUserInstance">Indicates whether to use a user instance of SQL Server.</param>
    /// <param name="isReadOnly">Indicates whether the connection is read-only.</param>
    /// <returns>The constructed connection string.</returns>
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
            .Build();

    /// <summary>
    /// Create a new ConnectionStringBuilder without an initial connection string.
    /// </summary>
    public static ConnectionStringBuilder Create()
        => new();

    /// <summary>
    /// Create a new ConnectionStringBuilder with a provided initial connection string.
    /// </summary>
    /// <param name="connectionString">The initial connection string.</param>
    public static ConnectionStringBuilder Create(string connectionString)
        => new(connectionString);

    /// <summary>
    /// Validate a connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to validate.</param>
    /// <returns>A Result containing the validated ConnectionStringBuilder.</returns>
    public static Result<ConnectionStringBuilder> Validate(string? connectionString)
        => Create(connectionString.ArgumentNotNull()).Validate();

    /// <summary>
    /// Set the ApplicationName in the connection string.
    /// </summary>
    /// <param name="value">The ApplicationName value.</param>
    /// <returns>The updated ConnectionStringBuilder.</returns>
    public ConnectionStringBuilder ApplicationName(string value)
        => this.Fluent(() => this._builder.ApplicationName = value);

    /// <summary>
    /// Set the AttachDbFilename in the connection string.
    /// </summary>
    /// <param name="value">The AttachDbFilename value.</param>
    /// <returns>The updated ConnectionStringBuilder.</returns>
    public ConnectionStringBuilder AttachDbFilename(string value)
        => this.Fluent(() => this._builder.AttachDBFilename = value);

    /// <summary>
    /// Build the final connection string.
    /// </summary>
    /// <returns>The constructed connection string.</returns>
    public string Build()
        => this._builder.ConnectionString;

    // Set the ConnectTimeout in the connection string.
    public ConnectionStringBuilder ConnectTimeout(int value)
        => this.Fluent(() => this._builder.ConnectTimeout = value);

    // Set the DataSource (Server) in the connection string.
    public ConnectionStringBuilder DataBase(string value)
        => this.Fluent(() => this._builder.DataSource = value);

    // Set the Encrypt option in the connection string.
    public ConnectionStringBuilder IsEncrypted(bool value)
        => this.Fluent(() => this._builder.Encrypt = value);

    // Set the IntegratedSecurity option in the connection string.
    public ConnectionStringBuilder IsIntegratedSecurity(bool value)
        => this.Fluent(() => this._builder.IntegratedSecurity = value);

    // Set the ReadOnly or ReadWrite option in the connection string.
    public ConnectionStringBuilder IsReadOnly(bool value)
        => this.Fluent(() => this._builder.ApplicationIntent = value ? ApplicationIntent.ReadOnly : ApplicationIntent.ReadWrite);

    // Set the UserInstance option in the connection string.
    public ConnectionStringBuilder IsUserInstance(bool value)
        => this.Fluent(() => this._builder.UserInstance = value);

    // Set the MultipleActiveResultSets option in the connection string.
    public ConnectionStringBuilder MultipleActiveResultSets(bool value)
        => this.Fluent(() => this._builder.MultipleActiveResultSets = value);

    // Set the Password in the connection string.
    public ConnectionStringBuilder Password(string value)
        => this.Fluent(() => this._builder.Password = value);

    // Set the Server (DataSource) in the connection string.
    public ConnectionStringBuilder Server(string value)
        => this.Fluent(() => this._builder.DataSource = value);

    // Set the PersistSecurityInfo option in the connection string.
    public ConnectionStringBuilder ShouldPersistSecurityInfo(bool value)
        => this.Fluent(() => this._builder.PersistSecurityInfo = value);

    public ConnectionStringBuilder UserName(string value)
        => this.Fluent(() => this._builder.UserID = value);

    // Validate the connection string.
    public Result<ConnectionStringBuilder> Validate()
        => new(this);
}
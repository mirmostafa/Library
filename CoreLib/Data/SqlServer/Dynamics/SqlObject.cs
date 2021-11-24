﻿using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace Library.Data.SqlServer.Dynamics;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class SqlObject<TSqlObject, TOwner> : DynamicObject, ISqlObject
    where TSqlObject : SqlObject<TSqlObject, TOwner>
{
    private readonly string? _connectionString;

    protected SqlObject(TOwner owner, string name, string? schema = null, string? connectionString = null)
    {
        this.Owner = owner;
        this.Name = name;
        this._connectionString = connectionString;
        this.Schema = schema;
    }

    public string ConnectionString => new SqlConnectionStringBuilder(this._connectionString).ConnectionString;

    public virtual string Name { get; }

    public virtual TOwner Owner { get; }

    public string? Schema { get; }

    protected static IEnumerable<DataRow> GetDataRows(string connectionString, string tableName)
    {
        using var dataSet = GetSql(connectionString).FillByTableNames(tableName);
        return SqlObject<TSqlObject, TOwner>.GetRows(dataSet);
    }

    protected static IEnumerable<DataRow> GetQueryItems(string connectionString, string query)
    {
        using var dataSet = GetSql(connectionString).FillDataSet(query);
        return GetRows(dataSet);
    }

    protected static Sql GetSql(string connectionString) =>
        new(connectionString);

    private static IEnumerable<DataRow> GetRows(DataSet ds) => 
        ds.Dispose(ds.GetTables().FirstOrDefault()?.Dispose(t => t?.Select())) ?? Enumerable.Empty<DataRow>();

    public override string ToString() => 
        this.Schema.IsNullOrEmpty() ? this.Name : $"{this.Schema}.{this.Name}";

    protected IEnumerable<DataRow> GetDataRows(string query) => GetDataRows(this.ConnectionString, query);

    protected IEnumerable<DataRow> GetQueryItems(string query) => GetQueryItems(this.ConnectionString, query);

    protected Sql GetSql() => GetSql(this.ConnectionString);
}

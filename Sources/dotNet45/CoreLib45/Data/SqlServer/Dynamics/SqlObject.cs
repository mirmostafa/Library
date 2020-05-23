#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Helpers;
using Mohammad.Logging;

namespace Mohammad.Data.SqlServer.Dynamics
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SqlObject<TSqlObject, TOwner> : DynamicObject, ISqlObject, IExceptionHandlerContainer<Exception>
        where TSqlObject : SqlObject<TSqlObject, TOwner>
    {
        public static ExceptionHandling<Exception> CommonExceptionHandling
        {
            get => _CommonExceptionHandling ?? (_CommonExceptionHandling = new ExceptionHandling<Exception>());
            set => _CommonExceptionHandling = value;
        }
        //public ILogger Logger { get; set; } = new Logger();

        public string ConnectionString => new SqlConnectionStringBuilder(this._ConnectionString).ConnectionString;

        public ExceptionHandling<Exception> ExceptionHandling
        {
            get => this._ExceptionHandling ?? CommonExceptionHandling;
            set => this._ExceptionHandling = value;
        }

        public ILogger Logger { get; set; } = Logging.Logger.Empty;

        public virtual string Name { get; }

        public virtual TOwner Owner { get; }

        public string Schema { get; }

        protected SqlObject(TOwner owner, string name, string schema = null, string connectionString = null)
        {
            this.Owner = owner;
            this.Name = name;
            this._ConnectionString = connectionString;
            this.Schema = schema;
        }

        public override string ToString() => this.Schema.IsNullOrEmpty() ? this.Name : $"{this.Schema}.{this.Name}";

        protected IEnumerable<DataRow> GetDataRows(string query) => GetDataRows(this.ConnectionString, query);

        protected static IEnumerable<DataRow> GetDataRows(string connectionString, string tableName) => GetRows(GetSql(connectionString, null).FillByTableNames(tableName));

        protected IEnumerable<DataRow> GetQueryItems(string query) => GetQueryItems(this.ConnectionString, query);

        protected static IEnumerable<DataRow> GetQueryItems(string connectionString, string query) =>
            GetRows(GetSql(connectionString, null).FillDataSet(query));

        protected Sql GetSql() => GetSql(this.ConnectionString, this.Logger);
        protected static Sql GetSql(string connectionString, ILogger logger) => new Sql(connectionString) {Logger = logger};

        private static IEnumerable<DataRow> GetRows(DataSet ds) => ds.Dispose(ds.GetTables().FirstOrDefault()?.Dispose(t => t?.Select()));

        #region Fields

        private static ExceptionHandling<Exception> _CommonExceptionHandling;
        private readonly string _ConnectionString;
        private ExceptionHandling<Exception> _ExceptionHandling;

        #endregion
    }
}
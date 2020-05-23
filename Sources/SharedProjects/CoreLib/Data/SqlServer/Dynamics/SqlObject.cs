using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer.Dynamics
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SqlObject<TSqlObject, TOwner> : DynamicObject, ISqlObject, IExceptionHandlerContainer<Exception>
        where TSqlObject : SqlObject<TSqlObject, TOwner>
    {
        private readonly string _ConnectionString;
        private ExceptionHandling<Exception> _ExceptionHandling;
        private static ExceptionHandling<Exception> _CommonExceptionHandling;

        public string ConnectionString => new SqlConnectionStringBuilder(this._ConnectionString).ConnectionString;

        public static ExceptionHandling<Exception> CommonExceptionHandling
        {
            get { return _CommonExceptionHandling ?? (_CommonExceptionHandling = new ExceptionHandling<Exception>()); }
            set { _CommonExceptionHandling = value; }
        }

        public virtual TOwner Owner { get; private set; }

        protected SqlObject(TOwner owner, string name, string connectionstring)
        {
            this.Owner = owner;
            this.Name = name;
            this._ConnectionString = connectionstring;
        }

        protected IEnumerable<DataRow> GetDataRows(string query) => GetDataRows(this.ConnectionString, query);

        protected static IEnumerable<DataRow> GetDataRows(string connectionstring, string tablename)
            => GetRows(new Sql(connectionstring).FillByTableNames(tablename));

        protected IEnumerable<DataRow> GetQueryItems(string query) => GetQueryItems(this.ConnectionString, query);
        protected static IEnumerable<DataRow> GetQueryItems(string connectionstring, string query) => GetRows(new Sql(connectionstring).FillByQuery(query));

        private static IEnumerable<DataRow> GetRows(DataSet ds) => ds.Dispose(ds.GetTables().FirstOrDefault()?.Dispose(t => t?.Select()));

        public override string ToString() => this.Name;

        public ExceptionHandling<Exception> ExceptionHandling
        {
            get { return this._ExceptionHandling ?? CommonExceptionHandling; }
            set { this._ExceptionHandling = value; }
        }

        public virtual string Name { get; }
    }
}
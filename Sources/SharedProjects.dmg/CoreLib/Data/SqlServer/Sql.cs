using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Mohammad.Dynamic;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Logging;
using Mohammad.Threading.Tasks;

namespace Mohammad.Data.SqlServer
{
    public sealed class Sql : ILoggerContainer
    {
        private ILogger _Logger;

        public static readonly DateTime SqlDateTimeMaxValue = DateTime.Parse("12/31/9999");
        public static readonly DateTime SqlDateTimeMinValue = DateTime.Parse("1/1/1753 12:00:00 AM");

        public string ConnectionString { get; }

        public Sql(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));
            this.ConnectionString = connectionString;
        }

        public event EventHandler<LogEventArgs> Logging;
        public event EventHandler<LogEventArgs> Logged;
        private void Logger_OnLogging(object sender, LogEventArgs e) => this.OnLogged(e);
        private void Logger_OnLogged(object sender, LogEventArgs e) => this.OnLogging(e);

        public DataSet FillByTableNames(params string[] tablenames)
        {
            var result = this.FillByQuery(tablenames.Select(t => SqlStatementBuilder.CreateSelect(t)).Merge(Environment.NewLine));
            for (var i = 0; i < tablenames.Length; i++)
                result.Tables[i].TableName = tablenames[i];
            return result;
        }

        public dynamic GetDynamicTablesByTypes(params Type[] types)
        {
            dynamic result = new Expando();
            using (var ds = this.FillByTableNames(types.Select(t => t.Name).ToArray()))
                foreach (var type in types)
                {
                    var table = ds.Tables[type.Name];
                    var rows = new ArrayList();
                    foreach (DataRow row in table.Rows)
                    {
                        var constructor = type.GetConstructor(new Type[] {});
                        if (constructor == null)
                            continue;
                        var newRow = constructor.Invoke(null);
                        foreach (DataColumn column in table.Columns)
                        {
                            var value = row[column];
                            if (DBNull.Value.Equals(value))
                                value = null;
                            type.GetProperty(column.ColumnName).SetValue(newRow, value);
                        }
                        rows.Add(newRow);
                    }
                    result[type.Name] = rows;
                }
            return result;
        }

        public IEnumerable<dynamic> GetDynamicRowsByName(string tablename)
        {
            using (var ds = this.FillByTableNames(tablename))
            {
                var table = ds.Tables[tablename];
                {
                    var dynRows = new List<Expando>();
                    foreach (DataRow row in table.Rows)
                    {
                        var dynRow = new Expando();
                        foreach (DataColumn column in table.Columns)
                            dynRow[column.ColumnName] = row[column];
                        dynRows.Add(dynRow);
                    }
                    return dynRows.ToEnumerable();
                }
            }
        }

        public SqlDataReader ExecuteReader(string query)
        {
            this.Logger.Debug(query);
            return new SqlConnection(this.ConnectionString).ExecuteReader(query, behavior: CommandBehavior.CloseConnection);
        }

        public DataSet FillByQuery(string query)
        {
            this.Logger.Debug(query);
            using (var conn = new SqlConnection(this.ConnectionString))
            {
                this.Logger.Info(query);
                return conn.FillDataSet(query);
            }
        }

        public async Task<DataSet> FillByQueryAsync(string query) { return await Async.Run(() => this.FillByQuery(query)); }

        public SqlCommand GetCommand(string query)
        {
            var connection = new SqlConnection(this.ConnectionString);
            var result = connection.CreateCommand(query);
            result.Disposed += delegate
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            };
            return result;
        }

        public DataTable FillDataTable(string query, Action<SqlParameterCollection> fillParams = null)
        {
            this.Logger.Debug(query);
            var result = new DataTable();
            using (var command = this.GetCommand(query))
            {
                fillParams?.Invoke(command.Parameters);
                command.Connection.Open();
                result.Load(command.ExecuteReader());
                command.Connection.Close();
            }
            return result;
        }

        public async Task<DataTable> FillDataTableAsync(string query, Action<SqlParameterCollection> fillParams = null)
        {
            var result = new DataTable();
            this.Logger.Debug(query);
            var command = this.GetCommand(query);
            fillParams?.Invoke(command.Parameters);
            await Async.Run(() =>
            {
                try
                {
                    command.Connection.Open();
                    result.Load(command.ExecuteReader());
                }
                finally
                {
                    command.Clone();
                    command.Dispose();
                    command.Connection.Close();
                }
            });
            return result;
        }

        public object ExecuteScalar(string sql) => this.ExecuteScalar(sql, null);

        public object ExecuteScalar(string sql, Action<SqlParameterCollection> fillParams)
        {
            object result = null;
            this.TransactionalCommand(sql, cmd => result = cmd.ExecuteScalar(), fillParams);
            return result;
        }

        public int ExecuteNonQuery(string sql, Action<SqlParameterCollection> fillParams = null)
        {
            var result = 0;
            this.TransactionalCommand(sql, cmd => result = cmd.ExecuteNonQuery(), fillParams);
            return result;
        }

        public void TransactionalCommand(string cmdText, Action<SqlCommand> executer = null, Action<SqlParameterCollection> fillParams = null)
        {
            if (cmdText == null)
                throw new ArgumentNullException(nameof(cmdText));
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                using (var command = new SqlCommand(cmdText, connection, transaction))
                {
                    command.CommandTimeout = connection.ConnectionTimeout;
                    fillParams?.Invoke(command.Parameters);
                    try
                    {
                        this.Logger.Debug(cmdText);
                        if (executer != null)
                            executer(command);
                        else
                            command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public IEnumerable<T> Select<T>(string query, Func<SqlDataReader, T> rowFiller)
        {
            this.Logger.Debug(query);
            using (var conn = new SqlConnection(this.ConnectionString))
                return conn.Select(query, rowFiller).ToList();
        }

        public IEnumerable<T> Select<T>(string query, Func<IDataReader, T> convertor) where T : new()
        {
            this.Logger.Debug(query);
            using (var conn = new SqlConnection(this.ConnectionString))
                return conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection).Select(convertor);
        }

        public IEnumerable<T> Select<T>(string query, Func<T> creator)
        {
            this.Logger.Debug(query);
            using (var conn = new SqlConnection(this.ConnectionString))
                return conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection).Select(creator);
        }

        public IEnumerable<T> Select<T>(string query) where T : new()
        {
            this.Logger.Debug(query);
            using (var conn = new SqlConnection(this.ConnectionString))
                return conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection).Select<T>();
        }

        public IEnumerable<dynamic> Select(string query, Func<SqlDataReader, dynamic> rowFiller)
        {
            this.Logger.Debug(query);
            using (var conn = new SqlConnection(this.ConnectionString))
                return conn.Select(query, rowFiller).ToList();
        }

        public object ExecuteStoredProcedure(string spName, Action<SqlParameterCollection> fillParams = null)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
                return conn.ExecuteStoredProcedure(spName, fillParams);
        }

        private void OnLogging(LogEventArgs e)
        {
            var handler = this.Logging;
            handler?.Invoke(this, e);
        }

        private void OnLogged(LogEventArgs e)
        {
            var handler = this.Logged;
            handler?.Invoke(this, e);
        }

        public ILogger Logger
        {
            get { return this._Logger ?? (this.Logger = new Logger()); }
            set
            {
                if (this._Logger == value)
                    return;
                if (this._Logger != null)
                {
                    this._Logger.Logged -= this.Logger_OnLogged;
                    this._Logger.Logging -= this.Logger_OnLogging;
                }
                this._Logger = value;
                var logger = this._Logger;
                if (logger == null)
                    return;
                logger.Logged += this.Logger_OnLogged;
                logger.Logging += this.Logger_OnLogging;
            }
        }

        public object DefaultSender { get; } = "Sql";
    }
}
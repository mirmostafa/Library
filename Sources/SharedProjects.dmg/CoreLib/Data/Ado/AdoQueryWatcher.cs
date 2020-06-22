using System;
using System.Data;
using System.Data.SqlClient;
using Mohammad.Helpers;

namespace Mohammad.Data.Ado
{
    public class AdoQueryWatcher
    {
        private readonly string _Query;
        private static SqlDependency _Dependency;
        private static DataTable _TempTable;
        public string ConnectionString { get; }

        public AdoQueryWatcher(string connectionString, string query)
        {
            this.ConnectionString = connectionString;
            this._Query = query;
        }

        public void Start()
        {
            SqlDependency.Start(this.ConnectionString);
            if (_Dependency != null)
            {
                _Dependency.OnChange -= this.OnDependencyChange;
                _Dependency = null;
            }

            var connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            var command = new SqlCommand(this._Query, connection) {Notification = null};

            _Dependency = new SqlDependency(command);
            _Dependency.OnChange += this.OnDependencyChange;
            _TempTable?.Dispose();
            _TempTable = new DataTable();
            _TempTable.Load(command.ExecuteReader(CommandBehavior.CloseConnection));
            connection.Close();
        }

        public event EventHandler<SqlNotificationEventArgs> Changed;
        protected virtual async void OnChanged(SqlNotificationEventArgs e) { await this.Changed.RaiseAsync(this, e); }

        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.OnChanged(e);
            this.Start();
        }

        public void Stop() { SqlDependency.Stop(this.ConnectionString); }
    }
}
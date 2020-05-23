using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using Mohammad.Data.SqlServer.Dynamics.Collections;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer.Dynamics
{
    public class Server : DynamicObject, ISqlObject
    {
        private Databases _Databases;
        protected SqlConnectionStringBuilder ConnectionStringBuilder = new SqlConnectionStringBuilder();

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get
            {
                this.ConnectionStringBuilder.DataSource = this.Name;
                return this.ConnectionStringBuilder.ConnectionString;
            }
            set
            {
                this.ConnectionStringBuilder.ConnectionString = value;
                if (!this.ConnectionStringBuilder.DataSource.IsNullOrEmpty())
                    this.Name = this.ConnectionStringBuilder.DataSource;
                else
                    this.ConnectionStringBuilder.DataSource = this.Name;
            }
        }

        public Databases Databases => this._Databases ?? (this._Databases = this.GetDatabases());
        public string Version { get; protected set; }

        public static Servers Servers => new Servers(from row in SqlDataSourceEnumerator.Instance.GetDataSources().Select()
                                                     select
                                                     new Server
                                                     {
                                                         Name =
                                                             row.Field<string>("InstanceName").IsNullOrEmpty()
                                                                 ? row.Field<string>("ServerName")
                                                                 : $@"{row.Field<string>("ServerName")}\{row.Field<string>("InstanceName")}",
                                                         Version = row.Field<string>("Version")
                                                     });

        protected Server() { }

        protected Databases GetDatabases()
        {
            var helper = new Sql(this.ConnectionString);
            var items = new List<Database>();
            using (var set = helper.FillByTableNames("sys.databases"))
            {
                var table = set.GetTables().First();
                Func<DataRow, Database> selector =
                    row =>
                        new Database(this, row.Field<string>("name"), this.ConnectionString)
                        {
                            CollationName = row.Field<string>("collation_name"),
                            CreateDate = row.Field("create_date", Convert.ToDateTime),
                            Id = row.Field("database_id", Convert.ToInt64),
                            IsBrokerEnabled = row.Field("is_broker_enabled", Convert.ToBoolean)
                        };
                items.AddRange(table.Select().Select(selector));
            }
            return new Databases(items);
        }

        internal static Server GetInstance() => new Server();
        internal static Server GetInstance(string name) => new Server {Name = name};
        internal static Server GetInstance(string name, string connectionString) => new Server {Name = name, ConnectionString = connectionString};
        public static Server GetServer(string connectionString) => new Server {ConnectionString = connectionString};
        internal void SetName(string name) => this.Name = name;

        public override string ToString() => !this.Version.IsNullOrEmpty() ? $"{this.Name} - {this.Version}" : $"{this.Name}";

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.Databases[binder.Name];
            return true;
        }

        public string Name { get; protected set; }
    }
}
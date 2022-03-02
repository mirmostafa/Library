using Library.Data.SqlClient;
using Library.Data.SqlServer.Dynamics.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace Library.Data.SqlServer.Dynamics
{
    public class Server : DynamicObject, ISqlObject
    {
        private Databases _Databases;

        protected SqlConnectionStringBuilder ConnectionStringBuilder { get; } = new SqlConnectionStringBuilder();

        protected Server()
        {
        }

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
                {
                    this.Name = this.ConnectionStringBuilder.DataSource;
                }
                else
                {
                    this.ConnectionStringBuilder.DataSource = this.Name;
                }
            }
        }

        public Databases Databases => this._Databases ??= this.GetDatabases();

        public string Name { get; protected set; }

        public static Servers Servers => new Servers(from instance in SqlBrowserClient.Instances
                                                     select new Server
                                                     {
                                                         Name = instance.InstanceName.IsNullOrEmpty()
                                                             ? instance.ServerName
                                                             : $@"{instance.ServerName}\{instance.InstanceName}",
                                                         Version = instance.Version
                                                     });

        public string Version { get; protected set; }

        public static Server GetServer(string connectionString) => new Server { ConnectionString = connectionString };

        internal static Server GetInstance() => new Server();

        internal static Server GetInstance(string name) => new Server { Name = name };

        internal static Server GetInstance(string name, string connectionString) => new Server
        {
            Name = name,
            ConnectionString = connectionString
        };

        public override string ToString() => !this.Version.IsNullOrEmpty() ? $"{this.Name} - {this.Version}" : $"{this.Name}";

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            result = this.Databases[binder.Name];
            return true;
        }

        internal void SetName(string name) => this.Name = name;

        protected Databases GetDatabases()
        {
            var helper = new Sql(this.ConnectionString);
            var items = new List<Database>();
            using (var set = helper.FillByTableNames("sys.databases"))
            {
                var table = set.GetTables().First();
                Func<DataRow, Database> selector = row => new Database(this, row.Field<string>("name"), this.ConnectionString)
                {
                    CollationName = row
                        .Field<string>(
                            "collation_name"),
                    CreateDate = row.Field(
                        "create_date",
                        Convert.ToDateTime),
                    Id = row.Field("database_id",
                        Convert.ToInt64),
                    IsBrokerEnabled =
                        row.Field(
                            "is_broker_enabled",
                            Convert.ToBoolean)
                };
                items.AddRange(table.Select().Select(selector));
            }

            return new Databases(items);
        }
    }
}
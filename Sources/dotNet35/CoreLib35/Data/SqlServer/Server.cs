#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using Library35.Data.Ado;
using Library35.Data.SqlServer.Collections;
using Library35.Helpers;

namespace Library35.Data.SqlServer
{
	public class Server : ISqlObject
	{
		protected SqlConnectionStringBuilder ConnectionStringBuilder = new SqlConnectionStringBuilder();
		private Databases _Databases;

		protected Server()
		{
		}

		/// <summary>
		///     Gets or sets the connection string.
		/// </summary>
		/// <value> The connection string. </value>
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
				this.Name = this.ConnectionStringBuilder.DataSource;
			}
		}

		public Databases Databases
		{
			get { return PropertyHelper.Get(ref this._Databases, this.GetDatabases); }
		}

		public string Version { get; protected set; }
		public static Servers Servers
		{
			get
			{
				return new Servers(new List<Server>(from row in SqlDataSourceEnumerator.Instance.GetDataSources().Select()
					select new Server
					       {
						       Name =
							       row.Field<string>("InstanceName").IsNullOrEmpty()
								       ? row.Field<string>("ServerName")
								       : string.Format(@"{0}\{1}", row.Field<string>("ServerName"), row.Field<string>("InstanceName")),
						       Version = row.Field<string>("Version")
					       }));
			}
		}

		#region ISqlObject Members
		public string Name { get; protected set; }
		#endregion

		protected Databases GetDatabases()
		{
			var helper = new SqlHelper(this.ConnectionString);
			var items = new List<Database>();
			using (var set = helper.FillByTableNames(new[]
			                                         {
				                                         "sys.databases"
			                                         }))
			{
				var table = set.GetTables().First();
				Func<DataRow, Database> selector = row => new Database(this, row.Field<string>("name"))
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

		internal static Server GetInstance()
		{
			return new Server();
		}

		public static Server GetServer(string connectionString)
		{
			return new Server
			       {
				       ConnectionString = connectionString
			       };
		}

		internal void SetName(string name)
		{
			this.Name = name;
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
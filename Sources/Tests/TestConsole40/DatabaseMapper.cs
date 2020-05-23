#region File Notice
// Created at: 2013/12/24 3:46 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using Library40.Helpers;

namespace TestConsole40
{
	internal class DatabaseMapper
	{
		private readonly SqlConnection _Connection;

		public DatabaseMapper(string connectionstring)
		{
			this._Connection = new SqlConnection(connectionstring);
		}

		internal void Insert(DatabaseEntity entity)
		{
			var fields = entity;
			var keys = string.Concat("(", string.Join(",", fields.Keys.Select(key => string.Format(" [{0}]", key))).Trim(), ")");
			var values = string.Concat("(", string.Join(",", fields.Values.Select(key => string.Format(" '{0}'", key))).Trim(), ")");
			var query = string.Format("INSERT INTO [{0}] {1} VALUES {2}", entity.TableName, keys, values);
			this._Connection.ExecuteNonQuery(query);
		}
	}

	internal class DatabaseEntity : DynamicObject, IEnumerable<KeyValuePair<String, Object>>
	{
		public DatabaseEntity(string tablename)
		{
			this.TableName = tablename;
			this.Storage = new Dictionary<string, object>();
		}

		public IEnumerable<string> Keys
		{
			get { return this.Storage.Keys; }
		}

		public IEnumerable<object> Values
		{
			get { return this.Storage.Values; }
		}

		public string TableName { get; set; }

		private Dictionary<String, object> Storage { get; set; }

		#region IEnumerable<KeyValuePair<string,object>> Members
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this.Storage.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = null;
			if (this.Storage.ContainsKey(binder.Name))
			{
				result = this.Storage[binder.Name];
				return true;
			}
			return false;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (this.Storage.ContainsKey(binder.Name))
				this.Storage[binder.Name] = value;
			else
				this.Storage.Add(binder.Name, value);
			return true;
		}
	}
}
#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using Library40.Helpers;

namespace Library40.Data.SqlServer
{
	public class DataReader : SqlObject<DataReader, Database>, IDisposable
	{
		public DataReader(SqlDataReader sqlDataReader, Database owner, string name)
			: base(owner, name)
		{
			this.SqlDataReader = sqlDataReader;
		}

		private SqlDataReader SqlDataReader { get; set; }

		#region IDisposable Members
		public void Dispose()
		{
			this.SqlDataReader.Close();
			((IDisposable)this.SqlDataReader).Dispose();
		}
		#endregion

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = this.SqlDataReader[binder.Name];
			return true;
		}

		public bool Read()
		{
			return this.SqlDataReader.Read();
		}

		public IEnumerable<T> Select<T>() where T : new()
		{
			return this.SqlDataReader.Select<T>();
		}
	}
}
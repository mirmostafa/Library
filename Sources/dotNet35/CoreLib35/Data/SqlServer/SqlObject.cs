#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Library35.Data.Ado;
using Library35.ExceptionHandlingPattern;
using Library35.Helpers;

namespace Library35.Data.SqlServer
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class SqlObject<TSqlObject, TOwner> : ISqlObject, IExceptionHandlerContainer<Exception>
		where TSqlObject : SqlObject<TSqlObject, TOwner>
	{
		private static ExceptionHandling<Exception> _CommonExceptionHandling;
		private ExceptionHandling<Exception> _ExceptionHandling;

		protected SqlObject(TOwner owner, string name)
		{
			this.Owner = owner;
			this.Name = name;
		}

		public static ExceptionHandling<Exception> CommonExceptionHandling
		{
			get { return _CommonExceptionHandling ?? (_CommonExceptionHandling = new ExceptionHandling<Exception>()); }
			set { _CommonExceptionHandling = value; }
		}

		public virtual TOwner Owner { get; private set; }

		#region IExceptionHandlerContainer<Exception> Members
		public ExceptionHandling<Exception> ExceptionHandling
		{
			get { return (this._ExceptionHandling ?? CommonExceptionHandling); }
			set { this._ExceptionHandling = value; }
		}
		#endregion

		#region ISqlObject Members
		public virtual string Name { get; private set; }
		#endregion

		protected static IEnumerable<DataRow> GetItems(string connectionstring, string query)
		{
			var helper = new SqlHelper(connectionstring);
			using (var set = helper.FillByTableNames(new[]
			                                         {
				                                         query
			                                         }))
				return set.GetTables().First().Select();
		}

		protected static IEnumerable<DataRow> GetQuery(string connectionstring, string query)
		{
			var helper = new SqlHelper(connectionstring);
			using (var set = helper.FillByQuery(query))
				return set.GetTables().First().Select();
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
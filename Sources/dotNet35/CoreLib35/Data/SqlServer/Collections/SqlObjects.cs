#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Library35.Helpers;

namespace Library35.Data.SqlServer.Collections
{
	public abstract class SqlObjects<TSqlObject> : IEnumerable<TSqlObject>
		where TSqlObject : class, ISqlObject
	{
		private readonly IEnumerable<TSqlObject> _Items;

		protected SqlObjects(IEnumerable<TSqlObject> items)
		{
			this._Items = items;
		}

		public bool FillOnDemand
		{
			get { return true; }
		}

		public virtual TSqlObject this[int index]
		{
			get { return this.Items.ElementAt(index); }
		}

		public TSqlObject this[string name]
		{
			get { return (from item in this.Items where item.Name.EqualsTo(name) select item).FirstOrDefault(); }
		}

		protected IEnumerable<TSqlObject> Items
		{
			get { return this._Items; }
		}

		#region IEnumerable<TSqlObject> Members
		public IEnumerator<TSqlObject> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}
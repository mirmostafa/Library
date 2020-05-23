#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Library40.Bcl;
using Library40.Helpers;

namespace Library40.Data.SqlServer.Collections
{
	public abstract class SqlObjects<TSqlObject> : IEnumerable<TSqlObject>
		where TSqlObject : class, ISqlObject
	{
		private readonly LazyInit<IEnumerable<TSqlObject>> _Items;

		protected SqlObjects(IEnumerable<TSqlObject> items)
		{
			this._Items = new LazyInit<IEnumerable<TSqlObject>>(() => items);
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
			get { return this._Items.Value; }
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
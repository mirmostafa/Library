#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections;
using System.Collections.Generic;

namespace Library40.Win.Controls.BarChart
{
	public class CDataColumnCollection : IList<CDataColumnItem>, ICollection<CDataColumnItem>, IEnumerable<CDataColumnItem>, IEnumerable
	{
		private readonly List<CDataColumnItem> items = new List<CDataColumnItem>();

		#region IList<CDataColumnItem> Members
		public void Add(CDataColumnItem item)
		{
			this.items.Add(item);
		}

		public void Clear()
		{
			this.items.Clear();
		}

		public bool Contains(CDataColumnItem item)
		{
			return this.items.Contains(item);
		}

		public void CopyTo(CDataColumnItem[] array, int arrayIndex)
		{
			this.items.CopyTo(array, arrayIndex);
		}

		public IEnumerator<CDataColumnItem> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		public int IndexOf(CDataColumnItem item)
		{
			return this.items.IndexOf(item);
		}

		public void Insert(int index, CDataColumnItem item)
		{
			this.items.Insert(index, item);
		}

		public bool Remove(CDataColumnItem item)
		{
			return this.items.Remove(item);
		}

		public void RemoveAt(int index)
		{
			this.items.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		public int Count
		{
			get { return this.items.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public CDataColumnItem this[int index]
		{
			get { return this.items[index]; }
			set { this.items[index] = value; }
		}
		#endregion
	}
}
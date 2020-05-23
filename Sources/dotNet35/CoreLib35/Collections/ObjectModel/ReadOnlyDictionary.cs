#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Library35.Helpers;
#endregion

namespace Library35.Collections.ObjectModel
{
	public class ReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		protected readonly Dictionary<TKey, TValue> Items = new Dictionary<TKey, TValue>();

		public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
		{
			pairs.ToList().ForEach(item => this.Items.Add(item.Key, item.Value));
		}

		public TValue this[TKey key]
		{
			get { return this.Items[key]; }
			set { this.Items[key] = value; }
		}

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}
		#endregion

		public bool ContainsKey(TKey key)
		{
			return this.Items.ContainsKey(key);
		}

		public virtual Dictionary<TKey, TValue> ToDictionary()
		{
			var result = new Dictionary<TKey, TValue>();
			this.Items.ForEach(item => result.Add(item.Key, item.Value));
			return result;
		}
	}
}
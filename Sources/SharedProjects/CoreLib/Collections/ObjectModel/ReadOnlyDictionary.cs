#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Helpers;

#endregion

namespace Mohammad.Collections.ObjectModel
{
    public class ReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        protected readonly Dictionary<TKey, TValue> Items = new Dictionary<TKey, TValue>();
        public TValue this[TKey key] { get { return this.Items[key]; } set { this.Items[key] = value; } }
        public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs) { pairs.ToList().ForEach(item => this.Items.Add(item.Key, item.Value)); }
        public bool ContainsKey(TKey key) { return this.Items.ContainsKey(key); }

        public virtual Dictionary<TKey, TValue> ToDictionary()
        {
            var result = new Dictionary<TKey, TValue>();
            this.Items.ForEach(item => result.Add(item.Key, item.Value));
            return result;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() { return this.Items.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return this.Items.GetEnumerator(); }
    }
}
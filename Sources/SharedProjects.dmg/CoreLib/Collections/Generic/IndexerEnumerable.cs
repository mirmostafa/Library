using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mohammad.Collections.Generic
{
    internal class IndexerEnumerable<T> : IIndexerEnumerable<T>
    {
        private readonly IEnumerable<T> _Items;
        public IndexerEnumerable(IEnumerable<T> items) { this._Items = items; }
        public IEnumerator<T> GetEnumerator() => this._Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public T this[int index] => this._Items.ElementAt(index);
    }
}
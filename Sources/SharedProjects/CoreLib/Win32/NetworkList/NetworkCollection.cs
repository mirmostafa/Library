using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Win32.Interop.NetworkList;

namespace Mohammad.Win32.NetworkList
{
    /// <summary>
    ///     An enumerable collection of <see cref="Network" /> objects.
    /// </summary>
    public class NetworkCollection : IEnumerable<Network>
    {
        private readonly IEnumerable _NetworkEnumerable;

        internal NetworkCollection(IEnumerable networkEnumerable) { this._NetworkEnumerable = networkEnumerable; }

        /// <summary>
        ///     Returns the strongly typed enumerator for this collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="System.Collections.Generic.IEnumerator{T}" />  object.
        /// </returns>
        public IEnumerator<Network> GetEnumerator()
        {
            return (from INetwork network in this._NetworkEnumerable
                    select new Network(network)).GetEnumerator();
        }

        /// <summary>
        ///     Returns the enumerator for this collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="System.Collections.IEnumerator" /> object.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (INetwork network in this._NetworkEnumerable)
                yield return new Network(network);
        }
    }
}
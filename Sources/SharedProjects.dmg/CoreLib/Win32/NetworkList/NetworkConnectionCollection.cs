using System.Collections;
using System.Collections.Generic;
using Mohammad.Win32.Interop.NetworkList;

namespace Mohammad.Win32.NetworkList
{
    /// <summary>
    ///     An enumerable collection of <see cref="NetworkConnection" /> objects.
    /// </summary>
    public class NetworkConnectionCollection : IEnumerable<NetworkConnection>
    {
        private readonly IEnumerable networkConnectionEnumerable;

        internal NetworkConnectionCollection(IEnumerable networkConnectionEnumerable) { this.networkConnectionEnumerable = networkConnectionEnumerable; }

        /// <summary>
        ///     Returns the strongly typed enumerator for this collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Collections.Generic.IEnumerator{T}" /> object.
        /// </returns>
        public IEnumerator<NetworkConnection> GetEnumerator()
        {
            foreach (INetworkConnection networkConnection in this.networkConnectionEnumerable)
                yield return new NetworkConnection(networkConnection);
        }

        /// <summary>
        ///     Returns the enumerator for this collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Collections.IEnumerator" /> object.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (INetworkConnection networkConnection in this.networkConnectionEnumerable)
                yield return new NetworkConnection(networkConnection);
        }
    }
}
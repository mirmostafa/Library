#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections;
using System.Collections.Generic;
using Library35.Win32.Interop.NetworkList;

namespace Library35.Win32.NetworkList
{
	/// <summary>
	///     An enumerable collection of <see cref="NetworkConnection" /> objects.
	/// </summary>
	public class NetworkConnectionCollection : IEnumerable<NetworkConnection>
	{
		#region Private Fields
		private readonly IEnumerable networkConnectionEnumerable;
		#endregion // Private Fields

		internal NetworkConnectionCollection(IEnumerable networkConnectionEnumerable)
		{
			this.networkConnectionEnumerable = networkConnectionEnumerable;
		}

		#region IEnumerable<NetworkConnection> Members
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
		#endregion
	}
}
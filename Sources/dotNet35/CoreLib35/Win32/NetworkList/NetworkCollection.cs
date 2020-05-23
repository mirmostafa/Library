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
	///     An enumerable collection of <see cref="Network" /> objects.
	/// </summary>
	public class NetworkCollection : IEnumerable<Network>
	{
		#region Private Fields
		private readonly IEnumerable networkEnumerable;
		#endregion // Private Fields

		internal NetworkCollection(IEnumerable networkEnumerable)
		{
			this.networkEnumerable = networkEnumerable;
		}

		#region IEnumerable<Network> Members
		/// <summary>
		///     Returns the strongly typed enumerator for this collection.
		/// </summary>
		/// <returns>
		///     An <see cref="System.Collections.Generic.IEnumerator{T}" /> object.
		/// </returns>
		public IEnumerator<Network> GetEnumerator()
		{
			foreach (INetwork network in this.networkEnumerable)
				yield return new Network(network);
		}

		/// <summary>
		///     Returns the enumerator for this collection.
		/// </summary>
		/// <returns>
		///     An <see cref="System.Collections.IEnumerator" /> object.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (INetwork network in this.networkEnumerable)
				yield return new Network(network);
		}
		#endregion
	}
}
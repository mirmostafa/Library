#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Runtime.InteropServices;

namespace Library40.Generic
{
	/// <summary>
	///     Supports cloning, which creates a new instance of a class with the same value as an existing instance.
	/// </summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public interface ICloneable<out T>
	{
		/// <summary>
		///     Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		///     A new object that is a copy of this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		T Clone();
	}
}
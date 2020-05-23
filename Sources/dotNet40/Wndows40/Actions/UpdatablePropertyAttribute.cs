#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Win.Actions
{
	/// <summary>
	///     Ensures that the owner is Updatable
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class UpdatablePropertyAttribute : Attribute
	{
	}
}
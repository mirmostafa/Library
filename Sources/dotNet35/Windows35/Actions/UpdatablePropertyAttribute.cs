#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Windows.Actions
{
	/// <summary>
	///     Ensures that the owner is Updatable
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class UpdatablePropertyAttribute : Attribute
	{
	}
}
#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;

namespace Library40.LogSystem.Attributes
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public enum AutoFillTag
	{
		None,
		SenderType
	}

	public class LogDescriptionAttribute : Attribute
	{
		public LogDescriptionAttribute()
		{
		}

		public LogDescriptionAttribute(string description)
		{
			this.Description = description;
		}

		public string Description { get; private set; }

		public AutoFillTag AutoFillTag { get; set; }
	}
}
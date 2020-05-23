#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.PlugIn
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class ExportAttribute : Attribute
	{
		public ExportAttribute()
		{
		}

		public ExportAttribute(string name)
		{
			this.Name = name;
		}

		public string Name { get; set; }
	}
}
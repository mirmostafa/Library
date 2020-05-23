#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library40.Internals;

namespace Library40.Globalization.Attributes
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal sealed class LocalizedDescriptionAttribute : Attribute
	{
		public LocalizedDescriptionAttribute([NotNull] string cultureName)
			: this(cultureName, string.Empty)
		{
		}

		public LocalizedDescriptionAttribute([NotNull] string cultureName, string description)
		{
			if (cultureName == null)
				throw new ArgumentNullException("cultureName");
			this.CultureName = cultureName;
			this.Description = description;
		}

		[NotNull]
		public string CultureName { get; set; }

		public string Description { get; set; }
	}
}
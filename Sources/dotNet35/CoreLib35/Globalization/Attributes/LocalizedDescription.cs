#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.Internals;

namespace Library35.Globalization.Attributes
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
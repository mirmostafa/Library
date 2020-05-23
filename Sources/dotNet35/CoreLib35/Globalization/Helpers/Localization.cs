#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Globalization.Helpers
{
	public static class Localization
	{
		public static ILocalizer Localizer { get; set; }

		public static string ToLocaliazedString(this PersianDateTime dateTime)
		{
			return Localizer.ToString(dateTime);
		}

		public static string ToLocaliazedString(this DateTime dateTime)
		{
			return Localizer.ToString(dateTime);
		}
	}
}
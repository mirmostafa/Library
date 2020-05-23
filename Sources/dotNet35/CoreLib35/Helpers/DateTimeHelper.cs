#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.Win32.Natives.IfacesEnumsStructsClasses;

namespace Library35.Helpers
{
	public static class DateTimeHelper
	{
		public static readonly DateTime SqlMaxDateTimeValue = DateTime.Parse("12/31/9999");
		public static readonly DateTime SqlMinDateTimeValue = DateTime.Parse("1/1/1753 12:00:00 AM");

		public static bool IsBetween(this TimeSpan source, TimeSpan start, TimeSpan end)
		{
			return source >= start && source <= end;
		}

		public static bool IsBetween(this TimeSpan source, string start, string end)
		{
			return source.IsBetween(start.ToTimeSpan(), end.ToTimeSpan());
		}

		public static bool IsBetween(string source, string start, string end)
		{
			return source.ToTimeSpan().IsBetween(start.ToTimeSpan(), end.ToTimeSpan());
		}

		private static TimeSpan ToTimeSpan(this string source)
		{
			return TimeSpan.Parse(source);
		}

		public static DateTime ToDateTime(this FILETIME filetime)
		{
			return DateTime.FromFileTime(filetime.ToLong());
		}

		public static long ToLong(this FILETIME filetime)
		{
			return (((long)filetime.dwHighDateTime) << 32) + filetime.dwLowDateTime;
		}
	}
}
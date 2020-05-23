#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library40.Globalization.DataTypes;

namespace Library40.Globalization.Helpers
{
	public static class PersianDateTimeHelper
	{
		public static PersianDateTime PersianToPersianDateTime(this string persianDateTimeString)
		{
			return PersianDateTime.ParsePersian(persianDateTimeString);
		}

		public static PersianDateTime EnglishToPersianDateTime(this string englishDateTimeString)
		{
			return PersianDateTime.ParseEnglish(englishDateTimeString);
		}

		public static PersianDateTime ToPersianDateTime(this DateTime dateTime)
		{
			return new PersianDateTime(dateTime);
		}

		public static int GetDayOfMonth(this PersianMonth persianMonth, int year)
		{
			switch (persianMonth)
			{
				case PersianMonth.Farvardin:
				case PersianMonth.Ordibehesht:
				case PersianMonth.Khordad:
				case PersianMonth.Tir:
				case PersianMonth.Mordad:
				case PersianMonth.Sharivar:
					return 31;
				case PersianMonth.Mehr:
				case PersianMonth.Aban:
				case PersianMonth.Azar:
				case PersianMonth.Dey:
				case PersianMonth.Bahman:
					return 30;
				case PersianMonth.Esfand:
					return PersianDateTime.PersianCalendar.IsLeapYear(year) ? 30 : 29;
			}
			return 0;
		}

		public static int GetDayOfMonth(this PersianMonth persianMonth)
		{
			switch (persianMonth)
			{
				case PersianMonth.Farvardin:
				case PersianMonth.Ordibehesht:
				case PersianMonth.Khordad:
				case PersianMonth.Tir:
				case PersianMonth.Mordad:
				case PersianMonth.Sharivar:
					return 31;
				case PersianMonth.Mehr:
				case PersianMonth.Aban:
				case PersianMonth.Azar:
				case PersianMonth.Dey:
				case PersianMonth.Bahman:
					return 30;
				case PersianMonth.Esfand:
					return 29;
			}
			return 0;
		}

		public static bool IsPersianDateTime(this string s)
		{
			PersianDateTime persianDateTime;
			return PersianDateTime.TryParsePersian(s, out persianDateTime);
		}
	}
}
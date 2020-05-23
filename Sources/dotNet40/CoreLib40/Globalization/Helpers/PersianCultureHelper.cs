#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Globalization;
using System.Reflection;

namespace Library40.Globalization.Helpers
{
	public static class PersianCultureHelper
	{
		//Represents a PropertyInfo that refer to ID of Calendar. The ID is private property of Calendar.
		private static readonly PropertyInfo _CalendarId;

		//Represents a FieldInfo that refer to m_isReadOnly of CultureInfo. The m_isReadOnly is private filed of CultureInfo.
		private static readonly FieldInfo _CultureInfoReadOnly;

		//Represents a FieldInfo that refer to calendar of CultureInfo. The calendar is private filed of CultureInfo.
		private static readonly FieldInfo _CultureInfoCalendar;

		//Represents a FieldInfo that refer to isReadOnly of NumberFormatInfo. The isReadOnly is private filed of NumberFormatInfo.
		private static readonly FieldInfo _NumberFormatInfoReadOnly;

		//Represents a FieldInfo that refer to m_isReadOnly of DateTimeFormatInfo. The m_isReadOnly is private filed of DateTimeFormatInfo.
		private static readonly FieldInfo _DateTimeFormatInfoReadOnly;

		//Represents a FieldInfo that refer to calendar of DateTimeFormatInfo. The calendar is private filed of DateTimeFormatInfo.
		private static readonly FieldInfo _DateTimeFormatInfoCalendar;

		//Represents a FieldInfo that refer to m_cultureTableRecord of DateTimeFormatInfo. The m_cultureTableRecord is private filed of DateTimeFormatInfo.
		private static readonly FieldInfo _DateTimeFormatInfoCultureTableRecord;

		//Represents a MethodInfo that refer to UseCurrentCalendar of CultureTableRecord. The UseCurrentCalendar is private method of CultureTableRecord that the class is private too.
		private static readonly MethodInfo _CultureTableRecordUseCurrentCalendar;

		/// <summary>
		///     Represents static constructor
		/// </summary>
		static PersianCultureHelper()
		{
			_CalendarId = typeof (Calendar).GetProperty("ID", BindingFlags.NonPublic | BindingFlags.Instance);
			_CultureInfoReadOnly = typeof (CultureInfo).GetField("m_isReadOnly", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			_CultureInfoCalendar = typeof (CultureInfo).GetField("calendar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			_NumberFormatInfoReadOnly = typeof (NumberFormatInfo).GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			_DateTimeFormatInfoCalendar = typeof (DateTimeFormatInfo).GetField("calendar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			_DateTimeFormatInfoReadOnly = typeof (DateTimeFormatInfo).GetField("m_isReadOnly", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			_DateTimeFormatInfoCultureTableRecord = typeof (DateTimeFormatInfo).GetField("m_cultureTableRecord", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			var cultureTableRecord = typeof (DateTimeFormatInfo).Assembly.GetType("System.Globalization.CultureTableRecord");
			_CultureTableRecordUseCurrentCalendar = cultureTableRecord.GetMethod("UseCurrentCalendar", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		/// <summary>
		///     Represents a method that set PersianCalendar to specified instances of CultureInfo
		/// </summary>
		/// <param name="culture">Represents an instance of CultureInfo that persian number format should be set to it.</param>
		public static void SetPersianOptions(CultureInfo culture)
		{
			SetPersianCalendar(culture, new DateTimeFormatInfo());
		}

		/// <summary>
		///     Represents a method that set PersianCalendar to specified instances of CultureInfo and DateTimeFormatInfo
		/// </summary>
		/// <param name="culture">Represents an instance of CultureInfo that persian number format should be set to it.</param>
		/// <param name="dateTimeFormat">Represents an instance of DateTimeFormatInfo that persian format should be set to it.</param>
		public static void SetPersianCalendar(CultureInfo culture, DateTimeFormatInfo dateTimeFormat)
		{
			if (culture == null || culture.LCID != 1065)
				return;
			var calendar = new PersianCalendar();
			var readOnly = (bool)_CultureInfoReadOnly.GetValue(culture);
			if (readOnly)
				_CultureInfoReadOnly.SetValue(culture, false);
			culture.DateTimeFormat = dateTimeFormat;
			InitPersianDateTimeFormat(dateTimeFormat);
			_CultureInfoCalendar.SetValue(culture, calendar);
			InitPersianNumberFormat(culture);
			if (readOnly)
				_CultureInfoReadOnly.SetValue(culture, true);
		}

		/// <summary>
		///     Represents a method that set persian number format to specified instance CultureInfo.
		/// </summary>
		/// <param name="info">Represents an instance of CultureInfo that persian number format should be set to it.</param>
		public static void InitPersianNumberFormat(CultureInfo info)
		{
			InitPersianNumberFormat(info.NumberFormat);
		}

		/// <summary>
		///     Represents a method that set persian number format to specified instance NumberFormatInfo.
		/// </summary>
		/// <param name="info">Represents an instance of NumberFormatInfo that persian option should be set to it.</param>
		public static void InitPersianNumberFormat(NumberFormatInfo info)
		{
			if (info == null)
				return;
			var readOnly = (bool)_NumberFormatInfoReadOnly.GetValue(info);
			if (readOnly)
				_NumberFormatInfoReadOnly.SetValue(info, false);
			info.NativeDigits = new[]
			                    {
				                    "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"
			                    };
			info.CurrencyDecimalSeparator = "/";
			info.CurrencySymbol = "ريال";
			if (readOnly)
				_NumberFormatInfoReadOnly.SetValue(info, true);
		}

		/// <summary>
		///     Represents a method that set persian option to specified instance CultureInfo
		/// </summary>
		/// <param name="info">Represents an instance of DateTimeFormatInfo that persian option should be set to it.</param>
		public static void InitPersianDateTimeFormat(DateTimeFormatInfo info)
		{
			if (info == null)
				return;
			var calendar = new PersianCalendar();
			var readOnly = (bool)_DateTimeFormatInfoReadOnly.GetValue(info);
			if (readOnly)
				_DateTimeFormatInfoReadOnly.SetValue(info, false);
			_DateTimeFormatInfoCalendar.SetValue(info, calendar);
			var obj2 = _DateTimeFormatInfoCultureTableRecord.GetValue(info);
			_CultureTableRecordUseCurrentCalendar.Invoke(obj2,
				new[]
				{
					_CalendarId.GetValue(calendar, null)
				});
			info.AbbreviatedDayNames = new[]
			                           {
				                           "ی", "د", "س", "چ", "پ", "ج", "ش"
			                           };
			info.ShortestDayNames = new[]
			                        {
				                        "ی", "د", "س", "چ", "پ", "ج", "ش"
			                        };
			info.DayNames = new[]
			                {
				                "یکشنبه", "دوشنبه", "ﺳﻪشنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"
			                };
			info.AbbreviatedMonthNames = new[]
			                             {
				                             "فروردین", "ارديبهشت", "خرداد", "تير", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
			                             };
			info.MonthNames = new[]
			                  {
				                  "فروردین", "ارديبهشت", "خرداد", "تير", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
			                  };
			info.AMDesignator = "ق.ظ";
			info.PMDesignator = "ب.ظ";
			info.FirstDayOfWeek = DayOfWeek.Saturday;
			info.FullDateTimePattern = "yyyy MMMM dddd, dd HH:mm:ss";
			info.LongDatePattern = "yyyy MMMM dddd, dd";
			info.ShortDatePattern = "yyyy/MM/dd";
			if (readOnly)
				_DateTimeFormatInfoReadOnly.SetValue(info, true);
		}
	}
}
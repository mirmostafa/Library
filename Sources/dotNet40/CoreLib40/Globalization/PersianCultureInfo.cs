#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Library40.Globalization
{
	public sealed class PersianCultureInfo : CultureInfo
	{
		private readonly Calendar _Calendar;
		private readonly Calendar[] _Optionals;

		public PersianCultureInfo(string cultureName = "fa-IR", bool useUserOverride = true, bool setPatterns = false)
			: base(cultureName, useUserOverride)
		{
			//Temporary Value for _Calendar.
			this._Calendar = base.OptionalCalendars[0];

			//populating new list of optional calendars.
			var optionalCalendars = new List<Calendar>();
			optionalCalendars.AddRange(base.OptionalCalendars);
			optionalCalendars.Insert(0, new PersianCalendar());

			var formatType = typeof (DateTimeFormatInfo);
			var calendarType = typeof (Calendar);

			var idProperty = calendarType.GetProperty("ID", BindingFlags.Instance | BindingFlags.NonPublic);
			var optionalCalendarfield = formatType.GetField("optionalCalendars", BindingFlags.Instance | BindingFlags.NonPublic);

			//populating new list of optional calendar ids
			var newOptionalCalendarIDs = new Int32[optionalCalendars.Count];
			for (var i = 0; i < newOptionalCalendarIDs.Length; i++)
				newOptionalCalendarIDs[i] = (Int32)idProperty.GetValue(optionalCalendars[i], null);

			optionalCalendarfield.SetValue(this.DateTimeFormat, newOptionalCalendarIDs);

			this._Optionals = optionalCalendars.ToArray();
			this._Calendar = this._Optionals[0];
			this.DateTimeFormat.Calendar = this._Optionals[0];

			this.DateTimeFormat.MonthNames = new[]
			                                 {
				                                 "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
			                                 };
			this.DateTimeFormat.MonthGenitiveNames = new[]
			                                         {
				                                         "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
			                                         };
			this.DateTimeFormat.AbbreviatedMonthNames = new[]
			                                            {
				                                            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
			                                            };
			this.DateTimeFormat.AbbreviatedMonthGenitiveNames = new[]
			                                                    {
				                                                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
			                                                    };

			this.DateTimeFormat.AbbreviatedDayNames = new[]
			                                          {
				                                          "ی", "د", "س", "چ", "پ", "ج", "ش"
			                                          };
			this.DateTimeFormat.ShortestDayNames = new[]
			                                       {
				                                       "ی", "د", "س", "چ", "پ", "ج", "ش"
			                                       };
			this.DateTimeFormat.DayNames = new[]
			                               {
				                               "یکشنبه", "دوشنبه", "ﺳﻪشنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"
			                               };

			this.DateTimeFormat.AMDesignator = "ق.ظ";
			this.DateTimeFormat.PMDesignator = "ب.ظ";

			this.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Saturday;

			if (setPatterns)
			{
				this.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
				this.DateTimeFormat.LongDatePattern = "yyyy/MM/dd";

				this.DateTimeFormat.SetAllDateTimePatterns(new[]
				                                           {
					                                           "yyyy/MM/dd"
				                                           },
					'd');
				this.DateTimeFormat.SetAllDateTimePatterns(new[]
				                                           {
					                                           "dddd, dd MMMM yyyy"
				                                           },
					'D');
				this.DateTimeFormat.SetAllDateTimePatterns(new[]
				                                           {
					                                           "yyyy MMMM"
				                                           },
					'y');
				this.DateTimeFormat.SetAllDateTimePatterns(new[]
				                                           {
					                                           "yyyy MMMM"
				                                           },
					'Y');
			}
		}

		public override Calendar Calendar
		{
			get { return this._Calendar; }
		}

		public override Calendar[] OptionalCalendars
		{
			get { return this._Optionals; }
		}
	}
}
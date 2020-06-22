#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Globalization;

namespace Mohammad.Helpers
{
    public static class DateTimeHelper
    {
        public static bool IsBetween(this TimeSpan source, TimeSpan start, TimeSpan end) => source >= start && source <= end;

        public static bool IsBetween(this DateTime source, DateTime start, DateTime end) => source.ToTimeSpan() >= start.ToTimeSpan() &&
                                                                                            source.ToTimeSpan() <= end.ToTimeSpan();

        public static bool IsBetween(this TimeSpan source, string start, string end) => source.IsBetween(ToTimeSpan(start), ToTimeSpan(end));
        public static bool IsValid(string dateTime) => DateTime.TryParse(dateTime, out var buffer);
        public static DateTime ToDateTime(this TimeSpan source) => new DateTime(source.Ticks);
        public static TimeSpan ToTimeSpan(string source) => TimeSpan.Parse(source);
        public static TimeSpan ToTimeSpan(this DateTime source) => new TimeSpan(source.Ticks);

        public static void Deconstruct(this DateTime dt, out int year, out int month, out int day, out int hour, out int minute, out int second, out int millisecond)
            => (year, month, day, hour, minute, second, millisecond) = (dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

        public static void Deconstruct(this DateTime dt, out int year, out int month, out int day)
            => (year, month, day) = (dt.Year, dt.Month, dt.Day);

        public static void Deconstruct(this DateTime dt, out int hour, out int minute, out int second, out int millisecond)
            => (hour, minute, second, millisecond) = (dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

        public static string ToShortPersianDateString(this DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date):0000}-{pc.GetMonth(date):00}-{pc.GetDayOfMonth(date):00}";
        }

        public static string ToPersianDateString(this DateTime date)
        {
            var pc = new PersianCalendar();
            return $@"{date.Hour.ToString().PadLeft(2, '0')}:{date.Minute.ToString().PadLeft(2, '0')}:{date.Second.ToString().PadLeft(2, '0')} {pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)}";
        }
    }
}
#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Helpers
{
    public static partial class DateTimeHelper
    {
        public static bool IsBetween(this TimeSpan source, TimeSpan start, TimeSpan end) => source >= start && source <= end;

        public static bool IsBetween(this DateTime source, DateTime start, DateTime end) => source.ToTimeSpan() >= start.ToTimeSpan() &&
                                                                                            source.ToTimeSpan() <= end.ToTimeSpan();

        public static bool IsBetween(this TimeSpan source, string start, string end) => source.IsBetween(ToTimeSpan(start), ToTimeSpan(end));
        public static bool IsValid(string dateTime) => DateTime.TryParse(dateTime, out var buffer);
        public static DateTime ToDateTime(this TimeSpan source) => new DateTime(source.Ticks);
        public static TimeSpan ToTimeSpan(string source) => TimeSpan.Parse(source);
        public static TimeSpan ToTimeSpan(this DateTime source) => new TimeSpan(source.Ticks);
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Mohammad.Globalization
{
    public static class DateTools
    {
        private static readonly PersianCalendar _PersianCalendar = new PersianCalendar();

        public static DateTime DefaulDate { get; set; } = _PersianCalendar.MinSupportedDateTime;

        public static string TranslateToPersian(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Saturday: return "شنبه";
                case DayOfWeek.Sunday: return "یکشنبه";
                case DayOfWeek.Monday: return "دوشنبه";
                case DayOfWeek.Tuesday: return "سه شنبه";
                case DayOfWeek.Wednesday: return "چهارشنبه";
                case DayOfWeek.Thursday: return "پنجشنبه";
                case DayOfWeek.Friday: return "جمعه";
                default: throw new ArgumentOutOfRangeException(nameof(day));
            }
        }

        public static string ToPersianDate(this DateTime? date) => date == null ? string.Empty : ToPersianDate(date.Value);

        public static string ToPersianDate(this DateTime date) => ReformatPersian(GetPersianDateParts(date));

        public static string ReformatPersian(int? year, int? month, int? day) => $"{(year < 1300 ? year + 1300 : year):0000}/{month:00}/{day:00}";

        public static string ReformatPersian(Tuple<int, int, int> data) =>
            $"{(data.Item1 < 1300 ? data.Item1 + 1300 : data.Item1):0000}/{data.Item2:00}/{data.Item3:00}";

        public static string ReformatPersian(string persianDate)
        {
            var parts = GetParts(persianDate);
            return ReformatPersian(parts[0], parts[1], parts[2]);
        }

        public static int[] GetParts(string persianDate)
        {
            var parts = persianDate.Split('/').Select(s =>
            {
                s = s.Trim();
                if (s.Contains(" "))
                    s = s.Substring(0, s.IndexOf(" ", StringComparison.Ordinal));
                return Convert.ToInt32(s);
            }).ToArray();
            if (parts.Length == 3)
                return parts;
            var result = new int[3];
            result[0] = GetPersianYear(DateTime.Now);
            result[1] = parts[0];
            result[2] = parts[1];
            return result;
        }

        public static DateTime ToDateTime(int? year, int? month, int? day) => AllHaveValue(year, month, day)
            ? _PersianCalendar.ToDateTime(year.Value, month.Value, day.Value, 0, 0, 0, 0)
            : DefaulDate;

        public static Tuple<int, int, int> GetPersianDateParts(DateTime date) => Tuple.Create(GetPersianYear(date),
            GetPersianMonth(date),
            GetPersianDayOfMonth(date));

        public static DateTime ToDateTime(string persianDateText)
        {
            var data = GetParts(persianDateText);
            return ToDateTime(data[0], data[1], data[2]);
        }

        public static bool AllHaveValue(int? year, int? month, int? day) => year != null && month != null && day != null;

        public static int GetPersianDayOfMonth(this DateTime? dateTime) => _PersianCalendar.GetDayOfMonth(dateTime ?? DefaulDate);
        public static int GetPersianMonth(this DateTime? dateTime) => _PersianCalendar.GetMonth(dateTime ?? DefaulDate);
        public static int GetPersianYear(this DateTime? dateTime) => _PersianCalendar.GetYear(dateTime ?? DefaulDate);
        public static int GetPersianDaysCountInMonth(int year, int month) => _PersianCalendar.GetDaysInMonth(year, month);

        public static DateTime GetPersianFirstDayOfMonth(this DateTime current)
        {
            while (GetPersianDayOfMonth(current) != 1)
                current = current.AddDays(-1);
            return current;
        }

        public static IEnumerable<DateTime> GetPersianDaysOfMonth(this DateTime current)
        {
            current = GetPersianFirstDayOfMonth(current);
            var startIndex = Convert.ToInt32(current.DayOfWeek) + 1;
            var endIndex = startIndex + GetPersianDaysCountInMonth(GetPersianYear(current), GetPersianDayOfMonth(current));
            for (var index = startIndex; index < endIndex; index++)
            {
                yield return current;
                current = current.AddDays(1);
            }
        }
    }
}
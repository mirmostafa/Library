using System;

namespace Primitives
{
    public static class PrettyTime
    {
        public static string Years { get; }
        public static string Ago { get; }
        public static string About { get; }
        public static string Month { get; }
        public static string Months { get; }
        public static string Day { get; }
        public static string Days { get; }
        public static string Hours { get; }
        public static string Hour { get; }
        public static string Minute { get; }
        public static string Minutes { get; }
        public static string Seconds { get; }
        public static string JustNow { get; set; }
        public static string UnknownTime { get; }
        public static string Year { get; }

        static PrettyTime()
        {
            Year = "year";
            Years = "years";
            Ago = "ago";
            About = "about";
            Month = "month";
            Months = "months";
            Day = "day";
            Days = "days";
            Hours = "hours";
            Hour = "hour";
            Minute = "minute";
            Minutes = "minutes";
            Seconds = "seconds";
            UnknownTime = "Unknown time";
            JustNow = "just now";
        }

        public static string FormatPretty(this DateTime dt)
        {
            var span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                var years = span.Days / 365;
                if (span.Days % 365 != 0)
                {
                    years += 1;
                }
                return $"{About} {years} {(years == 1 ? Year : Years)} {Ago}";
            }
            if (span.Days > 30)
            {
                var months = span.Days / 30;
                if (span.Days % 31 != 0)
                {
                    months += 1;
                }
                return $"{About} {months} {(months == 1 ? Month : Months)} {Ago}";
            }
            if (span.Days > 0)
            {
                return $"{About} {span.Days} {(span.Days == 1 ? Day : Days)} {Ago}";
            }
            if (span.Hours > 0)
            {
                return $"{About} {span.Hours} {(span.Hours == 1 ? Hour : Hours)} {Ago}";
            }
            if (span.Minutes > 0)
            {
                return $"{About} {span.Minutes} {(span.Minutes == 1 ? Minute : Minutes)} {Ago}";
            }
            if (span.Seconds > 5)
            {
                return $"{About} {span.Seconds} {Seconds} {Ago}";
            }
            if (span.Seconds <= 5)
            {
                return JustNow;
            }
            return UnknownTime;
        }
    }
}
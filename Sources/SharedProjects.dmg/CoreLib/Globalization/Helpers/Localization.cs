using System;

namespace Mohammad.Globalization.Helpers
{
    public static class Localization
    {
        public static ILocalizer Localizer { get; set; }
        public static string ToLocalString(this PersianDateTime dateTime) { return Localizer.ToString(dateTime); }
        public static string ToLocalString(this DateTime dateTime) { return Localizer.ToString(dateTime); }
    }
}
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mohammad.Globalization.DataTypes;
using Mohammad.Helpers;

namespace Mohammad.Globalization
{
    public static class GlobalizationUtilities
    {
        private static DateTimeFormatInfo GregorianDateTimeFormatInfo { get { return CultureInfo.InvariantCulture.DateTimeFormat; } }
        public static IEnumerable<string> GetPersianAbbreviatedDayNames() { return new[] {"Sh", "Ye", "Do", "Se", "Ch", "Pa", "Jo"}.AsEnumerable(); }
        public static IEnumerable<string> GetGregorianAbbreviatedDayNames() { return GregorianDateTimeFormatInfo.AbbreviatedDayNames.AsEnumerable(); }
        public static IEnumerable<string> GetPersianDayNames() { return EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianDayOfWeek>()); }

        public static IEnumerable<string> GetPersianDayNames(string cultureName)
        {
            return EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianDayOfWeek>(), cultureName);
        }

        public static IEnumerable<string> GetPersianShortestDayNames() { return new[] {"Ye", "Do", "Se", "Ch", "Pa", "Jo", "Sh"}.AsEnumerable(); }
        public static string GetPersianPmDesignator() { return "È.Ù"; }
        public static string GetPersianAmDesignator() { return "Þ.Ù"; }

        public static IEnumerable<string> GetPersianMonthGenitiveNames()
        {
            return new[] {"Farvaedin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar", "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand", ""}.AsEnumerable();
        }

        public static IEnumerable<string> GetPersianAbbreviatedMonthGenitiveNames()
        {
            return new[] {"Far", "Ord", "Kho", "Tir", "Mor", "Shah", "Meh", "Aba", "Aza", "Dey", "Bah", "Esf", ""}.AsEnumerable();
        }

        public static IEnumerable<string> GetPersianMonthNames()
        {
            return new[] {"Farvaedin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar", "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand", ""}.AsEnumerable();
        }

        public static IEnumerable<string> GetPersianAbbreviatedMonthNames()
        {
            return new[] {"Far", "Ord", "Kho", "Tir", "Mor", "Shah", "Meh", "Aba", "Aza", "Dey", "Bah", "Esf", ""}.AsEnumerable();
        }

        public static IEnumerable<string> GetGregorianDayNames() { return GregorianDateTimeFormatInfo.DayNames.AsEnumerable(); }
        public static IEnumerable<string> GetGregorianShortestDayNames() { return GregorianDateTimeFormatInfo.ShortestDayNames.AsEnumerable(); }
        public static string GetGregorianAmDesignator() { return GregorianDateTimeFormatInfo.AMDesignator; }
        public static string GetGregorianPmDesignator() { return GregorianDateTimeFormatInfo.PMDesignator; }
        public static IEnumerable<string> GetGregorianMonthGenitiveNames() { return GregorianDateTimeFormatInfo.MonthGenitiveNames.AsEnumerable(); }

        public static IEnumerable<string> GetGregorianAbbreviatedMonthGenitiveNames()
        {
            return GregorianDateTimeFormatInfo.AbbreviatedMonthGenitiveNames.AsEnumerable();
        }

        public static IEnumerable<string> GetGregorianMonthNames() { return GregorianDateTimeFormatInfo.MonthNames.AsEnumerable(); }
        public static IEnumerable<string> GetGregorianAbbreviatedMonthNames() { return GregorianDateTimeFormatInfo.AbbreviatedMonthNames.AsEnumerable(); }
    }
}
#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mohammad.Globalization.DataTypes;
using Mohammad.Helpers;

namespace Mohammad.Globalization
{
    public static class GlobalizationUtilities
    {
        private static DateTimeFormatInfo GregorianDateTimeFormatInfo => CultureInfo.InvariantCulture.DateTimeFormat;
        public static IEnumerable<string> GetPersianAbbreviatedDayNames() => new[] {"Sh", "Ye", "Do", "Se", "Ch", "Pa", "Jo"}.AsEnumerable();
        public static IEnumerable<string> GetGregorianAbbreviatedDayNames() => GregorianDateTimeFormatInfo.AbbreviatedDayNames.AsEnumerable();
        public static IEnumerable<string> GetPersianDayNames() => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianDayOfWeek>());

        public static IEnumerable<string> GetPersianDayNames(string cultureName) => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianDayOfWeek>(),
            cultureName);

        public static IEnumerable<string> GetPersianShortestDayNames() => new[] {"Ye", "Do", "Se", "Ch", "Pa", "Jo", "Sh"}.AsEnumerable();
        public static string GetPersianPmDesignator() => "È.Ù";
        public static string GetPersianAmDesignator() => "Þ.Ù";

        public static IEnumerable<string> GetPersianMonthGenitiveNames() => new[]
        {
            "Farvaedin",
            "Ordibehesht",
            "Khordad",
            "Tir",
            "Mordad",
            "Shahrivar",
            "Mehr",
            "Aban",
            "Azar",
            "Dey",
            "Bahman",
            "Esfand",
            ""
        }.AsEnumerable();

        public static IEnumerable<string> GetPersianAbbreviatedMonthGenitiveNames() => new[]
        {
            "Far",
            "Ord",
            "Kho",
            "Tir",
            "Mor",
            "Shah",
            "Meh",
            "Aba",
            "Aza",
            "Dey",
            "Bah",
            "Esf",
            ""
        }.AsEnumerable();

        public static IEnumerable<string> GetPersianMonthNames() => new[]
        {
            "Farvaedin",
            "Ordibehesht",
            "Khordad",
            "Tir",
            "Mordad",
            "Shahrivar",
            "Mehr",
            "Aban",
            "Azar",
            "Dey",
            "Bahman",
            "Esfand",
            ""
        }.AsEnumerable();

        public static IEnumerable<string> GetPersianAbbreviatedMonthNames() => new[]
        {
            "Far",
            "Ord",
            "Kho",
            "Tir",
            "Mor",
            "Shah",
            "Meh",
            "Aba",
            "Aza",
            "Dey",
            "Bah",
            "Esf",
            ""
        }.AsEnumerable();

        public static IEnumerable<string> GetGregorianDayNames() => GregorianDateTimeFormatInfo.DayNames.AsEnumerable();
        public static IEnumerable<string> GetGregorianShortestDayNames() => GregorianDateTimeFormatInfo.ShortestDayNames.AsEnumerable();
        public static string GetGregorianAmDesignator() => GregorianDateTimeFormatInfo.AMDesignator;
        public static string GetGregorianPmDesignator() => GregorianDateTimeFormatInfo.PMDesignator;
        public static IEnumerable<string> GetGregorianMonthGenitiveNames() => GregorianDateTimeFormatInfo.MonthGenitiveNames.AsEnumerable();

        public static IEnumerable<string> GetGregorianAbbreviatedMonthGenitiveNames() =>
            GregorianDateTimeFormatInfo.AbbreviatedMonthGenitiveNames.AsEnumerable();

        public static IEnumerable<string> GetGregorianMonthNames() => GregorianDateTimeFormatInfo.MonthNames.AsEnumerable();
        public static IEnumerable<string> GetGregorianAbbreviatedMonthNames() => GregorianDateTimeFormatInfo.AbbreviatedMonthNames.AsEnumerable();
    }
}
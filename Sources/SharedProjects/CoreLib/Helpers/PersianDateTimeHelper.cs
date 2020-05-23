using System;
using Mohammad.Globalization;
using Mohammad.Globalization.DataTypes;

namespace Mohammad.Helpers
{
    public static class PersianDateTimeHelper
    {
        public static PersianDateTime ToPersianDateTime(this DateTime dateTime) => new PersianDateTime(dateTime);

        public static int GetDayCount(this PersianMonth persianMonth, int? year = null)
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
                    return year.HasValue ? (PersianDateTime.PersianCalendar.IsLeapYear(year.Value) ? 30 : 29) : 29;
                default:
                    return 0;
            }
        }

        public static bool IsPersianDateTime(string s)
        {
            PersianDateTime persianDateTime;
            return PersianDateTime.TryParsePersian(s, out persianDateTime);
        }
    }
}
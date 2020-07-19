#nullable enable
using System;
using Mohammad.Globalization;
using Mohammad.Globalization.DataTypes;

namespace Mohammad.Helpers
{
    public static class PersianDateTimeHelper
    {
        public static int GetDayCount(this PersianMonth persianMonth, in int? year = null) => persianMonth switch
        {
            PersianMonth.Farvardin => 31,
            PersianMonth.Ordibehesht => 31,
            PersianMonth.Khordad => 31,
            PersianMonth.Tir => 31,
            PersianMonth.Mordad => 31,
            PersianMonth.Sharivar => 31,
            PersianMonth.Mehr => 30,
            PersianMonth.Aban => 30,
            PersianMonth.Azar => 30,
            PersianMonth.Dey => 30,
            PersianMonth.Bahman => 30,
            PersianMonth.Esfand => (year.HasValue ? PersianDateTime.PersianCalendar.IsLeapYear(year.Value) ? 30 : 29 : 29),
            _ => 0
        };

        public static PersianDateTime ToPersianDateTime(this DateTime dateTime) => new PersianDateTime(dateTime);
    }
}
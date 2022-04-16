using Library.Globalization.DataTypes;

namespace Library.Helpers;

public static class GlobalizationHelper
{
    public static DayOfWeek ToDayOfWeek(this PersianDayOfWeek dow) =>
        dow switch
        {
            PersianDayOfWeek.YekShanbeh => DayOfWeek.Sunday,
            PersianDayOfWeek.DoShanbeh => DayOfWeek.Monday,
            PersianDayOfWeek.SeShanbeh => DayOfWeek.Tuesday,
            PersianDayOfWeek.ChaharShanbeh => DayOfWeek.Wednesday,
            PersianDayOfWeek.PanjShanbeh => DayOfWeek.Thursday,
            PersianDayOfWeek.Jomeh => DayOfWeek.Friday,
            PersianDayOfWeek.Shanbeh => DayOfWeek.Saturday,
            _ => throw new NotImplementedException(),
        };

    public static PersianDayOfWeek ToPersianDayOfWeek(this DayOfWeek dow) =>
        dow switch
        {
            DayOfWeek.Sunday => PersianDayOfWeek.YekShanbeh,
            DayOfWeek.Monday => PersianDayOfWeek.DoShanbeh,
            DayOfWeek.Tuesday => PersianDayOfWeek.SeShanbeh,
            DayOfWeek.Wednesday => PersianDayOfWeek.ChaharShanbeh,
            DayOfWeek.Thursday => PersianDayOfWeek.PanjShanbeh,
            DayOfWeek.Friday => PersianDayOfWeek.Jomeh,
            DayOfWeek.Saturday => PersianDayOfWeek.Shanbeh,
            _ => throw new NotImplementedException(),
        };

    public static bool IsPersianHoliday(this PersianDayOfWeek dow) =>
        dow switch
        {
            PersianDayOfWeek.Jomeh => true,
            PersianDayOfWeek.Shanbeh => true,
            _ => false,
        };
}
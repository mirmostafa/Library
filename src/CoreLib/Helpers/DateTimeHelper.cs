using System.Globalization;
using Library.Validations;

namespace Library.Helpers;

public static class DateTimeHelper
{
    /// <summary>
    ///     Deconstructs the specified datetime.
    /// </summary>
    /// <param name="dt"> The dt. </param>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month. </param>
    /// <param name="day"> The day. </param>
    /// <param name="hour"> The hour. </param>
    /// <param name="minute"> The minute. </param>
    /// <param name="second"> The second. </param>
    /// <param name="millisecond"> The millisecond. </param>
    public static void Deconstruct(this DateTime dt,
        out int year,
        out int month,
        out int day,
        out int hour,
        out int minute,
        out int second,
        out int millisecond)
        => (year, month, day, hour, minute, second, millisecond) = (dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

    /// <summary>
    ///     Deconstructs the specified datetime.
    /// </summary>
    /// <param name="dt"> The dt. </param>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month. </param>
    /// <param name="day"> The day. </param>
    public static void Deconstruct(this DateTime dt, out int year, out int month, out int day)
        => (year, month, day) = (dt.Year, dt.Month, dt.Day);

    /// <summary>
    ///     Deconstructs the specified datetime.
    /// </summary>
    /// <param name="dt"> The dt. </param>
    /// <param name="hour"> The hour. </param>
    /// <param name="minute"> The minute. </param>
    /// <param name="second"> The second. </param>
    /// <param name="millisecond"> The millisecond. </param>
    public static void Deconstruct(this DateTime dt, out int hour, out int minute, out int second, out int millisecond)
        => (hour, minute, second, millisecond) = (dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

    /// <summary>
    ///     Determines whether this instance start is between.
    /// </summary>
    /// <param name="source"> The source. </param>
    /// <param name="start"> The start. </param>
    /// <param name="end"> The end. </param>
    /// <returns>
    ///     <c> true </c> if the specified start is between; otherwise, <c> false </c>.
    /// </returns>
    public static bool IsBetween(this TimeSpan source, in TimeSpan start, in TimeSpan end)
        => source >= start && source <= end;

    /// <summary>
    ///     Determines whether this instance start is between.
    /// </summary>
    /// <param name="source"> The source. </param>
    /// <param name="start"> The start. </param>
    /// <param name="end"> The end. </param>
    /// <returns>
    ///     <c> true </c> if the specified start is between; otherwise, <c> false </c>.
    /// </returns>
    public static bool IsBetween(this DateTime source, in DateTime start, in DateTime end) =>
        source >= start && source <= end;

    /// <summary>
    ///     Determines whether this instance is between.
    /// </summary>
    /// <param name="source"> The source. </param>
    /// <param name="start"> The start. </param>
    /// <param name="end"> The end. </param>
    /// <returns>
    ///     <c> true </c> if the specified start is between; otherwise, <c> false </c>.
    /// </returns>
    public static bool IsBetween(this TimeSpan source, in string start, in string end)
        => source.IsBetween(ToTimeSpan(start), ToTimeSpan(end));

    /// <summary>
    ///     Returns true if dateTime format is valid.
    /// </summary>
    /// <param name="dateTime"> The date time. </param>
    /// <returns>
    ///     <c> true </c> if the specified date time is valid; otherwise, <c> false </c>.
    /// </returns>
    public static bool IsValid(in string dateTime)
        => DateTime.TryParse(dateTime, out _);

    public static bool IsWeekend([DisallowNull] this DateTime dateTime, CultureInfo? culture = null)
            => (culture ?? CultureInfo.CurrentCulture).GetWeekdayState(dateTime.ArgumentNotNull(nameof(dateTime)).DayOfWeek)
                is CultureInfoHelper.WeekdayState.Weekend or CultureInfoHelper.WeekdayState.WorkdayMorning;

    /// <summary>
    ///     Converts to datetime.
    /// </summary>
    /// <param name="source"> The source. </param>
    /// <returns> </returns>
    public static DateTime ToDateTime(this TimeSpan source)
        => new(source.Ticks);

    /// <summary>
    ///     Converts to timespan.
    /// </summary>
    /// <param name="source"> The source. </param>
    /// <returns> </returns>
    public static TimeSpan ToTimeSpan(in string source)
        => TimeSpan.Parse(source, CultureInfo.CurrentCulture);

    /// <summary>
    /// Converts to timespan.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static TimeSpan ToTimeSpan(this DateTime source)
        => new(source.Ticks);
}
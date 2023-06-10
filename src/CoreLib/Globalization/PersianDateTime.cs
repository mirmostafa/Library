using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

using Library.Globalization.DataTypes;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

namespace Library.Globalization;

// Created: 85/5/4
/// <summary>
///
/// </summary>
/// <seealso cref="System.IComparable" />
/// <seealso cref="System.IComparable&lt;System.DateTime&gt;" />
/// <seealso cref="System.IConvertible" />
/// <seealso cref="System.IEquatable&lt;System.DateTime&gt;" />
/// <seealso cref="System.ISpanFormattable" />
/// <seealso cref="System.IFormattable" />
/// <seealso cref="System.Runtime.Serialization.ISerializable" />
/// <seealso cref="System.ICloneable" />
/// <seealso cref="System.IComparable&lt;Library.Globalization.PersianDateTime&gt;" />
/// <seealso cref="System.IEquatable&lt;Library.Globalization.PersianDateTime&gt;" />
/// <seealso cref="Library.Interfaces.IConvertible&lt;Library.Globalization.PersianDateTime, System.DateTime&gt;" />
/// <seealso cref="Library.Interfaces.IConvertible&lt;Library.Globalization.PersianDateTime, string&gt;" />
/// <seealso cref="Library.Interfaces.IAdditionOperators&lt;Library.Globalization.PersianDateTime, Library.Globalization.PersianDateTime, Library.Globalization.PersianDateTime&gt;" />
/// <seealso cref="Library.Interfaces.ISubtractionOperators&lt;Library.Globalization.PersianDateTime, Library.Globalization.PersianDateTime, Library.Globalization.PersianDateTime&gt;" />
/// <seealso cref="IParsable&lt;PersianDateTime&gt;" />
[Serializable]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public readonly struct PersianDateTime :
    IComparable, IComparable<DateTime>, IConvertible, IEquatable<DateTime>, ISpanFormattable, IFormattable, ISerializable, ICloneable,
    IComparable<PersianDateTime>, IEquatable<PersianDateTime>,
    //IConvertible<PersianDateTime, DateTime>, IConvertible<PersianDateTime, string>,
    IAdditionOperators<PersianDateTime, PersianDateTime, PersianDateTime>,
    ISubtractionOperators<PersianDateTime, PersianDateTime, PersianDateTime>,
    IParsable<PersianDateTime>,
    IStaticValidator<string>
{
    #region Date/Time Elements

    /// <summary>
    /// Gets the day.
    /// </summary>
    /// <value>The day.</value>
    public int Day => this.Data.Day;

    /// <summary>
    /// Gets the day of week.
    /// </summary>
    /// <value>The day of week.</value>
    public PersianDayOfWeek DayOfWeek => (PersianDayOfWeek)PersianCalendar.GetDayOfWeek(this).Cast().ToInt();

    /// <summary>
    /// Gets the day of week title.
    /// </summary>
    /// <value>The day of week title.</value>
    public string DayOfWeekTitle => EnumHelper.GetItemDescription(this.DayOfWeek)!;

    /// <summary>
    /// Gets the hour.
    /// </summary>
    /// <value>The hour.</value>
    public int Hour => this.Data.Hour;

    public double Millisecond => this.Data.Millisecond;

    /// <summary>
    /// Gets the minute.
    /// </summary>
    /// <value>The minute.</value>
    public int Minute => this.Data.Minute;

    /// <summary>
    /// Gets the month.
    /// </summary>
    /// <value>The month.</value>
    public int Month => this.Data.Month;

    /// <summary>
    /// Gets the second.
    /// </summary>
    /// <value>The second.</value>
    public int Second => this.Data.Second;

    /// <summary>
    /// Returns the tick count for this PersianDateTime. The returned value is the number of
    /// 100-nanosecond intervals that have elapsed since 1/1/0001 12:00am.
    /// </summary>
    public long Ticks => ((DateTime)this).Ticks;

    public int Year => this.Data.Year;

    #endregion Date/Time Elements

    #region Fields

    internal static PersianCalendar PersianCalendar = new();
    internal readonly PersianDateTimeData Data;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="PersianDateTime"/> struct.
    /// </summary>
    /// <param name="dateTime">A DateTime instance.</param>
    public PersianDateTime(in DateTime dateTime)
    {
        this.Data = new PersianDateTimeData
        {
            Year = PersianCalendar.GetYear(dateTime),
            Month = PersianCalendar.GetMonth(dateTime),
            Day = PersianCalendar.GetDayOfMonth(dateTime),
            Hour = PersianCalendar.GetHour(dateTime),
            Minute = PersianCalendar.GetMinute(dateTime),
            Second = PersianCalendar.GetSecond(dateTime),
            Millisecond = PersianCalendar.GetMilliseconds(dateTime)
        };
        this.IsInitiated = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersianDateTime"/> struct.
    /// </summary>
    /// <param name="year">       The year.</param>
    /// <param name="month">      The month.</param>
    /// <param name="day">        The day.</param>
    /// <param name="hour">       The hour.</param>
    /// <param name="minute">     The minute.</param>
    /// <param name="second">     The second.</param>
    /// <param name="millisecond"></param>
    public PersianDateTime(in int year, in int month, in int day, in int hour, in int minute, in int second, in double millisecond)
    {
        this.Data = new PersianDateTimeData
        {
            Year = year,
            Month = month,
            Day = day,
            Hour = hour,
            Minute = minute,
            Second = second,
            Millisecond = millisecond
        };
        this.IsInitiated = true;
    }

    /// <summary>
    /// Initializes a new instance of the PersianDateTime class with the specified year, month and day
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="day">The day (1 through the number of days in month).</param>
    /// <returns>A new instance of the PersianDateTime class.</returns>
    public PersianDateTime(in int year, in int month, in int day)
            : this(year, month, day, 0, 0, 0, 0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersianDateTime" /> struct.
    /// </summary>
    /// <param name="data">The data.</param>
    private PersianDateTime(in PersianDateTimeData data)
        => (this.Data, this.IsInitiated) = (data, true);

    /// <summary>
    /// Initializes a new instance of the <see cref="PersianDateTime"/> struct.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="context">The context.</param>
    private PersianDateTime(in SerializationInfo info, in StreamingContext context)
    {
        Check.IfArgumentNotNull(info);

        this.Data = (PersianDateTimeData)info.GetValue("Data", typeof(PersianDateTimeData))!;
        this.IsInitiated = true;
    }

    /// <summary>
    /// Gets or Sets the date separator.
    /// </summary>
    public static string DateSeparator { get; set; } = CultureConstants.DATE_SEPARATOR;

    /// <summary>
    /// Gets the days of week names.
    /// </summary>
    /// <value>The month names.</value>
    public static IEnumerable<string> DaysOfWeek => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianDayOfWeek>())!;

    /// <summary>
    /// Gets the month names.
    /// </summary>
    /// <value>The month names.</value>
    public static IEnumerable<string> MonthNames => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianMonth>())!;

    /// <summary>
    /// Gets a <see cref="T:HanyCo.Mes20.Infra.Globalization.PersianDateTime"/> object that is set
    /// to the current date and time on this computer, expressed as the local time.
    /// </summary>
    /// <returns>An object whose value is the current local date and time.</returns>
    public static PersianDateTime Now => DateTime.Now;

    /// <summary>
    /// Gets the Persian week days.
    /// </summary>
    /// <value>The Persian week days.</value>
    public static IEnumerable<(DayOfWeek Day, string Name)> PersianWeekDays
    {
        get
        {
            yield return (System.DayOfWeek.Saturday, CultureConstants.SHANBE_FA);
            yield return (System.DayOfWeek.Sunday, CultureConstants.YEK_SHANBE_FA);
            yield return (System.DayOfWeek.Monday, CultureConstants.DO_SHANBE_FA);
            yield return (System.DayOfWeek.Tuesday, CultureConstants.SE_SHANBE_FA);
            yield return (System.DayOfWeek.Wednesday, CultureConstants.CHAHAR_SHANBE_FA);
            yield return (System.DayOfWeek.Thursday, CultureConstants.PANJ_SHANBE_FA);
            yield return (System.DayOfWeek.Friday, CultureConstants.JOME_FA);
        }
    }

    /// <summary>
    /// Gets the Persian week days abbreviations.
    /// </summary>
    /// <value>The Persian week days abbreviations.</value>
    public static IEnumerable<(DayOfWeek Day, string Name)> PersianWeekDaysAbbrs
    {
        get
        {
            yield return (System.DayOfWeek.Saturday, CultureConstants.SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Sunday, CultureConstants.YEK_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Monday, CultureConstants.DO_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Tuesday, CultureConstants.SE_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Wednesday, CultureConstants.CHAHAR_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Thursday, CultureConstants.PANJ_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Friday, CultureConstants.JOME_ABBR_FA);
        }
    }

    /// <summary>
    /// Gets or sets the short date pattern.
    /// </summary>
    /// <value>
    /// The short date pattern.
    /// </value>
    public static string ShortDatePattern { get; set; } = CultureConstants.SHORT_DATE_PATTERN;

    /// <summary>
    /// Gets or sets the time separator.
    /// </summary>
    /// <value>
    /// The time separator.
    /// </value>
    public static string TimeSeparator { get; set; } = CultureConstants.TIME_SEPARATOR;

    /// <summary>
    /// Gets or sets the string format used when converting a PersianDateTime object to a string.
    /// </summary>
    public static string ToStringFormat { get; set; } = CultureConstants.DEFAULT_DATE_TIME_PATTERN;

    /// <summary>
    /// Gets a System.DateTime object that is set to the current date and time on this computer,
    /// expressed as the Coordinated Universal Time (UTC) in Persian.
    /// </summary>
    /// <value>An object whose value is the current UTC date and time in Persian.</value>
    public static PersianDateTime UtcNow => DateTime.UtcNow;

    /// <summary>
    /// Gets a value indicating whether this date is holiday.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is holiday; otherwise, <c>false</c>.
    /// </value>
    public bool IsHoliday => this.DayOfWeek.IsPersianHoliday();

    /// <summary>
    /// Gets a value indicating whether this instance is initiated.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is initiated; otherwise, <c>false</c>.
    /// </value>
    public bool IsInitiated { get; }

    /// <summary>
    /// Gets the year.
    /// </summary>
    /// <value>The year.</value>

    internal static IEnumerable<string> EnglishMonthNameAbbrsInPersian
    {
        get
        {
            yield return CultureConstants.FARVARDIN_ABBR_EN;
            yield return CultureConstants.ORDIBEHESHT_ABBR_EN;
            yield return CultureConstants.KHORDAD_ABBR_EN;
            yield return CultureConstants.TIR_ABBR_EN;
            yield return CultureConstants.MORDAD_ABBR_EN;
            yield return CultureConstants.SHAHRIVAR_ABBR_EN;
            yield return CultureConstants.MEHR_ABBR_EN;
            yield return CultureConstants.ABAN_ABBR_EN;
            yield return CultureConstants.AZAR_ABBR_EN;
            yield return CultureConstants.DEY_ABBR_EN;
            yield return CultureConstants.BAHMAN_ABBR_EN;
            yield return CultureConstants.ESFAND_ABBR_EN;
        }
    }

    /// <summary>
    /// Gets the persian month name abbrs in persian.
    /// </summary>
    /// <value>
    /// The persian month name abbrs in persian.
    /// </value>
    internal static IEnumerable<string> PersianMonthNameAbbrsInPersian
    {
        get
        {
            yield return CultureConstants.FARVARDIN_ABBR_FA;
            yield return CultureConstants.ORDIBEHESHT_ABBR_FA;
            yield return CultureConstants.KHORDAD_ABBR_FA;
            yield return CultureConstants.TIR_ABBR_FA;
            yield return CultureConstants.MORDAD_ABBR_FA;
            yield return CultureConstants.SHAHRIVAR_ABBR_FA;
            yield return CultureConstants.MEHR_ABBR_FA;
            yield return CultureConstants.ABAN_ABBR_FA;
            yield return CultureConstants.AZAR_ABBR_FA;
            yield return CultureConstants.DEY_ABBR_FA;
            yield return CultureConstants.BAHMAN_ABBR_FA;
            yield return CultureConstants.ESFAND_ABBR_FA;
        }
    }

    internal static IEnumerable<string> PersianMonthNamesInGenitive
    {
        get
        {
            yield return CultureConstants.FARVARDIN_EN;
            yield return CultureConstants.ORDIBEHESHT_EN;
            yield return CultureConstants.KHORDAD_EN;
            yield return CultureConstants.TIR_EN;
            yield return CultureConstants.MORDAD_EN;
            yield return CultureConstants.SHAHRIVAR_EN;
            yield return CultureConstants.MEHR_EN;
            yield return CultureConstants.ABAN_EN;
            yield return CultureConstants.AZAR_EN;
            yield return CultureConstants.DEY_EN;
            yield return CultureConstants.BAHMAN_EN;
            yield return CultureConstants.ESFAND_EN;
        }
    }

    internal static IEnumerable<string> PersianMonthNamesInPersian
    {
        get
        {
            yield return CultureConstants.FARVARDIN_FA;
            yield return CultureConstants.ORDIBEHESHT_FA;
            yield return CultureConstants.KHORDAD_FA;
            yield return CultureConstants.TIR_FA;
            yield return CultureConstants.MORDAD_FA;
            yield return CultureConstants.SHAHRIVAR_FA;
            yield return CultureConstants.MEHR_FA;
            yield return CultureConstants.ABAN_FA;
            yield return CultureConstants.AZAR_FA;
            yield return CultureConstants.DEY_FA;
            yield return CultureConstants.BAHMAN_FA;
            yield return CultureConstants.ESFAND_FA;
        }
    }

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <value>
    /// The debugger display.
    /// </value>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string? DebuggerDisplay => this.ToString();

    /// <summary>
    /// Adds the specified Persian date time1.
    /// </summary>
    /// <param name="PersianDateTime1">The Persian date time1.</param>
    /// <param name="PersianDateTime2">The Persian date time2.</param>
    /// <returns></returns>
    public static PersianDateTime Add(in PersianDateTime PersianDateTime1, in PersianDateTime PersianDateTime2)
        => PersianDateTime1 + PersianDateTime2;

    /// <summary>
    /// Compares the specified Persian date time1.
    /// </summary>
    /// <param name="PersianDateTime1">The Persian date time1.</param>
    /// <param name="PersianDateTime2">The Persian date time2.</param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException">
    /// cannot cast PersianDateTime1 to PersianDateTime or cannot cast PersianDateTime2 to PersianDateTime
    /// </exception>
    public static int Compare(in string PersianDateTime1, in string PersianDateTime2)
    {
        Check.If(TryParse(PersianDateTime1, out var p1), () => new InvalidCastException($"cannot cast {nameof(PersianDateTime1)} to PersianDateTime"));
        Check.If(TryParse(PersianDateTime2, out var p2), () => new InvalidCastException($"cannot cast {nameof(PersianDateTime2)} to PersianDateTime"));

        return p1.CompareTo(p2);
    }

    public static PersianDateTime From([DisallowNull] DateTime other)
        => other;

    public static PersianDateTime From([DisallowNull] string other)
        => other;

    /// <summary>
    /// Performs an implicit conversion from <see cref="PersianDateTime"/> to <see cref="DateTime"/>.
    /// </summary>
    /// <param name="PersianDateTime">The Persian date time.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator DateTime(PersianDateTime PersianDateTime)
        => PersianCalendar.ToDateTime(
            PersianDateTime.Year,
            PersianDateTime.Month,
            PersianDateTime.Day,
            PersianDateTime.Hour,
            PersianDateTime.Minute,
            PersianDateTime.Second == -1 ? 0 : PersianDateTime.Second,
            0);

    public static implicit operator PersianDateTime(string PersianDateTimeString)
        => ParsePersian(PersianDateTimeString);

    /// <summary>
    /// Performs an implicit conversion from <see cref="DateTime"/> to <see cref="PersianDateTime"/>.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator PersianDateTime(DateTime dateTime)
        => dateTime.ToPersianDateTime();

    /// <summary>
    /// Performs an implicit conversion from <see cref="PersianDateTime"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="PersianDateTime">The Persian date time.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator string(PersianDateTime PersianDateTime)
        => PersianDateTime.ToString();

    public static bool IsPersianDateTime(in string s)
        => Validate(s).IsSucceed;

    /// <summary>
    /// Implements the operator -.
    /// </summary>
    /// <param name="PersianDateTime1">The Persian date time1.</param>
    /// <param name="PersianDateTime2">The Persian date time2.</param>
    /// <returns>The result of the operator.</returns>
    public static PersianDateTime operator -(PersianDateTime PersianDateTime1, PersianDateTime PersianDateTime2)
    {
        DateTime dateTime1 = PersianDateTime1;
        DateTime dateTime2 = PersianDateTime2;
        return dateTime1.Subtract(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
    }

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="PersianDateTime1">The Persian date time1.</param>
    /// <param name="PersianDateTime2">The Persian date time2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(in PersianDateTime PersianDateTime1, in PersianDateTime PersianDateTime2)
        => !PersianDateTime1.Equals(PersianDateTime2);

    /// <summary>
    /// Implements the operator +.
    /// </summary>
    /// <param name="PersianDateTime1">The Persian date time1.</param>
    /// <param name="PersianDateTime2">The Persian date time2.</param>
    /// <returns>The result of the operator.</returns>
    public static PersianDateTime operator +(PersianDateTime PersianDateTime1, PersianDateTime PersianDateTime2)
    {
        DateTime dateTime1 = PersianDateTime1;
        DateTime dateTime2 = PersianDateTime2;
        return dateTime1.Add(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
    }

    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="left"> The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <(in PersianDateTime left, in PersianDateTime right)
        => left.CompareTo(right) < 0;

    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <(in PersianDateTime left, in DateTime right)
        => left.CompareTo(right) < 0;

    /// <summary>
    /// Implements the operator &lt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <=(in PersianDateTime left, in PersianDateTime right)
        => left.CompareTo(right) <= 0;

    /// <summary>
    /// Implements the operator &lt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <=(in PersianDateTime left, in DateTime right)
        => left.CompareTo(right) <= 0;

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="PersianDateTime1">The Persian date time1.</param>
    /// <param name="PersianDateTime2">The Persian date time2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(in PersianDateTime PersianDateTime1, in PersianDateTime PersianDateTime2)
        => PersianDateTime1.Equals(PersianDateTime2);

    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="left"> The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >(in PersianDateTime left, in PersianDateTime right)
        => left.CompareTo(right) > 0;

    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >(in PersianDateTime left, in DateTime right)
        => left.CompareTo(right) > 0;

    /// <summary>
    /// Implements the operator &gt;=.
    /// </summary>
    /// <param name="left"> The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >=(in PersianDateTime left, in PersianDateTime right)
        => left.CompareTo(right) >= 0;

    /// <summary>
    /// Implements the operator &gt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >=(in PersianDateTime left, in DateTime right)
        => left.CompareTo(right) >= 0;

    /// <summary>
    /// Parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <returns>
    /// The result of parsing <paramref name="s" />.
    /// </returns>
    static PersianDateTime IParsable<PersianDateTime>.Parse(string s, IFormatProvider? provider)
        => s;

    /// <summary>
    /// Parses the date time.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    public static PersianDateTime ParseDateTime(in DateTime dateTime)
        => new(dateTime);

    /// <summary>
    /// Parses the English string datetime.
    /// </summary>
    /// <param name="dateTimeString">The date time string.</param>
    /// <returns></returns>
    public static PersianDateTime ParseEnglish(in string dateTimeString)
    {
        Check.IfArgumentNotNull(dateTimeString);
        return DateTime.Parse(dateTimeString, CultureInfo.CurrentCulture).ToPersianDateTime();
    }

    /// <summary>
    /// Parses a string to a PersianDateTime object.
    /// </summary>
    /// <param name="dateTimeString">The string to parse.</param>
    /// <returns>A PersianDateTime object.</returns>
    public static PersianDateTime ParsePersian(in string dateTimeString)
    {
        _ = Validate(dateTimeString).ThrowOnFail();

        var indexOfSpace = dateTimeString.IndexOf(" ", StringComparison.Ordinal);
        var hasDate = dateTimeString.IndexOf(DateSeparator, StringComparison.Ordinal) > 0;
        var hasTime = dateTimeString.IndexOf(":", StringComparison.Ordinal) > 0;
        int year = 0, month = 0, day = 0, hour = 0, min = 0, sec = 0;

        if (hasDate)
        {
            var datePart = hasTime ? dateTimeString[..indexOfSpace] : dateTimeString;
            if (int.TryParse(datePart.AsSpan(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal)), out year) is false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }

            datePart = datePart.Remove(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal) + 1);
            if (int.TryParse(datePart.AsSpan(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal)), out month) is false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }

            datePart = datePart.Remove(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal) + 1);
            if (int.TryParse(datePart, out day) is false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }
        }

        if (hasTime)
        {
            var timePart = hasDate ? dateTimeString[indexOfSpace..] : dateTimeString;
            var timeParts = timePart.Split(TimeSeparator);
            if (timeParts.Length > 0)
            {
                hour = CodeHelper.ThrowOnError(() => int.Parse(timeParts[0]), _ => new ArgumentException("invalid time format", nameof(dateTimeString)));
            }

            if (timePart.Length > 1)
            {
                min = CodeHelper.ThrowOnError(() => int.Parse(timeParts[1]), _ => new ArgumentException("invalid time format", nameof(dateTimeString)));
            }

            if (timeParts.Length > 2)
            {
                sec = CodeHelper.ThrowOnError(() => int.Parse(timeParts[2]), _ => new ArgumentException("invalid time format", nameof(dateTimeString)));
            }
        }

        var data = new PersianDateTimeData
        {
            HasDate = hasDate,
            HasTime = hasTime,
            Year = year,
            Month = month,
            Day = day,
            Hour = hour,
            Minute = min,
            Second = sec
        };

        return data.ToString()?.Equals("00:00:00 0000/00/00", StringComparison.Ordinal) switch
        {
            not false => throw new ArgumentException("not valid date", nameof(dateTimeString)),
            _ => new PersianDateTime(data)
        };
    }

    /// <summary>
    /// Subtracts two PersianDateTime objects and returns the result.
    /// </summary>
    public static PersianDateTime Subtract(in PersianDateTime PersianDateTime1, in PersianDateTime PersianDateTime2)
        => PersianDateTime1 - PersianDateTime2;

    /// <summary>
    /// Converts a DateTime object to a PersianDateTime string.
    /// </summary>
    public static string ToPersian(in DateTime dateTime)
        => new PersianDateTime(dateTime).ToString()!;

    /// <summary>
    /// Tries to parse a string to a PersianDateTime object.
    /// </summary>
    /// <param name="str">The string to parse.</param>
    /// <param name="result">The parsed PersianDateTime object.</param>
    /// <returns>True if the string was successfully parsed, false otherwise.</returns>
    public static bool TryParse(in string str, out PersianDateTime result)
    {
        try
        {
            result = ParsePersian(str);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    /// <summary>
    /// Tries to parse the specified string to a PersianDateTime object.
    /// </summary>
    /// <param name="str">The string to parse.</param>
    /// <returns>A TryMethodResult containing the parsed PersianDateTime object.</returns>
    public static TryMethodResult<PersianDateTime> TryParse(in string str)
        => TryMethodResult<PersianDateTime>.TryParseResult(TryParse(str, out var result), result);

    /// <summary>
    /// Tries to parse the specified string into a PersianDateTime object.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">The format provider.</param>
    /// <param name="result">The parsed PersianDateTime object.</param>
    /// <returns>True if the string was successfully parsed, false otherwise.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out PersianDateTime result)
        => TryParse(s.ArgumentNotNull(), out result);

    /// <summary>
    /// Validates a string to check if it is a valid Persian date or time.
    /// </summary>
    /// <param name="item">The string to validate.</param>
    /// <returns>A Result object indicating whether the string is a valid date.</returns>
    public static Result<string?> Validate([DisallowNull] in string? item)
    {
        var regex = new Regex(@"^(\d{4})\/(0?[1-9]|1[012])\/(0?[1-9]|[12][0-9]|3[01])(\s([01]?\d|2[0-3]):([0-5]?\d)(:([0-5]?\d))?\s?(AM|PM)?)?$");
        return item.Check()
            .ArgumentNotNull()
            .RuleFor(regex.IsMatch, () => "Not valid date")
            .Build();
    }

    /// <summary>
    /// Adds the specified PersianDate time to current instance.
    /// </summary>
    /// <param name="PersianDateTime">The PersianDate time.</param>
    /// <returns></returns>
    public PersianDateTime Add(in PersianDateTime PersianDateTime)
        => Add(this, PersianDateTime);

    /// <summary>
    /// Adds the specified number of days to the current PersianDateTime object.
    /// </summary>
    public PersianDateTime AddDays(in int value)
        => this.ToDateTime().AddDays(value).ToPersianDateTime();

    /// <summary>
    /// Adds the specified number of hours to the current PersianDateTime object.
    /// </summary>
    public PersianDateTime AddHours(in int value)
        => this.ToDateTime().AddHours(value).ToPersianDateTime();

    /// <summary>
    /// Adds the specified number of milliseconds to the current PersianDateTime object.
    /// </summary>
    public PersianDateTime AddMilliseconds(in int value)
        => this.ToDateTime().AddMilliseconds(value).ToPersianDateTime();

    /// <summary>
    /// Adds the specified number of minutes to the current PersianDateTime object.
    /// </summary>
    public PersianDateTime AddMinutes(in int value)
        => this.ToDateTime().AddMinutes(value).ToPersianDateTime();

    /// <summary>
    /// Adds the specified number of months to the current PersianDateTime object.
    /// </summary>
    public PersianDateTime AddMonths(in int value)
        => this.ToDateTime().AddMonths(value).ToPersianDateTime();

    /// <summary>
    /// Adds the specified number of seconds to the current PersianDateTime object.
    /// </summary>
    public PersianDateTime AddSeconds(in int value)
        => this.ToDateTime().AddSeconds(value).ToPersianDateTime();

    /// <summary>
    /// Adds the specified number of ticks to the current PersianDateTime object.
    /// </summary>
    public PersianDateTime AddTocks(in long ticks)
        => this.ToDateTime().AddTicks(ticks).ToPersianDateTime();

    /// <summary>
    /// Adds the specified number of years to the current PersianDateTime object.
    /// </summary>
    public PersianDateTime AddYears(in int value)
        => this.ToDateTime().AddYears(value).ToPersianDateTime();

    /// <summary>
    /// Creates a new PersianDateTime object with the same values as the current PersianDateTime object.
    /// </summary>
    public object Clone()
        => new PersianDateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second, this.Millisecond);

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer
    /// that indicates whether the current instance precedes, follows, or occurs in the same
    /// position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value
    /// has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order. Zero This instance occurs in the same position in the sort
    /// order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.
    /// </returns>

    public int CompareTo(PersianDateTime other)
        => DateTime.Compare(this, other);

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="obj" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="obj" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="obj" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo(object? obj)
        => obj is PersianDateTime pdt ? this.CompareTo(pdt) : 1;

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo(DateTime other)
        => ((PersianDateTime)other).CompareTo(this);

    public DateTime ConvertTo()
        => this;

    /// <summary>
    /// Deconstructs this instance.
    /// </summary>
    /// <param name="hour">       The hour.</param>
    /// <param name="minute">     The minute.</param>
    /// <param name="second">     The second.</param>
    /// <param name="millisecond">The millisecond.</param>
    public void Deconstruct(out int hour, out int minute, out int second, out double millisecond)
    {
        hour = this.Hour;
        minute = this.Minute;
        second = this.Second;
        millisecond = this.Millisecond;
    }

    /// <summary>
    /// Deconstructs this instance.
    /// </summary>
    /// <param name="year"> The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="day">  The day.</param>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = this.Year;
        month = this.Month;
        day = this.Day;
    }

    /// <summary>
    /// Deconstructs this instance.
    /// </summary>
    /// <param name="year">       The year.</param>
    /// <param name="month">      The month.</param>
    /// <param name="day">        The day.</param>
    /// <param name="hour">       The hour.</param>
    /// <param name="minute">     The minute.</param>
    /// <param name="second">     The second.</param>
    /// <param name="millisecond">The millisecond.</param>
    public void Deconstruct(out int year, out int month, out int day, out int hour, out int minute, out int second, out double millisecond)
    {
        year = this.Year;
        month = this.Month;
        day = this.Day;
        hour = this.Hour;
        minute = this.Minute;
        second = this.Second;
        millisecond = this.Millisecond;
    }

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
        => obj is PersianDateTime target && this.CompareTo(target) == 0;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(PersianDateTime other)
        => this.CompareTo(other) == 0;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(DateTime other)
        => ((PersianDateTime)other).Equals(this);

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures
    /// like a hash table.
    /// </returns>
    public override int GetHashCode()
        => this.Data.GetHashCode();

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => info.AddValue("Data", this.Data);

    TypeCode IConvertible.GetTypeCode()
        => throw new NotSupportedException();

    bool IConvertible.ToBoolean(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    byte IConvertible.ToByte(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    char IConvertible.ToChar(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    /// <summary>
    /// Converts the DateTime object to a string with the specified separator.
    /// </summary>
    /// <param name="separator">The separator to use between the year, month and day.</param>
    /// <returns>A string representation of the DateTime object.</returns>
    public string ToDateString(in string? separator = null)
    {
        var buffer = separator ?? DateSeparator;

        return string.Concat(this.Year.ToString("0000", CultureInfo.CurrentCulture),
            buffer,
            this.Month.ToString("00", CultureInfo.CurrentCulture),
            buffer,
            this.Day.ToString("00", CultureInfo.CurrentCulture));
    }

    /// <summary>
    /// Converts the current instance to a DateTime object.
    /// </summary>
    public DateTime ToDateTime()
        => this;

    /// <summary>
    /// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using
    /// the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">
    /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
    /// culture-specific formatting information.
    /// </param>
    /// <returns>A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.</returns>
    public DateTime ToDateTime(IFormatProvider? provider)
        => this;

    decimal IConvertible.ToDecimal(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    public string? ToDefaultFomratString()
        => this.ToString("yyyy/MM/dd HH:mm:ss");

    double IConvertible.ToDouble(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    short IConvertible.ToInt16(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    int IConvertible.ToInt32(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    long IConvertible.ToInt64(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    sbyte IConvertible.ToSByte(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    float IConvertible.ToSingle(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString()
        => this.ToString(ToStringFormat);

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public string ToString(in string format)
    {
        if (this.IsInitiated is false)
        {
            return this.ToString();
        }

        Check.IfArgumentNotNull(format);
        var buffer = format;
        var isPm = this.Hour > 12;
        var tmpHrs = isPm ? this.Hour - 12 : this.Hour;
        var result = buffer.Trim()
            .Replace("yyyy", this.Year.ToString("0000", CultureInfo.CurrentCulture), StringComparison.Ordinal)
            .Replace("MMMM", MonthNames.ToArray()[this.Month - 1], StringComparison.Ordinal)
            .Replace("MM", this.Month.ToString("00", CultureInfo.CurrentCulture), StringComparison.Ordinal)
            .Replace("dd", this.Day.ToString("00", CultureInfo.CurrentCulture), StringComparison.Ordinal)
            .Replace("HH", this.Hour.ToString("00", CultureInfo.CurrentCulture), StringComparison.Ordinal)
            .Replace("hh", tmpHrs.ToString("00", CultureInfo.CurrentCulture), StringComparison.Ordinal)
            .Replace("mm", this.Minute.ToString("00", CultureInfo.CurrentCulture), StringComparison.Ordinal)
            .Replace("ss", this.Second.ToString("00", CultureInfo.CurrentCulture), StringComparison.Ordinal)
            .Replace("tt", isPm ? CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator : CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator, StringComparison.Ordinal);

        return result;
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => throw new NotImplementedException();

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    string IConvertible.ToString(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    /// <summary>
    /// Converts to Persian time string.
    /// </summary>
    /// <param name="separator">The separator.</param>
    /// <returns></returns>
    public string ToTimeString(in string? separator = null)
    {
        var sep = separator ?? TimeSeparator;
        return string.Concat(this.Hour.ToString("00", CultureInfo.CurrentCulture),
            sep,
            this.Minute.ToString("00", CultureInfo.CurrentCulture),
            sep,
            this.Second.ToString("00", CultureInfo.CurrentCulture));
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    ushort IConvertible.ToUInt16(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    uint IConvertible.ToUInt32(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    ulong IConvertible.ToUInt64(IFormatProvider? provider)
        => throw GetInvalidTypeCastException();

    /// <summary>
    /// Tries to format the value of the current instance into the provided span of characters.
    /// </summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    ///   <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => throw new NotImplementedException();

    /// <summary>
    /// Raises the invalid type cast exception.
    /// </summary>
    /// <returns></returns>
    [DoesNotReturn]
    private static InvalidCastException GetInvalidTypeCastException()
    {
        var targetType = CodeHelper.GetCallerMethodName()![2..];
        throw new InvalidCastException($"Unable to cast PersianDateTime to {targetType}");
    }

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    private string GetDebuggerDisplay()
        => this.ToString()!;
}
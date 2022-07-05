using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using Library.Globalization.DataTypes;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

namespace Library.Globalization;

// Created: 85/5/4
[Serializable]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public readonly struct PersianDateTime :
    IComparable, IComparable<DateTime>, IConvertible, IEquatable<DateTime>, ISpanFormattable, IFormattable, ISerializable,
    ICloneable, IComparable<PersianDateTime>, IEquatable<PersianDateTime>,
    IConvertible<PersianDateTime, DateTime>, IConvertible<PersianDateTime, string>
{
    #region Date/Time Elements

    /// <summary>
    ///     Gets the day.
    /// </summary>
    /// <value> The day. </value>
    public int Day => this.Data.Day;

    /// <summary>
    ///     Gets the day of week.
    /// </summary>
    /// <value> The day of week. </value>
    public PersianDayOfWeek DayOfWeek => (PersianDayOfWeek)PersianCalendar.GetDayOfWeek(this).ToInt();

    /// <summary>
    ///     Gets the day of week title.
    /// </summary>
    /// <value> The day of week title. </value>
    public string DayOfWeekTitle => EnumHelper.GetItemDescription(this.DayOfWeek)!;

    /// <summary>
    ///     Gets the hour.
    /// </summary>
    /// <value> The hour. </value>
    public int Hour => this.Data.Hour;

    public double Millisecond => this.Data.Millisecond;

    /// <summary>
    ///     Gets the minute.
    /// </summary>
    /// <value> The minute. </value>
    public int Minute => this.Data.Minute;

    /// <summary>
    ///     Gets the month.
    /// </summary>
    /// <value> The month. </value>
    public int Month => this.Data.Month;

    /// <summary>
    ///     Gets the second.
    /// </summary>
    /// <value> The second. </value>
    public int Second => this.Data.Second;

    /// <summary>
    ///     Returns the tick count for this PersianDateTime. The returned value is the number of
    ///     100-nanosecond intervals that have elapsed since 1/1/0001 12:00am.
    /// </summary>
    public long Ticks => ((DateTime)this).Ticks;

    public int Year => this.Data.Year;

    #endregion Date/Time Elements

    #region Fields

    internal static PersianCalendar PersianCalendar = new();
    internal readonly PersianDateTimeData Data;

    #endregion Fields

    /// <summary>
    ///     Initializes a new instance of the <see cref="PersianDateTime" /> struct.
    /// </summary>
    /// <param name="dateTime"> A DateTime instance. </param>
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
    ///     Initializes a new instance of the <see cref="PersianDateTime" /> struct.
    /// </summary>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month. </param>
    /// <param name="day"> The day. </param>
    /// <param name="hour"> The hour. </param>
    /// <param name="minute"> The minute. </param>
    /// <param name="second"> The second. </param>
    /// <param name="millisecond"> </param>
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

    private PersianDateTime(in PersianDateTimeData data)
        => (this.Data, this.IsInitiated) = (data, true);

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
    ///     Gets the days of week names.
    /// </summary>
    /// <value> The month names. </value>
    public static IEnumerable<string> DaysOfWeek => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianDayOfWeek>())!;

    /// <summary>
    ///     Gets the month names.
    /// </summary>
    /// <value> The month names. </value>
    public static IEnumerable<string> MonthNames => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianMonth>())!;

    /// <summary>
    ///     Gets a <see cref="T:HanyCo.Mes20.Infra.Globalization.PersianDateTime" /> object that is
    ///     set to the current date and time on this computer, expressed as the local time.
    /// </summary>
    /// <returns> An object whose value is the current local date and time. </returns>
    public static PersianDateTime Now => new(DateTime.Now);

    /// <summary>
    ///     Gets the persian week days.
    /// </summary>
    /// <value> The persian week days. </value>
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
    ///     Gets the persian week days abbreviations.
    /// </summary>
    /// <value> The persian week days abbreviations. </value>
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

    public static string ShortDatePattern { get; set; } = CultureConstants.SHORT_DATE_PATTERN;
    public static string TimeSeparator { get; set; } = CultureConstants.TIME_SEPARATOR;
    public static string ToStringFormat { get; set; } = CultureConstants.DEFAULT_DATE_TIME_PATTERN;

    public bool IsHoliday => this.DayOfWeek.IsPersianHoliday();
    public bool IsInitiated { get; }

    /// <summary>
    ///     Gets the year.
    /// </summary>
    /// <value> The year. </value>

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

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string? DebuggerDisplay => this.ToString();

    /// <summary>
    ///     Adds the specified persian date time1.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> </returns>
    public static PersianDateTime Add(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        => persianDateTime1 + persianDateTime2;

    /// <summary>
    ///     Compares the specified persian date time1.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> </returns>
    /// <exception cref="InvalidCastException">
    ///     cannot cast persianDateTime1 to PersianDateTime or cannot cast persianDateTime2 to PersianDateTime
    /// </exception>
    public static int Compare(in string persianDateTime1, in string persianDateTime2)
    {
        Check.MustBe(TryParse(persianDateTime1, out var p1), () => new InvalidCastException("cannot cast persianDateTime1 to PersianDateTime"));
        Check.MustBe(TryParse(persianDateTime2, out var p2), () => new InvalidCastException("cannot cast persianDateTime2 to PersianDateTime"));

        return p1.CompareTo(p2);
    }

    public static PersianDateTime ConvertFrom([DisallowNull] DateTime other) => other;

    public static PersianDateTime ConvertFrom([DisallowNull] string other) => other;

    /// <summary>
    ///     Performs an implicit conversion from <see cref="PersianDateTime" /> to <see cref="DateTime" />.
    /// </summary>
    /// <param name="persianDateTime"> The persian date time. </param>
    /// <returns> The result of the conversion. </returns>
    public static implicit operator DateTime(in PersianDateTime persianDateTime) => PersianCalendar.ToDateTime(
        persianDateTime.Year,
        persianDateTime.Month,
        persianDateTime.Day,
        persianDateTime.Hour,
        persianDateTime.Minute,
        persianDateTime.Second == -1 ? 0 : persianDateTime.Second,
        0);

    public static implicit operator PersianDateTime(in string persianDateTimeString)
        => ParsePersian(persianDateTimeString);

    /// <summary>
    ///     Performs an implicit conversion from <see cref="DateTime" /> to <see cref="PersianDateTime" />.
    /// </summary>
    /// <param name="dateTime"> The date time. </param>
    /// <returns> The result of the conversion. </returns>
    public static implicit operator PersianDateTime(in DateTime dateTime) => dateTime.ToPersianDateTime();

    /// <summary>
    ///     Performs an implicit conversion from <see cref="PersianDateTime" /> to <see cref="string" />.
    /// </summary>
    /// <param name="persianDateTime"> The persian date time. </param>
    /// <returns> The result of the conversion. </returns>
    public static implicit operator string(in PersianDateTime persianDateTime) => persianDateTime.ToString();

    public static bool IsPersianDateTime(in string s) =>
                                TryParse(s).IsSucceed;

    /// <summary>
    ///     Implements the operator -.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> The result of the operator. </returns>
    public static PersianDateTime operator -(in PersianDateTime persianDateTime1,
        in PersianDateTime persianDateTime2)
    {
        DateTime dateTime1 = persianDateTime1;
        DateTime dateTime2 = persianDateTime2;
        return dateTime1.Subtract(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
    }

    /// <summary>
    ///     Implements the operator !=.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator !=(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        => !persianDateTime1.Equals(persianDateTime2);

    /// <summary>
    ///     Implements the operator +.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> The result of the operator. </returns>
    public static PersianDateTime operator +(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
    {
        DateTime dateTime1 = persianDateTime1;
        DateTime dateTime2 = persianDateTime2;
        return dateTime1.Add(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
    }

    /// <summary>
    ///     Implements the operator &lt;.
    /// </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator <(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) < 0;

    public static bool operator <(in PersianDateTime left, in DateTime right) =>
            left.CompareTo(right) < 0;

    /// <summary>
    ///     Implements the operator &lt;=.
    /// </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator <=(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) <= 0;

    public static bool operator <=(in PersianDateTime left, in DateTime right) =>
            left.CompareTo(right) <= 0;

    /// <summary>
    ///     Implements the operator ==.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator ==(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        => persianDateTime1.Equals(persianDateTime2);

    /// <summary>
    ///     Implements the operator &gt;.
    /// </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator >(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) > 0;

    public static bool operator >(in PersianDateTime left, in DateTime right) =>
            left.CompareTo(right) > 0;

    /// <summary>
    ///     Implements the operator &gt;=.
    /// </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator >=(in PersianDateTime left, in PersianDateTime right) =>
        left.CompareTo(right) >= 0;

    public static bool operator >=(in PersianDateTime left, in DateTime right) =>
            left.CompareTo(right) >= 0;

    /// <summary>
    ///     Parses the date time.
    /// </summary>
    /// <param name="dateTime"> The date time. </param>
    /// <returns> </returns>
    public static PersianDateTime ParseDateTime(in DateTime dateTime) =>
        new(dateTime);

    /// <summary>
    ///     Parses the english string datetime.
    /// </summary>
    /// <param name="dateTimeString"> The date time string. </param>
    /// <returns> </returns>
    public static PersianDateTime ParseEnglish(in string dateTimeString)
    {
        Check.IfArgumentNotNull(dateTimeString);
        return DateTime.Parse(dateTimeString, CultureInfo.CurrentCulture).ToPersianDateTime();
    }

    /// <summary>
    ///     Parses the persian.
    /// </summary>
    /// <param name="dateTimeString"> The date time string. </param>
    /// <returns> </returns>
    /// <exception cref="ArgumentException">
    ///     not valid date - dateTimeString or not valid date - dateTimeString or not valid date -
    ///     dateTimeString or not valid date - dateTimeString or not valid date - dateTimeString
    ///     or not valid date - dateTimeString
    /// </exception>
    public static PersianDateTime ParsePersian(in string dateTimeString)
    {
        Check.IfArgumentNotNull(dateTimeString);

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
    ///     Subtracts the specified persian date time1.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> </returns>
    public static PersianDateTime Subtract(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        => persianDateTime1 - persianDateTime2;

    /// <summary>
    ///     Converts to persian.
    /// </summary>
    /// <param name="dateTime"> The date time. </param>
    /// <returns> </returns>
    public static string ToPersian(in DateTime dateTime)
        => new PersianDateTime(dateTime).ToString()!;

    /// <summary>
    ///     Tries to parse a string to PersianDateTime.
    /// </summary>
    /// <param name="str"> The STR. </param>
    /// <param name="result"> The result. </param>
    /// <returns> </returns>
    public static bool TryParse(in string str, out PersianDateTime result)
    {
        result = Now;
        try
        {
            result = ParsePersian(str);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static TryMethodResult<PersianDateTime> TryParse(in string str) =>
        new(TryParse(str, out var result), result);

    /// <summary>
    ///     Adds the specified PersianDate time.
    /// </summary>
    /// <param name="persianDateTime"> The PersianDate time. </param>
    /// <returns> </returns>
    public PersianDateTime Add(in PersianDateTime persianDateTime)
        => Add(this, persianDateTime);

    public PersianDateTime AddDays(in int value)
        => this.ToDateTime().AddDays(value).ToPersianDateTime();

    public PersianDateTime AddHours(in int value)
        => this.ToDateTime().AddHours(value).ToPersianDateTime();

    public PersianDateTime AddMilliseconds(in int value)
        => this.ToDateTime().AddMilliseconds(value).ToPersianDateTime();

    public PersianDateTime AddMinutes(in int value)
        => this.ToDateTime().AddMinutes(value).ToPersianDateTime();

    public PersianDateTime AddMonths(in int value)
        => this.ToDateTime().AddMonths(value).ToPersianDateTime();

    public PersianDateTime AddSeconds(in int value)
        => this.ToDateTime().AddSeconds(value).ToPersianDateTime();

    public PersianDateTime AddTocks(in long ticks)
        => this.ToDateTime().AddTicks(ticks).ToPersianDateTime();

    public PersianDateTime AddYears(in int value)
                                    => this.ToDateTime().AddYears(value).ToPersianDateTime();

    /// <summary>
    ///     Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns> A new object that is a copy of this instance. </returns>
    public object Clone() =>
        new PersianDateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second, this.Millisecond);

    /// <summary>
    ///     Compares the current instance with another object of the same type and returns an
    ///     integer that indicates whether the current instance precedes, follows, or occurs in
    ///     the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other"> An object to compare with this instance. </param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared. The return
    ///     value has these meanings: Value Meaning Less than zero This instance precedes
    ///     <paramref name="other" /> in the sort order. Zero This instance occurs in the same
    ///     position in the sort order as <paramref name="other" />. Greater than zero This
    ///     instance follows <paramref name="other" /> in the sort order.
    /// </returns>
    public int CompareTo(PersianDateTime other) =>
        DateTime.Compare(this, other);

    public int CompareTo(object? obj) =>
            obj is PersianDateTime pdt ? this.CompareTo(pdt) : 1;

    public int CompareTo(DateTime other) =>
            ((PersianDateTime)other).CompareTo(this);

    public DateTime ConvertTo() => this;

    string IConvertible<PersianDateTime, string>.ConvertTo() => this;

    /// <summary>
    ///     Deconstructs this instance.
    /// </summary>
    /// <param name="hour"> The hour. </param>
    /// <param name="minute"> The minute. </param>
    /// <param name="second"> The second. </param>
    /// <param name="millisecond"> The millisecond. </param>
    public void Deconstruct(out int hour, out int minute, out int second, out double millisecond)
    {
        hour = this.Hour;
        minute = this.Minute;
        second = this.Second;
        millisecond = this.Millisecond;
    }

    /// <summary>
    ///     Deconstructs this instance.
    /// </summary>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month. </param>
    /// <param name="day"> The day. </param>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = this.Year;
        month = this.Month;
        day = this.Day;
    }

    /// <summary>
    ///     Deconstructs this instance.
    /// </summary>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month. </param>
    /// <param name="day"> The day. </param>
    /// <param name="hour"> The hour. </param>
    /// <param name="minute"> The minute. </param>
    /// <param name="second"> The second. </param>
    /// <param name="millisecond"> The millisecond. </param>
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
    ///     Determines whether the specified <see cref="object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj"> The <see cref="object" /> to compare with this instance. </param>
    /// <returns>
    ///     <c> true </c> if the specified <see cref="object" /> is equal to this instance;
    ///     otherwise, <c> false </c>.
    /// </returns>
    public override bool Equals(object? obj) =>
        obj is PersianDateTime target && this.CompareTo(target) == 0;

    public bool Equals(PersianDateTime other) =>
        this.CompareTo(other) == 0;

    public bool Equals(DateTime other) =>
            ((PersianDateTime)other).Equals(this);

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     A hash code for this instance, suitable for use in hashing algorithms and data
    ///     structures like a hash table.
    /// </returns>
    public override int GetHashCode() =>
        this.Data.GetHashCode();

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) =>
            info.AddValue("Data", this.Data);

    TypeCode IConvertible.GetTypeCode() =>
            throw new NotSupportedException();

    bool IConvertible.ToBoolean(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    byte IConvertible.ToByte(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    char IConvertible.ToChar(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    /// <summary>
    ///     Converts to persian date string.
    /// </summary>
    /// <param name="separator"> The separator. </param>
    /// <returns> </returns>
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
    ///     Converts to the DateTime.
    /// </summary>
    /// <returns> </returns>
    public DateTime ToDateTime() => this;

    /// <summary>
    ///     Converts the value of this instance to an equivalent <see cref="T:System.DateTime" />
    ///     using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">
    ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
    ///     culture-specific formatting information.
    /// </param>
    /// <returns>
    ///     A <see cref="T:System.DateTime" /> instance equivalent to the value of this instance.
    /// </returns>
    public DateTime ToDateTime(IFormatProvider? provider) =>
        this;

    decimal IConvertible.ToDecimal(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    public string? ToDefaultFomratString()
            => this.ToString("yyyy/MM/dd HH:mm:ss");

    double IConvertible.ToDouble(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    short IConvertible.ToInt16(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    int IConvertible.ToInt32(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    long IConvertible.ToInt64(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    sbyte IConvertible.ToSByte(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    float IConvertible.ToSingle(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    /// <summary>
    ///     Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns> A <see cref="string" /> that represents this instance. </returns>
    public override string ToString() =>
        this.ToString(ToStringFormat);

    /// <summary>
    ///     Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <param name="format"> The format. </param>
    /// <returns> A <see cref="string" /> that represents this instance. </returns>
    public string ToString(in string format)
    {
        var buffer = format;
        if (this.IsInitiated is false)
        {
            return this.ToString();
        }

        Check.IfArgumentNotNull(buffer, "format");
        var isPm = this.Hour > 12;
        var tmpHrs = isPm ? this.Hour - 12 : this.Hour;
        buffer = buffer.Trim().Replace("yyyy", this.Year.ToString("0000", CultureInfo.CurrentCulture));
        buffer = buffer.Trim().Replace("MMMM", MonthNames.ToArray()[this.Month - 1]);
        buffer = buffer.Trim().Replace("MM", this.Month.ToString("00", CultureInfo.CurrentCulture));
        buffer = buffer.Trim().Replace("dd", this.Day.ToString("00", CultureInfo.CurrentCulture));
        buffer = buffer.Trim().Replace("HH", this.Hour.ToString("00", CultureInfo.CurrentCulture));
        buffer = buffer.Trim().Replace("hh", tmpHrs.ToString("00", CultureInfo.CurrentCulture));
        buffer = buffer.Trim().Replace("mm", this.Minute.ToString("00", CultureInfo.CurrentCulture));
        buffer = buffer.Trim().Replace("ss", this.Second.ToString("00", CultureInfo.CurrentCulture));
        buffer = buffer.Trim().Replace("tt", isPm ? CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator : CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator);

        return buffer;
    }

    string IConvertible.ToString(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    public string ToString(string? format, IFormatProvider? formatProvider)
            => throw new NotImplementedException();

    /// <summary>
    ///     Converts to persian time string.
    /// </summary>
    /// <param name="separator"> The separator. </param>
    /// <returns> </returns>
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
            => throw RaiseInvalidTypeCastException();

    ushort IConvertible.ToUInt16(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    uint IConvertible.ToUInt32(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    ulong IConvertible.ToUInt64(IFormatProvider? provider) =>
            throw RaiseInvalidTypeCastException();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
            => throw new NotImplementedException();

    /// <summary>
    ///     Raises the invalid type cast exception.
    /// </summary>
    /// <returns> </returns>
    [DoesNotReturn]
    private static InvalidCastException RaiseInvalidTypeCastException()
    {
        var targetType = CodeHelper.GetCallerMethodName()![2..];
        throw new InvalidCastException($"Unable to cast PersianDateTime to {targetType}");
    }

    private string GetDebuggerDisplay() =>
        this.ToString()!;
}